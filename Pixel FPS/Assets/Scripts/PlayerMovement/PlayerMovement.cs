using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //public Vector3 

    private void Update()
    {
        print(Controls.controls.Player.Movement.ReadValue<Vector2>());
    }
}
