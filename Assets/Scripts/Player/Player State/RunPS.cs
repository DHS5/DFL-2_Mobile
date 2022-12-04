using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunPS : PlayerState
{
    private bool anim;
    public RunPS(Player _player, bool _anim) : base(_player)
    {
        name = PState.RUN;

        anim = _anim;
    }

    public override void Enter()
    {
        base.Enter();

        if (anim)
        {
            SetTrigger("Run");
        }
        SetFloat("Dir", 0f);

        controller.Speed = att.NormalSpeed;
        controller.SideSpeed = 0f;
    }

    public override void Update()
    {
        base.Update();

        PlayerOrientation();


        // Jump
        if (Jump && controller.CanJump(att.JumpCost))
        {
            nextState = new JumpPS(player);
            stage = Event.EXIT;
        }
        // Acceleration
        else if (acc > 0 && controller.CanAccelerate)
        {
            nextState = new SprintPS(player);
            stage = Event.EXIT;
        }
        // Slowsiderun
        else if (acc < 0 && side != 0)
        {
            nextState = new SlowsiderunPS(player, side / Mathf.Abs(side), false);
            stage = Event.EXIT;
        }
        // Pre Feint
        else if (att.CanFeint && (LeftSwipe || RightSwipe))// && Time.time >= startTime + UD.siderunDelay)
        {
            nextState = new PreFeintPS(player, LeftSwipe ? -1 : 1);
            stage = Event.EXIT;
        }
        // Siderun
        else if (side != 0)
        {
            nextState = new SiderunPS(player, side / Mathf.Abs(side), false, false);// att.CanFeint && Time.time >= startTime + UD.siderunDelay);
            stage = Event.EXIT;
        }
        // Slow
        else if (acc < 0)
        {
            nextState = new SlowrunPS(player);
            stage = Event.EXIT;
        }
        
    }

    public override void Exit()
    {
        ResetTrigger("Run");

        base.Exit();
    }
}
