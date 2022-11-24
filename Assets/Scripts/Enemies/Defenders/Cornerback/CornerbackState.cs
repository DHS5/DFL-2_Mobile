using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CornerbackState : EnemyState
{
    protected CornerbackAttributesSO att;

    new protected Cornerback enemy;

    public CornerbackState(Cornerback _enemy, NavMeshAgent _agent, Animator _animator) : base(_enemy, _agent, _animator)
    {
        enemy = _enemy;

        att = enemy.Att;
    }

    protected bool CanAttack(float interceptTime)
    {
        if (enemy.toPlayerAngle > att.attackAngle) return false;
        if (!att.modeTime && enemy.rawDistance > att.attackDist) return false;
        if (att.modeTime && interceptTime > att.attackTime) return false;

        float speedC = enemy.playerSpeed / agent.speed;

        float A = 1 - (1 / (speedC * speedC));

        float B = -Mathf.Cos(Vector3.Angle(-enemy.toPlayerDirection, enemy.playerVelocity) * Mathf.Deg2Rad) * 2 * enemy.rawDistance;

        float C = enemy.rawDistance * enemy.rawDistance;

        float delta = (B * B) - (4 * A * C);

        if (A == 0 || delta < 0) return false;

        return true;
    }
}
