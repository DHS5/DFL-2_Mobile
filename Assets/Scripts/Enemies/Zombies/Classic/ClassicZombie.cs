using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClassicZombie : Zombie
{
    public ClassicZAttributesSO Att { get; private set; }


    public override void GetAttribute(EnemyAttributesSO att)
    {
        base.GetAttribute(att);

        Att = Attribute as ClassicZAttributesSO;

        currentState = new WaitCZS(this, navMeshAgent, animator);
    }

    public override void ChasePlayer()
    {
        base.ChasePlayer();

        if (Attribute.reactivity != 0 && !gameOver && !dead)
        {
            Invoke(nameof(ChasePlayer), Attribute.reactivity);
        }
    }

    private void Update()
    {
        if (Attribute.reactivity == 0 && playerG.onField && !gameOver && !dead)
        {
            ChasePlayer();
        }
    }
}
