using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpFireball : MonoBehaviour
{
    [Header("Layer Masks")]
    [SerializeField] LayerMask player;
    [SerializeField] LayerMask enemies;
    [SerializeField] LayerMask environment;

    [Header("Fireball Parameters")]
    [SerializeField] float lifetime;
    [SerializeField] bool active = true;
    [SerializeField] float speed;
    [SerializeField] float endAnimTime;
    [SerializeField] Vector2 damageRange;

    [Header("Sound and Particle Systems")]
    [SerializeField] AudioSource audioSource;
    [SerializeField] ParticleSystem[] PS;
    [SerializeField] AudioClip[] sfx;

    public Transform spawner;

    private void Start()
    {
        /*Collider[] colliders = Physics.OverlapSphere(transform.position, radius);

        foreach (Collider collider in colliders)
        {
            Physics.IgnoreCollision(GetComponent<Collider>(), collider);
        }*/
    }


    private void Update()
    {
        if (active)
        {
            lifetime -= Time.deltaTime;
            transform.position += transform.forward * Time.deltaTime * speed;

            if (lifetime <= 0f)
            {
                Die();
            }
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        int layer = other.gameObject.layer;
        bool causeDamage = false;

        // Collided with player
        if (player == (player | (1 << layer)))
        {
            causeDamage = true;
        }
        // Collided with another enemy
        else if (enemies == (enemies | (1 << layer)))
        {
            if (other.transform == spawner)   // So the fireball doesn't hit the imp it came from
                return;
            causeDamage = true;
        }
        // Collided with the environment
        else if (environment == (environment | (1 << layer)))
        {
            causeDamage = false;
        }
        // Hit something, but not of relevance
        else
            return;

        if (causeDamage)
        {
            other.GetComponent<Target>().InflictDMG(Mathf.RoundToInt(RNG.RangeBetweenVector2(damageRange)));
        }

        Die();
        
    }


    void Die()
    { 
        active = false;

        PS[0].Stop();
        PS[1].Stop();
        PS[2].Play();

        Destroy(GetComponent<Collider>());
        Destroy(gameObject, endAnimTime);
    }
}
