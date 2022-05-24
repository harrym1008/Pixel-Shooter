using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ATK1_Fists : MonoBehaviour
{
    bool[] combatTypes = new bool[7] { true, true, true, true, true, true, true };

    [SerializeField] Animator animator;
    [SerializeField] string[] strings;

    [SerializeField] float attackTime = 1f;
    [SerializeField] float speed = 1f;
    [SerializeField] float reach = 1.5f;
    [SerializeField] GameObject idleHand;
    [SerializeField] Transform player;
    [SerializeField] AudioClip[] sfx;

    AudioSource audioSource;
    bool canThrowHands = true;

    private void Start()
    {
        animator.SetFloat("speed", speed);
        CheckToShowIdleHand();

        audioSource = GetComponent<AudioSource>();
    }


    private void Update()
    {      
        if (Controls.controls.Combat.Fire.ReadValue<float>() != 0f && canThrowHands)
        {
            StartCoroutine(ThrowHands());
        }
    }

    private IEnumerator ThrowHands()
    {
        audioSource.PlayOneShot(sfx[RNG.Next(0, 2)]);

        canThrowHands = false;
        CheckToShowIdleHand();

        string trigger = strings[GetNextCombatType()];
        animator.SetTrigger(trigger);

        Invoke(nameof(AttackDMG), attackTime * 0.6f / speed);

        yield return Wait.Seconds(attackTime / speed);

        animator.ResetTrigger(trigger);
        canThrowHands = true;

        Invoke(nameof(CheckToShowIdleHand), 0.01f);

    }

    private void CheckToShowIdleHand()
    {
        idleHand.SetActive(canThrowHands);
    }


    private int GetNextCombatType()
    {
        int pick, counter = 0;

        string s = "";
        foreach (var item in combatTypes)
        {
            s += item.ToString();
        }
        print(s);

        do
        {
            pick = RNG.Next(0, combatTypes.Length-1);
            counter++;            
        }
        while (!combatTypes[pick] && counter < 100 );

        UpdateCombatTypes(pick);
        return pick;
    }

    private void UpdateCombatTypes(int chosen)
    {
        for (int i = 0; i < combatTypes.Length; i++)
        {
            combatTypes[i] = i != chosen;

            if (chosen == i-1 && i != 0)
            {
                combatTypes[i] = false;
            }
        }
    }

    private GameObject FindEnemyToHit()
    {
        int[] angles = new int[] { 0, -10, 10, -20, 20 };

        for (int angle = 0; angle < angles.Length; angle++)
        {
            if (Physics.Raycast(player.position + PlayerMovement.attackPosition, player.eulerAngles 
                + new Vector3(angle, 0f, 0f), out RaycastHit hit, reach, LayerMask.NameToLayer("Enemies")))
            {
                return hit.collider.gameObject;
            }
        }

        return null;
    }


    private void AttackDMG()
    {
        Debug.DrawRay(player.position + PlayerMovement.attackPosition, player.eulerAngles);

        if (Physics.Raycast(player.position + PlayerMovement.attackPosition, player.eulerAngles, 
            out RaycastHit hit, reach, LayerMask.NameToLayer("Enemies")))
        {
            audioSource.PlayOneShot(sfx[3]);
            print($"I hit {hit.collider.gameObject}");
        }
    }

}
