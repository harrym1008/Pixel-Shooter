using System.Collections.Generic;
using UnityEngine;

public class ColliderContainer : MonoBehaviour
{
    private List<Collider> colliders = new List<Collider>();
    public Collider[] GetColliders() { return colliders.ToArray(); }

    private void OnTriggerEnter(Collider other)
    {
        if (!colliders.Contains(other)) { colliders.Add(other); }
    }

    private void OnTriggerExit(Collider other)
    {
        colliders.Remove(other);
    }
}
