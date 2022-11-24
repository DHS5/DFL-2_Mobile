using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChaseCZS : ClassicZState
{
    public ChaseCZS(ClassicZombie _enemy, NavMeshAgent _agent, Animator _animator) : base(_enemy, _agent, _animator)
    {
        name = EState.CHASE;
    }

    public override void Enter()
    {
        base.Enter();

        animator.SetTrigger("Run");
    }

    public override void Update()
    {
        base.Update();

        enemy.destination = enemy.playerPosition + enemy.playerVelocity;


        if (enemy.rawDistance < att.attackDist)
        {
            nextState = new AttackCZS(enemy, agent, animator);
            stage = Event.EXIT;
        }
    }

    public override void Exit()
    {
        base.Exit();

        animator.ResetTrigger("Run");
    }
}
