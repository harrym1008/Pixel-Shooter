using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    public float effectiveRange;
    public float maximumRange;
    public float velocity;
    public Vector2 upperDamageRange;
    public Vector2 lowerDamageRange;
    public LayerMask layerMask;

    Vector3 spawnLocation;
    float totalDistanceTravelled;
    float upperDamage, lowerDamage;

    private void Start()
    {
        spawnLocation = transform.position;
        upperDamage = RNG.RangeBetweenVector2(upperDamageRange);
        lowerDamage = RNG.RangeBetweenVector2(lowerDamageRange);
    }


    private void Update()
    {
        float distanceThisFrame = velocity * Time.deltaTime;
        totalDistanceTravelled += distanceThisFrame;

        Obstruction obstruction = CheckForObstruction(distanceThisFrame);
        transform.position = obstruction.advanceTo;

        if (obstruction.hitSomething)
        {
            print(GetDamage(obstruction.advanceTo));

            if (obstruction.collider.TryGetComponent(out Health targetHealth))
            {
            }

            Destroy(gameObject);
        }

        if (totalDistanceTravelled > maximumRange)
        {
            print("Over max range, killing this bullet");
            Destroy(gameObject);
        }
    }



    private float GetDamage(Vector3 endLocation)
    {
        float distance = Vector3.Distance(spawnLocation, endLocation) - effectiveRange;

        if (distance < 0) { return upperDamage; }
        float lerpValue = distance / (maximumRange - effectiveRange);

        print(lerpValue);

        return Mathf.Lerp(upperDamage, lowerDamage, lerpValue);
    }



    private Obstruction CheckForObstruction(float distanceToAdvance)
    {
        Vector3 endingPosition = transform.position + transform.forward * distanceToAdvance;
        bool didIHit = Physics.Linecast(transform.position, endingPosition, out RaycastHit hit, layerMask);

        if (didIHit)
        {
            return new Obstruction
            {
                hitSomething = true,
                raycastHit = hit,
                collider = hit.collider,
                advanceTo = hit.point
            };
        }
        else
        {
            return new Obstruction
            {
                hitSomething = false,
                raycastHit = hit,
                collider = null,
                advanceTo = endingPosition
            };
        }
    }
}


public class Obstruction
{
    public bool hitSomething;
    public RaycastHit raycastHit;
    public Collider collider = null;
    public Vector3 advanceTo = new Vector3();
}