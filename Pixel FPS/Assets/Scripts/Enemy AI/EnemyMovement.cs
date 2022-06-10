using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEditor;

public class EnemyMovement : MonoBehaviour
{
    Enemy myEnemy;

    bool movement = true;
    bool wandering = false;

    public bool targetInSight;
    float searchTime;

    NavMeshAgent agent;
    Target myTarget;

    Vector3 targetLocation;
    Transform targetTransform;

    Vector3 lastSeenTargetLocation;
    Vector3 spawnPosition;
    Vector3 stationedPosition;

    [Header("Main Parameters")]
    [SerializeField] Vector3 sightLocation;
    [SerializeField] Vector2 maxSearchTime;
    [SerializeField] LayerMask environment;

    [Header("Wandering Parameters")]
    [SerializeField] float wanderRange;
    [SerializeField] Vector2 wanderWaitTime;

    private void Start()
    {
        myEnemy = GetComponent<Enemy>();
        stationedPosition = transform.position;
        spawnPosition = stationedPosition;

        agent = GetComponent<NavMeshAgent>();
        myTarget = GetComponent<Target>();

        ChangeTarget(GameObject.Find("Player").transform);

        SetMovement(true);
    }


    private void Update()
    {
        if (myTarget.isDead || !movement)
            return;


        if (targetTransform != null)
            targetLocation = targetTransform.position;


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
            StopCoroutine(Wandering());
            agent.isStopped = true;
        }
        else
        {
            agent.isStopped = false;
            movement = true;
            StartCoroutine(Wandering());
        }
    }

    

    IEnumerator Wandering()
    {
        while (true)
        {
            while (wandering)
            {
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
        int failsafeCounter = 0;

        while (failsafeCounter < 100)
        {
            failsafeCounter++;
            float randomX = RNG.Range(-wanderRange, wanderRange);
            float randomZ = RNG.Range(-wanderRange, wanderRange);

            Vector3 walkPoint = new Vector3(stationedPosition.x + randomX, stationedPosition.y, stationedPosition.z + randomZ);

            //print(walkPoint);
            if (Physics.Raycast(walkPoint, -transform.up, out RaycastHit hit, 2f, environment)
                /*|| !Physics.Linecast(walkPoint, transform.position, environment)*/)
            {
                return hit.point;
            }
        }
        return spawnPosition;

    }



    private void OnDrawGizmos()
    {
        if (Camera.current != Camera.main && Camera.current != SceneView.lastActiveSceneView.camera) return;


        if (targetInSight)
            Gizmos.color = Color.green;
        else
            Gizmos.color = Color.red;

        Gizmos.DrawLine(transform.position + sightLocation, targetLocation);


        Gizmos.color = Color.blue;

        try
        {
            for (int i = 0; i < agent.path.corners.Length - 1; i++)
            {
                Gizmos.DrawLine(agent.path.corners[i], agent.path.corners[i + 1]);
            }
        } catch { }
    }    
}