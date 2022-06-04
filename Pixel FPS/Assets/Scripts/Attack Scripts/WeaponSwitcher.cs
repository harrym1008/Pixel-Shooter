using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSwitcher : MonoBehaviour
{
    public GameObject[] attackParents;

    public int currentWeapon;
    public int maxWeapons;


    private void Start()
    {
        Controls.controls.Combat.NextWeapon.performed += _ 
            => SwitchWeapon(1);
        Controls.controls.Combat.PreviousWeapon.performed += _ 
            => SwitchWeapon(-1);
    }

    void SwitchWeapon(int weapon)
    {
        print($"Switching {weapon}");

        attackParents[currentWeapon].SetActive(false);
        currentWeapon += weapon;

        if (currentWeapon < 0) { currentWeapon = maxWeapons - 1; }
        if (currentWeapon >= maxWeapons) { currentWeapon = 0; }

        attackParents[currentWeapon].SetActive(true);

    }
}
