using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TruckedES : EnemyState
{
    Vector3 impact;
    Vector3 impactDir;
    float impactPower;

    readonly float impactMin = 10;
    readonly float impactMax = 20;

    public TruckedES(Enemy _enemy, NavMeshAgent _agent, Animator _animator, Collision _collision) : base(_enemy, _agent, _animator)
    {
        name = EState.TRUCKED;

        enemy = _enemy;

        agent.ResetPath();
        agent.isStopped = true;
        agent.updateRotation = false;

        impactDir = Vector3.Slerp(enemy.playerVelocity, -agent.velocity, 0.75f).normalized;
        impactPower = Mathf.Clamp(_collision.impulse.magnitude / Time.fixedDeltaTime, impactMin, impactMax);
        impact = impactPower * new Vector3(impactDir.x, 0, impactDir.z).normalized;

        enemy.transform.rotation = Quaternion.Euler(0, Quaternion.LookRotation(-impact).eulerAngles.y, 0);
        agent.velocity = impact;

        animator.SetLayerWeight(animator.GetLayerIndex("Trucked Layer"), 1);
        animator.SetTrigger("Trucked");
    }

    public override void Enter()
    {
        base.Enter();

        agent.ResetPath();
        agent.isStopped = true;

        agent.velocity = impact;
    }
}
