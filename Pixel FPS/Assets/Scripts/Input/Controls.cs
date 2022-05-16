using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controls : MonoBehaviour
{
    public static InputMaster controls = null;


    private void Awake()
    {
        if (controls != null)
        {
            Destroy(gameObject);
        }

        controls = new InputMaster();
        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

}
