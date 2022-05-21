using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpBehaviour : MonoBehaviour
{
    public Transform target;


    IEnumerator Start()
    {
        yield return Wait.Seconds(RNG.Range(2f, 3f));

        while (true)
        {
            yield return null;
        }
    }
}