using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ATK1_Fists : MonoBehaviour
{
    bool[] combatTypes = new bool[7] { true, true, true, true, true, true, true };

    [SerializeField] Animator animator;
    [SerializeField] string[] strings;
    [SerializeField] Vector2 damageRange;

    [SerializeField] float attackTime = 1f;
    [SerializeField] float speed = 1f;
    [SerializeField] float reach = 1.5f;
    [SerializeField] GameObject idleHand;
    [SerializeField] AudioClip[] sfx;

    [SerializeField] LayerMask[] layerMasks;

    AudioSource audioSource;
    bool canThrowHands = true;
    Transform fireAngle;

    private void Start()
    {
        animator.SetFloat("speed", speed);
        CheckToShowIdleHand();

        audioSource = GetComponent<AudioSource>();

        fireAngle = Camera.main.transform;
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
        PlaySound(RNG.Next(0, 2));

        canThrowHands = false;
        CheckToShowIdleHand();

        string trigger = strings[GetNextCombatType()];
        animator.SetTrigger(trigger);

        Invoke(nameof(AttackDMG), attackTime * 0.3f / speed);

        yield return Wait.Seconds(attackTime / speed);

        animator.ResetTrigger(trigger);
        canThrowHands = true;

        Invoke(nameof(CheckToShowIdleHand), 0.03f);

    }

    private void CheckToShowIdleHand()
    {
        idleHand.SetActive(canThrowHands);
    }


    private int GetNextCombatType()
    {
        int pick, counter = 0;

        /*string s = "";
        foreach (var item in combatTypes)
        {
            s += item.ToString();
        }*/

        do
        {
            pick = RNG.Next(0, combatTypes.Length-1);
            counter++;            
        }
        while (!combatTypes[pick] && counter < 100 );

        //print(s + $" --> {strings[pick]}");

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
            if (Physics.Raycast(fireAngle.position, fireAngle.eulerAngles 
                + new Vector3(angle, 0f, 0f), out RaycastHit hit, reach, layerMasks[0]))
            {
                return hit.collider.gameObject;
            }
        }

        return null;
    }


    private void AttackDMG()
    {
        Debug.DrawRay(fireAngle.position, fireAngle.forward * 180f, Color.white, 0.5f);

        if (Physics.Raycast(fireAngle.position, fireAngle.forward * 180f, 
            out RaycastHit hit, reach, layerMasks[0]))
        {
            if (hit.collider.TryGetComponent(out Target target))
            {
                target.InflictDMG( Mathf.RoundToInt(RNG.RangeBetweenVector2(damageRange)) );
                print($"I hit {hit.collider.gameObject}");
            }
            PlaySound(3);

        }
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
