using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MouseLook : MonoBehaviour
{
    public InputMaster controls;

    public Vector2 sensitivity;

    public Transform playerBody;
    public Transform gunCamera;

    public float xRot = 0;

    public float mouseX;
    public float mouseY;

    void Awake()
    {
        controls = new InputMaster();
    }

    void Update()
    {
        Vector2 mouseDelta = controls.Combat.Aim.ReadValue<Vector2>() * sensitivity;

        mouseX = mouseDelta.x * Time.deltaTime / (Time.timeScale > 0 ? Time.timeScale : Mathf.Infinity); 
        mouseY = mouseDelta.y * Time.deltaTime / (Time.timeScale > 0 ? Time.timeScale : Mathf.Infinity);

        xRot -= mouseY;
        xRot = Mathf.Clamp(xRot, -75f, 80f);

        transform.localRotation = Quaternion.Euler(xRot, 0f, 0f);
        gunCamera.localRotation = transform.localRotation;

        playerBody.Rotate(Vector3.up * mouseX);
        playerBody.eulerAngles = new Vector3(0f, playerBody.eulerAngles.y, 0f);
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
