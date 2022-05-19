using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MouseLook : MonoBehaviour
{
    public Vector2 sensitivity;

    public Transform playerBody;

    public float xRot = 0;

    public float mouseX;
    public float mouseY;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }


    void Update()
    {
        Vector2 mouseDelta = Controls.controls.Combat.Aim.ReadValue<Vector2>() * sensitivity * Time.deltaTime;
        mouseX = mouseDelta.x;  mouseY = mouseDelta.y;

        xRot -= mouseY;
        xRot = Mathf.Clamp(xRot, -75f, 80f);

        transform.localRotation = Quaternion.Euler(xRot, 0f, 0f);

        playerBody.Rotate(Vector3.up * mouseX);
        playerBody.eulerAngles = new Vector3(0f, playerBody.eulerAngles.y, 0f);
    }
}
