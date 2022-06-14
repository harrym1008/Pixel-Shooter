using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy_Chaingunner : Enemy
{
    [Header("Chaingunner Parameters")]
    [SerializeField] float sightDistance;
    [SerializeField] float timeBetweenBullets;
    [SerializeField] float attackWaitBeforeSpawn;

    [SerializeField] Vector3 spawnLocation;
    [SerializeField] GameObject bulletObject;
    [SerializeField] BulletData bulletData;
    [SerializeField] Vector2 bulletSpread;
    [SerializeField] LayerMask attackLayerMask;

    bool firing = false;


    public override void Start()
    {
        base.Start();
        bloodType = BloodManager.BloodType.Crimson;

        animator.SetFloat("DieSpeedFactor", RNG.Range(0.8f, 1.2f));
    }


    public override void Die()
    {
        transform.Find("Sprite").GetComponent<SpriteAngle>().turnOffFlipping = true;

        enemyMovement.SetMovement(false);
        GetComponent<Animator>().SetBool("Dead", true);
        GetComponent<Animator>().SetTrigger("Die");

        GetComponent<Collider>().enabled = false;

        StopCoroutine(ChaingunnerAttacks());

        base.Die();
    }

    public override void ChangeTarget(Target target)
    {
        enemyMovement.ChangeTarget(target.transform);
    }

    public override void StateToAttacking(Transform newTarget)
    {
        base.StateToAttacking(newTarget);

        enemyMovement.targetInSight = true;
        StartCoroutine(ChaingunnerAttacks());
    }

    public override void StateToWander()
    {
        enemyMovement.targetInSight = false;
        StopCoroutine(ChaingunnerAttacks());
    }

    IEnumerator ChaingunnerAttacks()
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
                    firing = true;
                    enemyMovement.SetMovement(false);

                    StartCoroutine(StartAttack());
                    firing = true;

                    yield return new WaitUntil(() => !firing);

                    if (myTarget.isDead)
                        yield break;

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

    IEnumerator StartAttack()
    {
        animator.SetBool("Attacking", true);

        float untilNextBullet = timeBetweenBullets;

        while (AttackCheck() && !myTarget.isDead)
        {
            untilNextBullet -= Time.deltaTime;

            if (untilNextBullet <= 0f)
            {
                /*transform.eulerAngles = new Vector3(FaceTargetAngle().x, 
                    transform.eulerAngles.y, transform.eulerAngles.z);*/

                FaceTarget(true);

                untilNextBullet = timeBetweenBullets - Time.deltaTime;
                
                Vector3 spawnAt = transform.position +
                    spawnLocation.x * transform.right +
                    spawnLocation.y * transform.up +
                    spawnLocation.z * transform.forward;

                Vector3 spread = RNG.RandomVector3(bulletSpread.y, bulletSpread.x, 0f);

                BulletBehaviour bullet = Instantiate(bulletObject, spawnAt, 
                    Quaternion.Euler(transform.eulerAngles + spread)).GetComponent<BulletBehaviour>();

                bullet.velocity = 180f;
                bullet.effectiveRange = 32;

                bullet.bulletData = bulletData;
            }

            FaceTarget();
            // TurnTowards(180f * Time.deltaTime);
            yield return Wait.Frame;
        }

        firing = false;

        animator.SetBool("Attacking", false);



               
    }

    float AttackWaitTime()
    {
        switch (EnemyManager.difficulty)
        {
            case EnemyManager.Difficulty.High:
                return RNG.Range(0.5f, 1f);
            case EnemyManager.Difficulty.Low:
                return RNG.Range(2f, 3.2f);
            default:
                return RNG.Range(1f, 1.6f);
        }
    }

    void FaceTarget(bool allAngles = false)
    {
        transform.LookAt(attackingTarget);

        if (!allAngles)
            transform.eulerAngles = new Vector3(0f, transform.eulerAngles.y, 0f);
    }


    Vector3 FaceTargetAngle(bool allAngles = false)
    {
        Quaternion x = transform.rotation;
        FaceTarget(allAngles);
        Quaternion y = transform.rotation;
        transform.rotation = x;
        return y.eulerAngles;
    }



    Vector3 TurnTowards(float maxDegrees, bool allAngles = false)
    {
        Quaternion x = transform.rotation;

        FaceTarget(allAngles);
        Quaternion y = transform.rotation;

        transform.rotation = x;
        return Quaternion.RotateTowards(x, y, maxDegrees).eulerAngles;
    }

}
