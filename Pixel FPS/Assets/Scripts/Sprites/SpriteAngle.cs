using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteAngle : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite[] sprites;
    [SerializeField] private bool[] flipSprites;

    private Vector3 targetPos;
    private Vector3 targetDir;

    public float angle;
    public int lastIndex;

    private void Update()
    {
        // Get Target position and direction
        targetPos = new Vector3(target.position.x, transform.position.y, target.position.z);
        targetDir = targetPos - transform.position;

        // Get angle
        angle = Vector3.SignedAngle(targetDir, transform.forward, Vector3.up);

        lastIndex = GetIndex(angle);

        spriteRenderer.sprite = sprites[lastIndex];
        spriteRenderer.flipX = !flipSprites[lastIndex];
    }

    private int GetIndex(float angle)
    {
        // Front
        if (angle >= -22.5f && angle < 22.5f)
            return 0;
        else if (angle >= 22.5f && angle < 67.5f)
            return 7;
        else if (angle >= 67.5f && angle < 112.5f)
            return 6;
        else if (angle >= 112.5f && angle < 157.5f)
            return 5;
        // Back
        else if (angle <= -157.5f || angle >= 157.5f)
            return 4;
        else if (angle >= -157.5f && angle < -112.5f)
            return 3;
        else if (angle >= -112.5f && angle < -67.5f)
            return 2;
        else if (angle >= -67.5f && angle < -22.5f)
            return 1;

        return lastIndex;
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.forward);

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, targetPos);

    }
}