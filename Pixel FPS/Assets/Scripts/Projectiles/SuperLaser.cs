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

        Collider[] targetColliders = Physics.OverlapBox(transform.position - shape.position, shape.scale / 2, Quaternion.identity, targets);

        foreach (Target target in GetTargets(targetColliders))
        {
            target.InflictDMG(120);
            target.GetComponent<Momentum>().AddImpact(-transform.forward + Vector3.up * RNG.RangePosNeg(20), 120f);
        }
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
}
