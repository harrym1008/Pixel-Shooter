using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spin : MonoBehaviour
{
    public Vector3 spinPerSecond;

    private void Update()
    {
        transform.eulerAngles += spinPerSecond * Time.deltaTime;
    }
}
