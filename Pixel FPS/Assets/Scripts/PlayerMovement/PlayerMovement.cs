using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.AI;

public class PlayerMovement : MonoBehaviour
{
    //public static Vector3 attackPosition = new Vector3(0f, 0.4f, 0f);

    public CharacterController controller;

    public float runSpeed;
    public float sprintSpeed;
    public float gravity = -9.81f;
    public float jumpHeight;

    public float momentumDamping;
    public float inputLerping;

    Vector3 groundCheck = Vector3.zero;
    [SerializeField] float groundDistance;
    [SerializeField] Vector2 belowMapFailsafe;

    public Controls controlsPrefab;
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
        Instantiate(controlsPrefab);
    }

    private void Start()
    {
        // Controls functions
        Controls.controls.Player.Jump.performed += _ => jumping = true;
        Controls.controls.Player.Sprint.performed += _ => sprinting = true;
        Controls.controls.Player.Sprint.canceled += _ => sprinting = false;

        speed = runSpeed;
        groundCheck.y = -(controller.height / 2);
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
        
        //isGrounded = Physics.Linecast(transform.position, transform.position - groundDistance * Vector3.down, groundMask);
        //Debug.DrawLine(transform.position, transform.position - groundDistance * Vector3.down, Color.cyan);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        // Movement
        Vector2 beforeLerping = Controls.controls.Player.Movement.ReadValue<Vector2>();
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



        if (transform.position.y < belowMapFailsafe.x)
        {
            controller.enabled = false;
            Vector3 backupPos = new Vector3(transform.position.x, belowMapFailsafe.y, transform.position.z);

            if (NavMesh.SamplePosition(transform.position, out NavMeshHit navMeshHit, 10000f, groundMask))
                transform.position = navMeshHit.position + Vector3.up * (controller.height + 0.5f);
            else
                transform.position = backupPos;
            
            controller.enabled = true;
        }
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
