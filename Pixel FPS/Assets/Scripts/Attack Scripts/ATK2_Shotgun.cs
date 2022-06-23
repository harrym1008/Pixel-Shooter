using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ATK2_Shotgun : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip[] sfx;

    [SerializeField] int pellets = 8;
    [SerializeField] float fireTime;
    [SerializeField] BulletData pelletData;
    [SerializeField] BulletData slugData;
    [SerializeField] Vector2 pelletSpread;
    [SerializeField] Vector2 slugSpread;


    public bool isSlugs = false;

    bool canShoot = true;

    [SerializeField] Transform spawnPosition;
    [SerializeField] GameObject bulletObject;

    private void Update()
    {
        if (Controls.controls.Combat.Fire.ReadValue<float>() != 0f && canShoot)
        {
            Fire();
        }
    }


    public void Fire()
    {
        StartCoroutine(_Fire());
    }


    IEnumerator _Fire()
    {
        PlayerRecoil.playerRecoil.RecoilFire();

        PlaySound(RNG.Next(0, 1), true);
        canShoot = false;
        animator.SetTrigger("Fire");

        if (isSlugs)
            SpawnSlug();
        else
            SpawnPellets();

        Invoke(nameof(ResetTrigger), fireTime / 2);
        yield return Wait.Seconds(fireTime);

        canShoot = true;
    }

    void ResetTrigger()
    {
        animator.ResetTrigger("Fire");
    }


    void SpawnPellets()
    {
        for (int i = 0; i < pellets; i++)
        {
            Vector3 spread = RNG.RandomSpread(pelletSpread.x, pelletSpread.y);
            if (i == 0) { spread = Vector3.zero; }

            BulletBehaviour bullet = Instantiate(bulletObject, spawnPosition.position, 
                Quaternion.Euler(spawnPosition.eulerAngles + spread)).GetComponent<BulletBehaviour>();

            bullet.bulletData = pelletData;
            bullet.spawnedByPlayer = true;
        }
    }

    void SpawnSlug()
    {
        Vector3 spread = RNG.RandomSpread(slugSpread.x, slugSpread.y);

        BulletBehaviour bullet = Instantiate(bulletObject, spawnPosition.position,
            Quaternion.Euler(spawnPosition.eulerAngles + spread)).GetComponent<BulletBehaviour>();

        bullet.bulletData = slugData;
        bullet.spawnedByPlayer = true;
    }


    private void PlaySound(int soundID, bool playOneShot = false)
    {
        if (playOneShot)
        {
            audioSource.PlayOneShot(sfx[soundID]);
            return;
        }

        audioSource.clip = sfx[soundID];
        audioSource.Play();
    }
}
