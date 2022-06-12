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

    public Vector2Int closeDamageRange;
    public Vector2Int damageRange;


    private void Start()
    {
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Debug.DrawRay(transform.position, forward * maxLength, Color.cyan, 5f);

        bool ray = Physics.Raycast(transform.position, forward * maxLength, out RaycastHit hit, maxLength, environment);
        length = ray ? hit.distance : maxLength;
        length -= 0.5f;

        mainPS = GetComponent<ParticleSystem>();
        shape = mainPS.shape;
        shape.position = new Vector3(0f, 0f, length / 2f);
        shape.scale = new Vector3(0.1f, 0.1f, length);

        hitObjectTransform = transform.Find("Hit Object");

        if (ray)
        {
            hitObjectTransform.transform.position = hit.point;
            hitObjectTransform.transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(hit.normal), 0.5f);
        }
        else
        {
            Destroy(hitObjectTransform.gameObject);
        }

        Invoke(nameof(DealDamage), 0.05f);
        Destroy(gameObject, 1.5f);
    }


    /*private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawCube(transform.position - shape.position, shape.scale);
    }*/



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
        //Debug.DrawRay(transform.position, Quaternion.LookRotation(transform.right).eulerAngles, Color.magenta, 0.5f);

        Collider[] targetColliders = Physics.OverlapBox(transform.position - shape.position, shape.scale / 2, Quaternion.LookRotation(transform.forward), targets);

        foreach (Target target in GetTargets(targetColliders))
        {
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
