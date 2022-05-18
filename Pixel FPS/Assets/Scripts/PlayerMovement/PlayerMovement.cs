using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PlayerMovement : MonoBehaviour
{
    public InputMaster controls;
    public CharacterController controller;

    public float runSpeed;
    public float sprintSpeed;
    public float gravity = -9.81f;
    public float jumpHeight;

    Vector3 groundCheck = Vector3.zero;
    float groundDistance;

    public LayerMask groundMask;

    bool jumping;
    public bool sprinting;


    Vector3 velocity;
    bool isGrounded;
    float speed;


    private void Awake()
    {
        controls = new InputMaster();

        controls.Player.Jump.performed += _ => jumping = true;
        controls.Player.Sprint.performed += _ => sprinting = true;
        controls.Player.Sprint.canceled += _ => sprinting = false;
    }

    private void Start()
    {
        speed = runSpeed;

        groundDistance = controller.radius - 0.001f;
        groundCheck.y = -(controller.height / 2);
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }
    

    void LateUpdate()
    {
        if (sprinting)
        {
            speed = sprintSpeed;
        }
        else
        {
            speed = runSpeed;
        }


        isGrounded = Physics.CheckSphere(transform.position + groundCheck, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        Vector2 movement = controls.Player.Movement.ReadValue<Vector2>();
        float x = movement.x;
        float z = movement.y;

        Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(move * speed * Time.deltaTime);

        if (jumping && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
        jumping = false;

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

    }
}
