using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Target : MonoBehaviour
{
    public int health;
    public int armour;
    public float protectionLevel;

    public bool isDead = false;
    public Enemy enemy = null;
    public bool isPlayer;
       
    // testing
    [SerializeField] TextMeshProUGUI text;


    private void Start()
    {
        isPlayer = !TryGetComponent(out enemy);
    }



    private void Update()
    {
        if (isPlayer)
        {
            text.text = $"HEALTH: {health.ToString() }\nARMOUR: { armour.ToString() }"; 
        }
    }


    public void InflictDMG(int DMG)
    {
        if (isDead)
        {
            return;
        }


        int healthDamage = Mathf.RoundToInt(DMG * (1 - protectionLevel));
        int armourDamage = Mathf.RoundToInt(DMG * protectionLevel);

        //print($"{gameObject.name} took {DMG} damage");

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
        else if (isPlayer)
        {
            StartCoroutine(PlayerDie());
        }
    }


    public BloodManager.BloodType GetBloodType() {
        return isPlayer ? BloodManager.BloodType.Crimson : enemy.bloodType; }
    

    public Momentum GetMomentum() {
        return isPlayer ? GetComponent<Momentum>() : enemy.momentum; }



    IEnumerator PlayerDie()
    {
        float until = 2f;

        Controls.controls.Disable();

        Transform attackCamera = GameObject.Find("Attack Camera").transform;

        do
        {
            PlayerRecoil.playerRecoil.positionalOffset = new Vector3(0f,
                Mathf.MoveTowards(PlayerRecoil.playerRecoil.positionalOffset.y, -1.3f, Time.deltaTime * 1.5f), 0f);

            PlayerRecoil.playerRecoil.rotationalOffset = new Vector3(0f, 0f,
                Mathf.MoveTowards(PlayerRecoil.playerRecoil.positionalOffset.z, -20f, Time.deltaTime * 30f));

            attackCamera.localPosition = new Vector3(0f,
                Mathf.MoveTowards(attackCamera.localPosition.y, 0.25f, Time.deltaTime * 0.5f), 0f);


            until -= Time.deltaTime;
            yield return Wait.Frame;

        } while (until > 0f);

        attackCamera.gameObject.SetActive(false);
    }
}
