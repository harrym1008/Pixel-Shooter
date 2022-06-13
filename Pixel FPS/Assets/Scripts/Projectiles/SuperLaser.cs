using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperLaser : MonoBehaviour
{
    float length;
    [SerializeField] float maxLength;
    ParticleSystem mainPS;
    Transform hitObjectTransform;

    ParticleSystem.ShapeModule shape;

    public LayerMask environment;
    public LayerMask targets;
    public Transform spawner;

    public BoxCollider boxTrigger;
    ColliderContainer container;

    public Vector2Int closeDamageRange;
    public Vector2Int damageRange;


    private void Start()
    {
        container = boxTrigger.GetComponent<ColliderContainer>();

        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Debug.DrawRay(transform.position, forward * maxLength, Color.cyan, 5f);

        bool ray = Physics.Raycast(transform.position, forward * maxLength, out RaycastHit hit, maxLength, environment);
        length = ray ? hit.distance : maxLength;
        length -= 0.5f;

        mainPS = GetComponent<ParticleSystem>();
        shape = mainPS.shape;
        shape.position = new Vector3(0f, 0f, length / 2f);
        shape.scale = new Vector3(0.1f, 0.1f, length);

        mainPS.Play();

        boxTrigger.center = shape.position;
        boxTrigger.size = new Vector3(0.5f, 0.5f, shape.scale.z);

        hitObjectTransform = transform.Find("Hit Object");

        if (ray)
        {
            hitObjectTransform.transform.position = hit.point;
            hitObjectTransform.transform.rotation = Quaternion.LookRotation(hit.normal);  // Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(hit.normal), 0.5f);
        }
        else
        {
            Destroy(hitObjectTransform.gameObject);
        }

        Invoke(nameof(DealDamage), 0.05f);
        Destroy(gameObject, 1.5f);
    }






    public Target[] GetTargets(Collider[] colliders)
    {
        List<Target> targets = new List<Target>();

        foreach (var collider in colliders)
        {
            if (collider.transform == spawner)
            {
                continue;
            }

            targets.Add(collider.GetComponent<Target>());
        }

        return targets.ToArray();
    }


    public void DealDamage()
    {
        Collider[] targetColliders = container.GetColliders();

        foreach (Target target in GetTargets(targetColliders))
        {
            if (target == null) { continue; }
            if (targets != (targets | (1 << target.gameObject.layer))) { continue; }

            int DMG = Vector3.Distance(target.transform.position, transform.position) <= 5f
                ? Mathf.RoundToInt(RNG.RangeBetweenVector2(closeDamageRange))
                : Mathf.RoundToInt(RNG.RangeBetweenVector2(damageRange));

            target.InflictDMG(DMG);

            if (!target.isPlayer)
            {
                target.enemy.momentum.AddImpact(transform.forward + RNG.RandomVector3(y: false) * 0.5f, DMG);
            }
        }

    }
}
