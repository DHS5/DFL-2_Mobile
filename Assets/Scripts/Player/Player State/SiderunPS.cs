using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SiderunPS : PlayerState
{
    private bool anim;
    private bool canFeint;

    public SiderunPS(Player _player, float _side, bool _anim, bool _canFeint) : base(_player)
    {
        name = PState.SIDERUN;

        startSide = _side;
        anim = _anim;
        canFeint = _canFeint;
    }


    public override void Enter()
    {
        base.Enter();

        //SetFloat("Dir", startSide);
        //if (canFeint)
        //{
        //    SetTrigger("Side");
        //    animTime = UD.siderunTime;
        //}
        //else 
        if (anim) SetTrigger("Run");

        controller.Speed = att.NormalSpeed;
    }


    public override void Update()
    {
        base.Update();

        PlayerOrientation();


        controller.SideSpeed = att.NormalSideSpeed * side;

        // Jump
        if (Jump && controller.CanJump(att.JumpCost))
        {
            nextState = new JumpPS(player);
            stage = Event.EXIT;
        }

        else if (!canFeint || Time.time >= startTime + animTime)
        {
            if (nextState == null)
            {
                // Sprint
                if (acc > 0 && controller.CanAccelerate)
                {
                    nextState = new SprintPS(player);
                    stage = Event.EXIT;
                }
                // Slowsiderun
                else if (acc < 0 && side * startSide > 0)
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
                else if (side * startSide < 0)
                {
                    nextState = new SiderunPS(player, -startSide, false, false);
                    stage = Event.EXIT;
                }
                // Run
                else if (side == 0)
                {
                    nextState = new RunPS(player, false);
                    stage = Event.EXIT;
                }
            }
            else stage = Event.EXIT;
        }
    }

    public override void Exit()
    {
        //ResetTrigger("Side");
        ResetTrigger("Run");

        base.Exit();
    }
}
