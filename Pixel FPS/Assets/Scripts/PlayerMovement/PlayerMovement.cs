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

    public float momentumDamping;
    public float inputLerping;

    Vector3 groundCheck = Vector3.zero;
    [SerializeField] float groundDistance;

    public LayerMask groundMask;

    bool jumping;
    public bool sprinting;
    
    Vector3 momentum;
    Vector3 velocity;
    bool isGrounded;
    float speed;

    Vector2 lastInput;


    private void Awake()
    {
        // Controls functions
        controls = new InputMaster();

        controls.Player.Jump.performed += _ => jumping = true;
        controls.Player.Sprint.performed += _ => sprinting = true;
        controls.Player.Sprint.canceled += _ => sprinting = false;
    }

    private void Start()
    {
        speed = runSpeed;
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


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position + groundCheck, groundDistance);
    }


    // Handles player movement
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

        // Movement
        Vector2 beforeLerping = controls.Player.Movement.ReadValue<Vector2>();
        Vector2 movement = LerpInput(beforeLerping);
        Vector3 move;
        float x = movement.x;
        float z = movement.y;

        if (movement != Vector2.zero)
        {
            move = transform.right * x * speed + transform.forward * z * speed;
            momentum = move;
        }
        else
        {
            momentum = Vector3.Lerp(momentum, Vector3.zero, momentumDamping * Time.deltaTime  );
            momentum = new Vector3(momentum.x, 0f, momentum.z);
            move = momentum;
        }

        move = RemoveY(move);
        controller.Move(move * Time.deltaTime);

        // Jumping
        if (jumping && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
        jumping = false;

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

    }


    Vector3 RemoveY(Vector3 vector3)
    {
        return new Vector3(vector3.x, 0f, vector3.z);
    }


    Vector2 LerpInput(Vector2 newInput)
    {
        Vector2 returns = Vector2.Lerp(lastInput, newInput, inputLerping * Time.deltaTime);
        lastInput = returns;
        return returns;
    }
}
