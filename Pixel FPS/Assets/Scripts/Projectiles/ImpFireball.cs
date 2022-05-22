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
    [SerializeField] bool active = true;
    [SerializeField] float speed;
    [SerializeField] float endAnimTime;

    [Header("Sound and Particle Systems")]
    [SerializeField] AudioSource audioSource;
    [SerializeField] ParticleSystem[] PS;
    [SerializeField] AudioClip[] sfx;





    private void Update()
    {
        if (active)
        {
            transform.position += transform.forward * Time.deltaTime * speed;
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        int layer = collision.gameObject.layer;

        // Collided with player
        if (player == (player | (1 << layer)))
        {
            print("Hit the player");
        }
        // Collided with another enemy
        else if (enemies == (enemies | (1 << layer)))
        {
            print("Hit another enemy");
        }
        // Collided with the environment
        else if (environment == (environment | (1 << layer)))
        {
            print("Hit the environment");
        }
        // Hit something, but not of relevance
        else
            return;

        active = false;
        Invoke(nameof(KillMe), endAnimTime);

        PS[0].Stop();
        PS[1].Stop();
        PS[2].Play();
    }

    void KillMe()
    {
        Destroy(gameObject);
    }
}
