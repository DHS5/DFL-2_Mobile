using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowrunPS : PlayerState
{
    public SlowrunPS(Player _player) : base(_player)
    {
        name = PState.SLOWRUN;
    }


    public override void Enter()
    {
        base.Enter();

        SetTrigger("Slow");

        controller.SideSpeed = 0;
    }

    public override void Update()
    {
        base.Update();

        PlayerOrientation();


        controller.Speed = att.NormalSpeed * ( 1 + (1 - att.SlowM) * acc );


        // Juke
        if (att.CanJuke && Time.time - startTime > UD.jukeDelay && (LeftSwipe || RightSwipe))
        {
            nextState = new JukePS(player, LeftSwipe ? -1 : 1);
            stage = Event.EXIT;
        }
        // Flip
        else if (att.CanFlip && Time.time - startTime > UD.flipDelay && UpSwipe && controller.CanJump(att.FlipCost))
        {
            nextState = new FlipPS(player);
            stage = Event.EXIT;
        }
        // Jump
        else if (touch.Jump && controller.CanJump(att.JumpCost))
        {
            nextState = new JumpPS(player);
            stage = Event.EXIT;
        }
        // Sprint
        else if (acc > 0 && controller.CanAccelerate)
        {
            nextState = new SprintPS(player);
            stage = Event.EXIT;
        }
        // Run
        else if (acc == 0)
        {
            nextState = new RunPS(player, true);
            stage = Event.EXIT;
        }
        // Slowsiderun
        else if (side != 0)
        {
            nextState = new SlowsiderunPS(player, side / Mathf.Abs(side), true);
            stage = Event.EXIT;
        }
    }

    public override void Exit()
    {
        ResetTrigger("Slow");

        base.Exit();
    }
}
