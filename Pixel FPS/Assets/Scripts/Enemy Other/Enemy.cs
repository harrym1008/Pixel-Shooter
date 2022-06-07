using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Target myTarget;
    protected Transform attackingTarget = null;
    protected EnemyMovement enemyMovement;
    protected Animator animator;

    [Header("Looking for player parameters")]
    public bool targetInSight = false;
    public bool currentlyInfighting = false;
    [SerializeField] float seeOffsetY;
    [SerializeField] float maxSeeDistance;
    [SerializeField] protected LayerMask seeingLayerMask;


    private void Start()
    {
        myTarget = GetComponent<Target>();
        enemyMovement = GetComponent<EnemyMovement>();
        animator = GetComponent<Animator>();
    }


    public virtual void LookForPlayer(Transform player)
    {
        if (LineOfSightCheck(player.position, maxSeeDistance))
        {
            ChangeTarget(player.GetComponent<Target>());
            StateToAttacking(player);
        }
    }


    protected bool LineOfSightCheck(Vector3 endLocation, float maxDistance = 0f)
    {
        bool line = Physics.Linecast(transform.position + seeOffsetY * transform.up, endLocation, seeingLayerMask);
        bool distance = Vector3.Distance(endLocation, transform.position) <= maxDistance;
        if (maxDistance == 0f) { distance = true; }

        return !line && distance;
    }


    public virtual void StateToWander()
    {
        attackingTarget = null;
        targetInSight = false;
    }

    public virtual void StateToAttacking(Transform newTarget)
    {
        attackingTarget = newTarget;
        enemyMovement.ChangeTarget(newTarget);
        targetInSight = true;
    }

    public virtual void Die() { }
    public virtual void Hurt() { }
    public virtual void ChangeTarget(Target target) { }
}
