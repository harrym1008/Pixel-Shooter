using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimator : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] SpriteAngle spriteAngle;

    private void Update()
    {
        animator.SetFloat("SprRot", spriteAngle.lastIndex);    
    }
}