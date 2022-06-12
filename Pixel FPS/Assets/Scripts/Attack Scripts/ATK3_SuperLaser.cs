using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ATK3_SuperLaser : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] AudioSource audioSource;
    [SerializeField] GameObject laserObject;

    [SerializeField] Transform spawnPosition;
    [SerializeField] Transform player;

    bool canShoot = true;



    private void Update()
    {
        if (Controls.controls.Combat.Fire.ReadValue<float>() != 0f && canShoot)
        {
            Fire();
        }
    }


    void Fire()
    {
        canShoot = false;

        audioSource.PlayOneShot(audioSource.clip);

        animator.SetTrigger("Fire");
        Invoke(nameof(Spawn), 0.1f);
        Invoke(nameof(EndReload), 0.5f);
    }

    void Spawn()
    {
        SuperLaser superLaser = Instantiate(laserObject, spawnPosition.position + Vector3.down * 0.6f,
            Quaternion.LookRotation(spawnPosition.forward)).GetComponent<SuperLaser>();
        superLaser.spawner = player;
    }

    void EndReload()
    {
        canShoot = true;
        animator.ResetTrigger("Fire");
    }
}
