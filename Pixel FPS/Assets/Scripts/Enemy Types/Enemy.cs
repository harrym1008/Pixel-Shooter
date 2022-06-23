using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Target myTarget;
    public Momentum momentum;

    [SerializeField] protected Vector2 spawnHealthRange;

    protected Transform attackingTarget = null;
    protected EnemyMovement enemyMovement;
    protected Animator animator;
    public BloodManager.BloodType bloodType;

    [Header("Looking for player parameters")]
    public bool targetInSight = false;
    public bool currentlyInfighting = false;
    [SerializeField] float seeOffsetY;
    [SerializeField] float maxSeeDistance;
    [SerializeField] protected LayerMask seeingLayerMask;


    public virtual void Start()
    {
        myTarget = GetComponent<Target>();
        momentum = GetComponent<Momentum>();
        enemyMovement = GetComponent<EnemyMovement>();
        animator = GetComponent<Animator>();

        myTarget.health = Mathf.RoundToInt(RNG.RangeBetweenVector2(spawnHealthRange));
    }


    public virtual void LookForPlayer(Transform player)
    {
        if (LineOfSightCheck(player.position, maxSeeDistance))
        {
            ChangeTarget(player);
            StateToAttacking(player);
        }
    }


    protected bool LineOfSightCheck(Vector3 endLocation, float maxDistance = 0f, int layerMask = 0)
    {
        if (layerMask == 0)
            layerMask = seeingLayerMask;

        bool line = Physics.Linecast(transform.position + seeOffsetY * transform.up, endLocation, layerMask);
        bool distance = Vector3.Distance(endLocation, transform.position) <= maxDistance;
        if (maxDistance == 0f) { distance = true; }

        return !line && distance;
    }

    protected bool LineOfSightCheck(Vector3 endLocation, out RaycastHit hit, float maxDistance = 0f, int layerMask = 0)
    {
        if (layerMask == 0)
            layerMask = seeingLayerMask;

        bool line = Physics.Linecast(transform.position + seeOffsetY * transform.up, endLocation, out hit, layerMask);
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
        ChangeTarget(newTarget);
    }

    public virtual void Die()
    {
        enemyMovement.agent.enabled = false;
        momentum.dead = true;
    }


    public virtual void Hurt() { }

    public virtual void ChangeTarget(Transform newTarget)
    {
        attackingTarget = newTarget;
        enemyMovement.ChangeTarget(newTarget);
        targetInSight = true;
    }




    
}
