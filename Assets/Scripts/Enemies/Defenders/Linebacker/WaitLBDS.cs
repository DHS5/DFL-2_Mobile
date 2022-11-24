using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WaitLBDS : LinebackerState
{
    public WaitLBDS(LineBacker _enemy, NavMeshAgent _agent, Animator _animator) : base(_enemy, _agent, _animator)
    {
        name = EState.WAIT;
    }

    public override void Enter()
    {
        base.Enter();

        animator.SetTrigger("Wait");
    }

    public override void Update()
    {
        base.Update();

        agent.isStopped = true;


        if (enemy.playerOnField && enemy.zDistance < att.waitDist)
        {
            // Positionning
            nextState = new PositionningLBDS(enemy, agent, animator);
            stage = Event.EXIT;
        }
    }

    public override void Exit()
    {
        base.Exit();

        agent.isStopped = false;

        animator.ResetTrigger("Wait");
    }
}
