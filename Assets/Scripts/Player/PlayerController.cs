using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

/// <summary>
/// Gives the user control of the player
/// The user can accelerate, decelerate, go on the sides and look on the sides (without changing the running orientation)
/// </summary>
public class PlayerController : MonoBehaviour
{
    [Tooltip("Player script")]
    private Player player;

    [Tooltip("Touch Manager script")]
    [HideInInspector] public TouchManager touchManager;


    public PlayerState CurrentState { get; private set; }



    [Tooltip("Rigidbody of the player")]
    public Rigidbody PlayerRigidbody { get; private set; }


    [Tooltip("Gravity multiplier constant")]
    readonly float gravityScale = 6f;
    [Tooltip("Jump multiplier constant")]
    readonly float jumpCst = 1.25f;



    [HideInInspector] public PlayerAttributesSO playerAtt;
    public PlayerUniversalDataSO playerUD;



    private float realDir;
    private float realAcc;
    private float rawDir;
    private float rawAcc;


    /// <summary>
    /// Velocity of the player
    /// </summary>
    public Vector3 Velocity { get; private set; }

    [Tooltip("Current speed of the player")]
    private float speed;

    [Tooltip("Current side speed of the player")]
    private float sideSpeed;


    [Tooltip("Bonus speed attribute of the player (changed by the bonus)")]
    [HideInInspector] public float bonusSpeed = 0f;
    [Tooltip("Bonus jump attribute of the player (changed by the bonus)")]
    [HideInInspector] public Vector3 bonusJump = Vector3.zero;


    [Tooltip("Current charge of the jump (between 0 and playerAtt.jumpStamina")]
    private float jumpCharge;


    private bool isRaining;


    readonly float checkboxSize = 1.5f;
    readonly float checkboxThreshold = 0.25f;
    private Vector3 checkBox;
    private int groundLayerMask;

    // Player state variables
    public bool OnGround { get; private set; }
    public bool CanAccelerate { get; private set; }
    public bool AlreadySlide { get; private set; }
    public bool Sprinting { get; private set; }
    public float SprintStartTime { get; private set; }



    // ### Properties ###

    // Control parameters
    private float DirSensitivity
    {
        get { return isRaining ? playerAtt.DirSensitivity / 2 : playerAtt.DirSensitivity; }
    }
    private float DirGravity
    {
        get { return isRaining ? playerAtt.DirGravity / 2 : playerAtt.DirGravity; }
    }
    private float AccSensitivity
    {
        get { return isRaining ? playerAtt.AccSensitivity / 2 : playerAtt.AccSensitivity; }
    }
    private float AccGravity
    {
        get { return isRaining ? playerAtt.AccGravity / 2 : playerAtt.AccGravity; }
    }
    private float Snap
    {
        get { return playerAtt.snap; }
    }




    public float Direction
    {
        get { return realDir; }
    }
    public float Acceleration
    {
        get { return realAcc; }
    }

    public float Speed
    {
        get { return speed + bonusSpeed; }
        set { speed = value; }
    }

    public float FSpeed
    {
        get { return Mathf.Sqrt(Speed * Speed - SideSpeed * SideSpeed); }
    }

    public float SideSpeed
    {
        get { return sideSpeed; }
        set { sideSpeed = value; }
    }

    // ### Base functions ###

    private void Awake()
    {
        player = GetComponent<Player>();

        PlayerRigidbody = GetComponent<Rigidbody>();

        //GetControlParams();
    }


    private void Start()
    {
        PlayerRescale();

        CurrentState = new RunPS(player, true);
        CurrentState.Flashlight(player.playerManager.FlashlightActive);

        CanAccelerate = true;
        AlreadySlide = false;

        jumpCharge = playerAtt.JumpStamina;

        checkBox = new Vector3(checkboxSize, checkboxThreshold, checkboxSize);
        groundLayerMask = LayerMask.GetMask("Ground", "NavMesh");
    }

    private void Update()
    {
        FilterDir();
        FilterAcc();

        if (!player.gameplay.freeze)
            CurrentState = CurrentState.Process();

        Velocity = ( Vector3.forward * FSpeed + Vector3.right * SideSpeed ) * Time.deltaTime;

        RechargeJump();
    }

    private void FixedUpdate()
    {
        // Keep the gravity constant
        PlayerRigidbody.AddForce(Physics.gravity * gravityScale);
    }

    void LateUpdate()
    {
        if (!player.gameplay.freeze)
        {
            transform.Translate(Velocity); // Makes the player run
        }
    }


    // ### Functions ###
    private void PlayerRescale()
    {
        var fpScale = player.fPPlayer.gameObject.transform.localScale;
        var tpScale = player.tPPlayer.gameObject.transform.localScale;
        player.fPPlayer.gameObject.transform.localScale = new Vector3(fpScale.x * playerAtt.size.x, fpScale.y * playerAtt.size.y, fpScale.z * playerAtt.size.z);
        player.tPPlayer.gameObject.transform.localScale = new Vector3(tpScale.x * playerAtt.size.x, tpScale.y * playerAtt.size.y, tpScale.z * playerAtt.size.z);
    }


    // # Handling #

    private void FilterDir()
    {
        rawDir = touchManager.Side;

        if (Mathf.Abs(realDir - rawDir) <= Snap)
        {
            SnapDir();
            return;
        }

        //if (rawDir == 0)
        //{
        //    int side = realDir > 0 ? -1 : 1;
        //    realDir += side * DirGravity * Time.deltaTime;
        //    if (realDir * side < 0) realDir = 0;
        //}
        //else
        //{
        //    if (rawDir * realDir <= 0 || Mathf.Abs(realDir) < Mathf.Abs(rawDir))
        //    {
        //        realDir += (rawDir > 0 ? 1 : -1) * DirSensitivity * Time.deltaTime;
        //    }
        //    else
        //    {
        //        realDir += (realDir > 0 ? -1 : 1) * DirGravity * Time.deltaTime;
        //    }
        //}


        realDir = Mathf.Lerp(realDir, rawDir, DirSensitivity * Time.deltaTime);

        if (Mathf.Abs(realDir - rawDir) <= Snap) SnapDir();

        realDir = Mathf.Clamp(realDir, -1, 1);
    }
    private void FilterAcc()
    {
        rawAcc = touchManager.Acc;

        if (Mathf.Abs(realAcc - rawAcc) <= Snap + AccGravity * Time.deltaTime)
        {
            SnapAcc();
            return;
        }

        realAcc = Mathf.Lerp(realAcc, rawAcc, AccSensitivity * 2 * Time.deltaTime);

        if (Mathf.Abs(realAcc - rawAcc) <= Snap) SnapAcc();

        realAcc = Mathf.Clamp(realAcc, -1, 1);
    }

    public void SnapDir() { realDir = touchManager.Side; }
    public void SnapAcc() { realAcc = touchManager.Acc; }

    public void FullDir(float side) { realDir = side / Mathf.Abs(side); }


    // # Jump #

    /// <summary>
    /// Detects a collision with the ground to know if the player is on the ground
    /// </summary>
    /// <param name="collision">Collider of the colliding game object</param>
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            OnGround = true;
            player.playerManager.JumpButtonColor(false);
        }
    }

    public bool TouchGround()
    {
        return Physics.CheckBox(player.transform.position, checkBox, Quaternion.identity, groundLayerMask);
    }

    public bool CanJump(float cost)
    {
        return OnGround && cost <= jumpCharge && TouchGround();
    }

    private void RechargeJump()
    {
        jumpCharge = Mathf.Clamp(jumpCharge + Time.deltaTime * (playerAtt.JumpStamina / playerAtt.JumpRechargeTime), 0, playerAtt.JumpStamina);
    }




    /// <summary>
    /// Jump power of the player
    /// </summary>
    public Vector3 JumpPower(float height)
    {
        return new Vector3(0, Mathf.Sqrt(height * jumpCst * -2 * (Physics.gravity.y * gravityScale)), 0);
    }
    /// <summary>
    /// Triggers the player's jump
    /// </summary>
    /// <param name="cost"></param>
    /// <returns>Hang time</returns>
    public float Jump(float cost, float bonusHeight)
    {
        float height = playerAtt.JumpHeight + bonusHeight + bonusJump.y;

        PlayerRigidbody.AddForce(JumpPower(height), ForceMode.Impulse);
        OnGround = false;
        jumpCharge -= cost;

        player.playerManager.JumpButtonColor(true);

        return 0.5f + 0.09f * (height - 2);
    }



    // # Skills #
    public void Sprint() { if (!Sprinting) { Sprinting = true; SprintStartTime = Time.time; Invoke(nameof(Rest), playerAtt.accelerationTime); } }
    private void Rest() { Sprinting = false; CanAccelerate = false; Invoke(nameof(Rested) , playerAtt.accelerationRestTime) ; }
    private void Rested() { CanAccelerate = true; AlreadySlide = false; }
    public void Slide() { AlreadySlide = true; }


    // # Weather #
    public void Rain()
    {
        isRaining = true;
    }
}
