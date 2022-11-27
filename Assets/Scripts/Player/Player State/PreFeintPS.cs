using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreFeintPS : PlayerState
{
    public PreFeintPS(Player _player, float _side) : base(_player)
    {
        name = PState.PREFEINT;

        startSide = _side;
    }


    public override void Enter()
    {
        SetFloat("Dir", startSide);
        SetTrigger("Side");
        animTime = UD.siderunTime;

        controller.Speed = att.NormalSpeed;
        controller.SideSpeed = att.NormalSideSpeed * startSide;
        PlayerOrientation();

        base.Enter();
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

        if (Time.time >= startTime + animTime)
        {
            if (nextState != null) stage = Event.EXIT;
            // Sprint
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
            // Slow
            else if (acc < 0 && side == 0)
            {
                nextState = new SlowrunPS(player);
                stage = Event.EXIT;
            }
            // Other side
            else if (side != 0)
            {
                nextState = new SiderunPS(player, side / Mathf.Abs(side), false, false);
                stage = Event.EXIT;
            }
            // Run
            else
            {
                nextState = new RunPS(player, false);
                stage = Event.EXIT;
            }
        }
        if ((rawSide * startSide < 0) || (startSide < 0 && RightSwipe) || (startSide > 0 && LeftSwipe))
        {
            nextState = new FeintPS(player, -startSide);
            SetFloat("Dir", -startSide);
            SetTrigger("Feint");
        }
    }

    public override void Exit()
    {
        ResetTrigger("Side");

        base.Exit();
    }
}
