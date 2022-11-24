using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChaseLBDS : LinebackerState
{
    public ChaseLBDS(LineBacker _enemy, NavMeshAgent _agent, Animator _animator) : base(_enemy, _agent, _animator)
    {
        name = EState.CHASE;

        Intercept();
        //enemy.destination = enemy.playerPosition + PlayerDir * (att.anticipation + Mathf.Abs(enemy.xDistance) * att.intelligence);
    }

    public override void Enter()
    {
        base.Enter();

        animator.SetTrigger("Run");

        Intercept();
        //enemy.destination = enemy.playerPosition + PlayerDir * (att.anticipation + Mathf.Abs(enemy.xDistance) * att.intelligence);
    }

    public override void Update()
    {
        base.Update();

        //enemy.destination = enemy.playerPosition + PlayerDir * ((enemy.toPlayerAngle < 90 ? att.anticipation : 0) + Mathf.Abs(enemy.xDistance) * att.intelligence);
        Intercept();

        // Attack
        if (CanAttack())
        {
            nextState = new AttackLBDS(enemy, agent, animator);
            stage = Event.EXIT;
        }
    }

    public override void Exit()
    {
        base.Exit();

        animator.ResetTrigger("Run");
    }

    private void Intercept()
    {
        float distP;

        float speedC = (enemy.playerSpeed != 0 ? enemy.playerSpeed : 0.1f) / agent.speed;

        float A = 1 - (1 / (speedC * speedC));

        float B = -Mathf.Cos(Vector3.Angle(-enemy.toPlayerDirection, enemy.playerVelocity) * Mathf.Deg2Rad) * 2 * enemy.rawDistance;

        float C = enemy.rawDistance * enemy.rawDistance;

        float delta = (B * B) - (4 * A * C);

        if (A == 0)
        {
            distP = C / -B;
            if (distP < 0 || B == 0)
                distP = att.anticipation;
        }

        else if (delta > 0)
        {
            distP = (-B - Mathf.Sqrt(delta)) / (2 * A);

            if (distP < 0)
            {
                distP = Mathf.Abs(-B + Mathf.Sqrt(delta)) / (2 * A);
            }
        }

        else if (delta == 0)
        {
            distP = -B / (2 * A);
        }

        else
        {
            distP = att.anticipation;
        }

        enemy.destination = enemy.playerPosition + distP * att.intelligence * enemy.playerVelocity;
    }
}
