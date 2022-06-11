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

    public BulletData bulletData = null;
    public GameObject bulletHole;

    private bool killing = false;

    private void Start()
    {
        UpdateData();

        spawnLocation = transform.position;
        upperDamage = RNG.RangeBetweenVector2(upperDamageRange);
        lowerDamage = RNG.RangeBetweenVector2(lowerDamageRange);
    }

    void UpdateData()
    {
        if (killing) { return; } 

        if (bulletData != null)
        {
            if (bulletData.isNull) { return; }

            effectiveRange = bulletData.effectiveRange;
            maximumRange = bulletData.maximumRange;
            velocity = bulletData.velocity;
            upperDamageRange = bulletData.upperDamageRange;
            lowerDamageRange = bulletData.lowerDamageRange;
        }
    }


    private void Update()
    {
        float distanceThisFrame = velocity * Time.deltaTime;
        totalDistanceTravelled += distanceThisFrame;

        Obstruction obstruction = CheckForObstruction(distanceThisFrame);
        transform.position = obstruction.advanceTo;

        if (obstruction.hitSomething)
        {
            if (obstruction.collider.TryGetComponent(out Target target))
            {
                int damage = Mathf.RoundToInt(GetDamage(obstruction.advanceTo));
                target.InflictDMG(damage);

                if (target.isDead)
                {
                    Manager.blood.CreateBigBlood(obstruction.raycastHit.point,
                        Quaternion.LookRotation(obstruction.raycastHit.normal), target.enemy.bloodType);

                    target.enemy.momentum.AddImpact(obstruction.raycastHit.normal, velocity * -0.05f);
                }
                else
                {
                    Manager.blood.CreateSmallBlood(obstruction.raycastHit.point,
                        Quaternion.LookRotation(obstruction.raycastHit.normal), target.enemy.bloodType);

                    target.enemy.momentum.AddImpact(obstruction.raycastHit.normal, velocity * -0.03f);
                }
            }
            else
            {
                Destroy(Instantiate(bulletHole, obstruction.raycastHit.point,
                    Quaternion.LookRotation(obstruction.raycastHit.normal)), 120f);
                
            }

            killing = true;
            Invoke(nameof(DestroyMe), Time.deltaTime * 2);
        }

        if (totalDistanceTravelled > maximumRange)
        {
            Destroy(gameObject);
        }
    }

    private void DestroyMe()
    {
        Destroy(gameObject);
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

[System.Serializable]
public class BulletData
{
    public float effectiveRange;
    public float maximumRange;
    public float velocity;
    public Vector2 upperDamageRange;
    public Vector2 lowerDamageRange;

    public bool isNull = false;
}