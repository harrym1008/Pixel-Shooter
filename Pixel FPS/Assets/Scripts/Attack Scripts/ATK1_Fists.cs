using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ATK1_Fists : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] string[] strings;

    [SerializeField] float speed = 1f;

    private IEnumerator Start()
    {
        while (true)
        {
            animator.SetFloat("speed", speed);

            string trigger = RNG.RandomObject(strings);
            print(trigger);

            animator.SetTrigger(trigger);
            yield return Wait.Seconds(1 / 3f / speed);
            animator.ResetTrigger(trigger);
        }
    }
}
