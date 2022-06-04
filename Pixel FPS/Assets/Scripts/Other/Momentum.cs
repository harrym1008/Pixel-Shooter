using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Momentum : MonoBehaviour
{
    public bool characterControllerBased;

    public float mass = 3.0f;
    public float damping = 5f;

    Vector3 impact = Vector3.zero;

    private CharacterController character;

    void Start()
    {
        character = GetComponent<CharacterController>();
    }


    void Update()
    {
        if (characterControllerBased)
        {
            if (impact.magnitude > 0.2f) character.Move(impact * Time.deltaTime);
        }
        else
        {
            if (impact.magnitude > 0.2f) transform.Translate(impact * Time.deltaTime, Space.World);
        }


        impact = Vector3.Lerp(impact, Vector3.zero, damping * Time.deltaTime);

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
