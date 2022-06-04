using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy_Imp : Enemy
{
    public override void Die()
    {
        GetComponent<Animator>().SetBool("Dead", true);
        GetComponent<Animator>().SetTrigger("Die");

        GetComponent<Collider>().enabled = false;
        GetComponent<NavMeshAgent>().enabled = false;
    }

    public override void ChangeTarget(Target target)
    {
        enemyMovement.ChangeTarget(target.transform);
    }

    public override void StateToAttacking()
    {
        enemyMovement.targetInSight = true;
    }

    public override void StateToWander()
    {
        enemyMovement.targetInSight = false;
    }
}
