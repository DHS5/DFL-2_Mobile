using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SprintFeintPS : PlayerState
{
    public SprintFeintPS(Player _player) : base(_player)
    {
        name = PState.SPRINTFEINT;
    }


    public override void Enter()
    {
        base.Enter();

        SetTrigger("SprintFeint");

        player.effects.AudioPlayerEffort(true);

        controller.Speed = att.SprintFeintSpeed;
        controller.SideSpeed = Vector3.Dot(controller.Velocity.normalized * att.SprintFeintSpeed, Vector3.right);

        SlowMotion(UD.sprintFeintTime, 7.5f, 2);
    }

    public override void Update()
    {
        base.Update();

        if (Time.time > startTime + 0.275f && acc < 0)
        {
            nextState = new SlowrunPS(player);
            stage = Event.EXIT;
        }
        if (Time.time > startTime + UD.sprintFeintTime)
        {
            // Sprint
            if (acc > 0 && controller.CanAccelerate)
                nextState = new SprintPS(player);
            // Siderun
            else if (acc == 0 && side != 0)
                nextState = new SiderunPS(player, side / Mathf.Abs(side), true, true);
            // Slowsiderun
            else if (acc < 0 && side != 0)
                nextState = new SlowsiderunPS(player, side / Mathf.Abs(side), true);
            // Slow
            else if (acc < 0)
                nextState = new SlowrunPS(player);
            // Run
            else
                nextState = new RunPS(player, true);

            stage = Event.EXIT;
        }
    }

    public override void Exit()
    {
        ResetTrigger("SprintFeint");

        base.Exit();
    }
}
