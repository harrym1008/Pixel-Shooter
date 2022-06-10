using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Target : MonoBehaviour
{
    public int health;
    public int armour;
    public float protectionLevel;

    public bool isDead = false;
    public Enemy enemy = null;

    private void Start()
    {
        enemy = GetComponent<Enemy>();
    }


    public void InflictDMG(int DMG)
    {
        int healthDamage = Mathf.RoundToInt(DMG * (1 - protectionLevel));
        int armourDamage = Mathf.RoundToInt(DMG * protectionLevel);

        print($"{gameObject.name} took {DMG} damage");

        armour -= armourDamage;
        if (armour < 0)
        {
            healthDamage += Mathf.Abs(armour);
            armour = 0;
        }

        health -= healthDamage;

        if (health <= 0)
        {
            health = 0;
            Die();
        }
        else if (enemy != null)
        {
            enemy.Hurt();
        }

    }


    public void Die()
    {
        isDead = true;

        if (enemy != null)
        {
            enemy.Die();
        }
    }
}
