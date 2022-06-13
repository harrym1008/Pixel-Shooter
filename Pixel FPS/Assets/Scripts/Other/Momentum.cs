using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Momentum : MonoBehaviour
{
    public bool characterControllerBased;

    [Header("Mass used in code is 8x smaller.")]
    public Vector2 massRange;
    public float mass = 0f;
    [Header("Other")]
    [SerializeField] float damping = 5f;
    float agentHeight;

    Vector3 impact = Vector3.zero;

    private CharacterController character;
    [SerializeField] bool agent;
    public bool dead;

    void Start()
    {
        character = GetComponent<CharacterController>();

        if (mass == 0f)
            mass = RNG.RangeBetweenVector2(massRange);

        mass /= 8;

        if (TryGetComponent(out NavMeshAgent nmagent))
        {
            agentHeight = nmagent.height / 2;
        }
    }


    void LateUpdate()
    {
        if (characterControllerBased)
        {
            if (impact.magnitude > 0.2f) character.Move(impact * Time.deltaTime);
        }
        else
        {
            if (impact.magnitude > 0.2f) transform.position += impact * Time.deltaTime;
        }

        impact = Vector3.Lerp(impact, Vector3.zero, damping * Time.deltaTime);



        if (impact.magnitude > 0.2f && agent && dead)
        {
            if (NavMesh.SamplePosition(transform.position, out NavMeshHit hit, 256f, NavMesh.AllAreas))
                transform.position = hit.position + Vector3.up * agentHeight;
        }

    }

    public void AddImpact(Vector3 dir, float force, Vector2 varience)
    {
        dir.Normalize();
        if (dir.y < 0) dir.y = -dir.y;
        impact += RNG.RangeBetweenVector2(varience) * dir.normalized * force / mass;
    }

    public void AddImpact(Vector3 dir, float force)
    {
        dir.Normalize();
        if (dir.y < 0) dir.y = -dir.y;
        impact += dir.normalized * force / mass;
    }
}
