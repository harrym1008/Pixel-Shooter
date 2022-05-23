using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    bool movement = true;
    bool wandering = false;

    bool targetInSight;
    float searchTime;

    NavMeshAgent agent;

    Vector3 targetLocation;
    Transform targetTransform;

    Vector3 lastSeenTargetLocation;
    Vector3 spawnPosition;
    Vector3 stationedPosition;

    [Header("Main Parameters")]
    [SerializeField] Vector3 sightLocation;
    [SerializeField] float maxSeeDistance;
    [SerializeField] Vector2 maxSearchTime;
    [SerializeField] LayerMask environment;

    [Header("Wandering Parameters")]
    [SerializeField] float wanderRange;
    [SerializeField] Vector2 wanderWaitTime;

    private void Start()
    {
        stationedPosition = transform.position;
        spawnPosition = stationedPosition;

        agent = GetComponent<NavMeshAgent>();
        ChangeTarget(GameObject.Find("Player").transform);

        SetMovement(true);
    }


    private void Update()
    {
        if (targetTransform != null)
            targetLocation = targetTransform.position;

        if (!movement)
            return;

        targetInSight = !Physics.Linecast(transform.position + sightLocation, targetLocation, environment)
            && Vector3.Distance(transform.position + sightLocation, targetLocation) <= maxSeeDistance;

        if (targetInSight)
        {
            lastSeenTargetLocation = targetLocation;
            searchTime = RNG.RangeBetweenVector2(maxSearchTime);
        }
        else
        {
            searchTime -= Time.deltaTime;
        }


        if (targetInSight)
        {
            wandering = false;
            agent.SetDestination(targetLocation);
        }
        else if (searchTime > 0f)
        {
            wandering = false;
            agent.SetDestination(lastSeenTargetLocation);
        }
        else
        {
            wandering = true;
        }

    }



    public void ChangeTarget(Transform target)
    {
        targetTransform = target;
    }

    public void ChangeTarget(Vector3 target)
    {
        targetTransform = null;
        targetLocation = target;
    }

    public void SetMovement(bool enabled)
    {
        if (!enabled)
        {
            movement = false;
            agent.isStopped = true;
            StopCoroutine(Wandering());
        }
        else
        {
            movement = true;
            agent.isStopped = false;
            StartCoroutine(Wandering());
        }
    }


    public void OnDrawGizmos()
    {      
        if (targetInSight)
            Gizmos.color = Color.green;
        else
            Gizmos.color = Color.red;

        Gizmos.DrawLine(transform.position + sightLocation, targetLocation);
    }


    IEnumerator Wandering()
    {
        while (true)
        {
            while (wandering)
            {
                print("Refresh");
                stationedPosition = SearchWalkPoint();
                agent.SetDestination(stationedPosition);

                float until = RNG.RangeBetweenVector2(wanderWaitTime);
                while (wandering && until > 0)
                {
                    until -= Time.deltaTime;
                    yield return null;
                }
            }

            yield return new WaitUntil(() => wandering);
        }
    }

    private Vector3 SearchWalkPoint()
    {
        print("Searching");
        int failsafeCounter = 0;

        while (failsafeCounter < 100)
        {
            failsafeCounter++;
            float randomX = RNG.Range(-wanderRange, wanderRange);
            float randomZ = RNG.Range(-wanderRange, wanderRange);

            Vector3 walkPoint = new Vector3(stationedPosition.x + randomX, stationedPosition.y, stationedPosition.z + randomZ);

            print(walkPoint);
            if (Physics.Raycast(walkPoint, -transform.up, out RaycastHit hit, 2f, environment)
                /*|| !Physics.Linecast(walkPoint, transform.position, environment)*/)
            {
                Debug.Log($"{gameObject.name}: Found a new walking point {hit.point}");
                return hit.point;
            }
        }
        Debug.LogError($"{gameObject.name}: Could not find a new wander location after 100 attempts\nReturning enemy spawn location");
        return spawnPosition;

    }
}