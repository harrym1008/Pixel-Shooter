using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHeightLerp : MonoBehaviour
{
    public Transform myParent;

    public float lerpSpeed = 10f;
    public float yBase;
    public Vector3 lastPosition;

    private void LateUpdate()
    {
        Vector3 moveToPosition = transform.position;

        transform.position = new Vector3(transform.position.x,
            Mathf.Lerp(lastPosition.y, moveToPosition.y, lerpSpeed * Time.deltaTime),
            transform.position.z);

        lastPosition = transform.position;
    }
}
