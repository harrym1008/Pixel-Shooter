using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy_Imp : Enemy
{
    [Header("Imp Parameters")]
    [SerializeField] float sightDistance;
    [SerializeField] float attackTime;
    [SerializeField] float attackWaitBeforeSpawn;
    [SerializeField] Vector3 spawnLocation;
    [SerializeField] GameObject impFireball;
    [SerializeField] LayerMask attackLayerMask;


    public override void Start()
    {
        base.Start();
        bloodType = BloodType.Crimson;
    }


    public override void Die()
    {
        enemyMovement.SetMovement(false);
        GetComponent<Animator>().SetBool("Dead", true);
        GetComponent<Animator>().SetTrigger("Die");

        GetComponent<Collider>().enabled = false;

        StopCoroutine(Attack());
    }

    public override void ChangeTarget(Target target)
    {
        enemyMovement.ChangeTarget(target.transform);
    }

    public override void StateToAttacking(Transform newTarget)
    {
        base.StateToAttacking(newTarget);

        enemyMovement.targetInSight = true;
        StartCoroutine(ImpAttacks());
    }

    public override void StateToWander()
    {
        enemyMovement.targetInSight = false;
        StopCoroutine(ImpAttacks());
    }

    IEnumerator ImpAttacks()
    {
        float waiting = AttackWaitTime();

        while (true)
        {
            if (attackingTarget == null || myTarget.isDead)
                break;

            waiting -= Time.deltaTime;

            if (waiting <= 0f)
            {
                waiting = AttackWaitTime();

                if (AttackCheck())
                {
                    waiting *= 0.5f;
                    enemyMovement.SetMovement(false);
                    StartCoroutine(Attack());
                    yield return Wait.Seconds(attackTime);
                    enemyMovement.SetMovement(true);
                }

            }

            yield return Wait.Frame;
        }

        yield return Wait.Frame;
    }


    bool AttackCheck()
    {
        return LineOfSightCheck(attackingTarget.position, sightDistance, attackLayerMask);
    }

    IEnumerator Attack()
    {
        animator.SetBool("Attacking", true);

        float until = attackWaitBeforeSpawn;
        do
        {
            FaceTarget();
            until -= Time.deltaTime;
            yield return Wait.Frame;

        } while (until > 0);

        animator.SetBool("Attacking", false);

        Vector3 spawnAt = transform.position + 
            spawnLocation.x * transform.right +
            spawnLocation.y * transform.up +
            spawnLocation.z * transform.forward;

        FaceTarget(true);

        ImpFireball ball = Instantiate(impFireball, spawnAt, transform.rotation).GetComponent<ImpFireball>();
        ball.spawner = transform;

        FaceTarget(false);
    }


    float AttackWaitTime()
    {
        switch (EnemyManager.difficulty)
        {
            case EnemyManager.Difficulty.High:
                return RNG.Range(0.4f, 1.1f);
            case EnemyManager.Difficulty.Low:
                return RNG.Range(3f, 4.2f);
            default:
                return RNG.Range(1.5f, 2.8f);
        }
    }

    void FaceTarget(bool allAngles = false)
    {
        transform.LookAt(attackingTarget);

        if (!allAngles)
            transform.eulerAngles = new Vector3(0f, transform.eulerAngles.y, 0f);
    }
}
