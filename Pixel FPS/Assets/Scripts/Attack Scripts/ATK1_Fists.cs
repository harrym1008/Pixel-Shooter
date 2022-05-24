using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ATK1_Fists : MonoBehaviour
{
    bool[] combatTypes = new bool[5] { true, true, true, true, true };

    [SerializeField] Animator animator;
    [SerializeField] string[] strings;

    [SerializeField] float attackTime = 1f;
    [SerializeField] float speed = 1f;
    [SerializeField] GameObject idleHand;

    bool canThrowHands = true;

    private void Start()
    {
        animator.SetFloat("speed", speed);
        CheckToShowIdleHand();        
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
        canThrowHands = false;
        CheckToShowIdleHand();

        string trigger = strings[GetNextCombatType()];
        animator.SetTrigger(trigger);

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
            pick = RNG.Next(0, 4);
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
        }
    }
}
