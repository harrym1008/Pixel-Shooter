using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ImpBehaviour : MonoBehaviour
{
    NavMeshAgent agent;
    Transform target;
    Animator animator;

    [SerializeField] LayerMask environmentLayer;

    // Patrolling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    // Attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked;

    // States
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    private void Start()
    {
        target = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        // Check for sight and attack range
        playerInSightRange = Vector3.Distance(transform.position, target.position) <= sightRange 
            && !Physics.Linecast(transform.position, target.position, environmentLayer);
        playerInAttackRange = Vector3.Distance(transform.position, target.position) <= attackRange
            && !Physics.Linecast(transform.position, target.position, environmentLayer);

        if (!playerInAttackRange && !playerInSightRange)        Patrolling();
        else if (!playerInAttackRange && playerInSightRange)    ChaseTarget();
        else                                                    AttackTarget();

    }

    private void Patrolling()
    {
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
            agent.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        // Walkpoint reached
        if (distanceToWalkPoint.magnitude < 1f)
        {
            walkPointSet = true;
        }
    }

    private void SearchWalkPoint()
    {
        float randomX = RNG.Range(-walkPointRange, walkPointRange);
        float randomZ = RNG.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, environmentLayer))
        {
            walkPointSet = true;
        }
    }

    private void ChaseTarget()
    {
        agent.SetDestination(target.position);
    }

    private void AttackTarget()
    {
        print("Fire!");

        agent.SetDestination(transform.position);

        transform.LookAt(target);

        if (!alreadyAttacked)
        {
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }



}