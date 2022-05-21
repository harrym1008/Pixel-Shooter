using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    Transform cam;
    public bool canLookVertically;
    public Vector3 offset;


    private void Start()
    {
        cam = Camera.main.transform;
    }

    private void LateUpdate()
    {
        transform.eulerAngles = cam.eulerAngles + offset;

        if (!canLookVertically)
        {
            transform.eulerAngles = new Vector3(0f, transform.eulerAngles.y, transform.eulerAngles.z);
        }
        /*else
        {
            transform.eulerAngles = new Vector3(-transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z);
        }*/
    }
}