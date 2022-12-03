using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SprintPS : PlayerState
{
    public SprintPS(Player _player) : base(_player)
    {
        name = PState.SPRINT;
    }


    public override void Enter()
    {
        base.Enter();

        if (!att.CanTruck)
            SetTrigger("Sprint");
        else
        {
            player.gameplay.isTrucking = true;
            SetTrigger("Truck");
        }

        player.playerManager.SprintUIAnimation();

        controller.Sprint();
    }

    public override void Update()
    {
        base.Update();

        PlayerOrientation();

        controller.Speed = att.NormalSpeed * (1 - (1 - att.AccelerationM) * acc);

        controller.SideSpeed = att.AccSideSpeed * side;

        // Jump
        if (UpSwipe)
        {
            if (att.CanHighKnee && controller.CanJump(att.HighKneeCost))
            {
                nextState = new HighKneePS(player);
                stage = Event.EXIT;
            }
            else if (att.CanHurdle && controller.CanJump(att.HurdleCost))
            {
                nextState = new HurdlePS(player);
                stage = Event.EXIT;
            }
        }
        else if (Jump && controller.CanJump(att.JumpCost))
        {
            nextState = new JumpPS(player);
            stage = Event.EXIT;
        }
        // Slide
        else if (DownSwipe)
        {
            if (att.CanSlide && !controller.AlreadySlide)
            {
                nextState = new SlidePS(player);
                stage = Event.EXIT;
            }
            else if (att.CanSprintFeint)
            {
                nextState = new SprintFeintPS(player);
                stage = Event.EXIT;
            }
        }
        if (stage != Event.EXIT)
        {
            // Run
            if ((acc <= 0 && side == 0) || (!controller.CanAccelerate && side == 0))
            {
                nextState = new RunPS(player, true);
                stage = Event.EXIT;
            }
            // Siderun
            else if ((acc <= 0 && side != 0) || (!controller.CanAccelerate && side != 0))
            {
                nextState = new SiderunPS(player, side / Mathf.Abs(side),true, true);
                stage = Event.EXIT;
            }
        }
    }

    public override void Exit()
    {
        ResetTrigger("Sprint");

        player.gameplay.isTrucking = false;
        
        base.Exit();
    }
}
