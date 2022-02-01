using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMotionController : MonoBehaviour
{
    [SerializeField] [Range(0, 20)] private float walkSpeed = 1f;
    [SerializeField] [Range(0, 200)] private float rotationSpeed = 1f;
    [SerializeField] [Range(0, 10)] private float jumpMaxHeight = 1f;
    private Rigidbody char_body;
    private PlayerInput char_playerInput;
    private CharacterController char_characterControler;
    public Animator char_animator;
    public float gravity = -9.81f;

    private Vector3 forwardDirection;
    private Vector3 rightDirection;
    private Vector2 inputAxis;
    private Vector3 velocity;
    
    private bool isJumping = false;
    // Start is called before the first frame update
    private void Awake()
    {
        char_characterControler = gameObject.GetComponent<CharacterController>();
        char_animator = gameObject.GetComponent<Animator>();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //forwardDirection = Input.GetAxis("Vertical");
        //rightDirection = Input.GetAxis("Horizontal");

    }

    private void FixedUpdate()
    {
        Move();
        Jump();
        Rotate();
        AddForce();
    }

    /// <summary>
    /// Handles moving inputs using InputSystem
    /// </summary>
    /// <param name="value"></param>
    void OnMove(InputValue value)
    {
        //Debug.Log(value.Get<Vector2>());
        inputAxis = value.Get<Vector2>();
        
    }

    /// <summary>
    /// Handles Jump inputs
    /// </summary>
    /// <param name="value"></param>
    private void OnJump(InputValue value)
    {
        Debug.Log("jump");
        if(char_characterControler.isGrounded)
        {
            isJumping = true;
        }
        
    }

    /// <summary>
    /// Handles Transformations
    /// </summary>
    private void Move()
    {
        forwardDirection = new Vector3(0, 0, inputAxis.y * transform.forward.z);
        rightDirection = inputAxis.x * transform.right;
        Vector3 movement = forwardDirection + rightDirection;
        movement.Normalize();
        velocity = movement * walkSpeed * Time.fixedDeltaTime;     
    }

    /// <summary>
    /// Handles rotations
    /// </summary>
    private void Rotate()
    {

    }

    /// <summary>
    /// Apply forces if the player needs to jump
    /// </summary>
    private void Jump()
    {
        if(isJumping)
        {
            velocity.y += Mathf.Sqrt(jumpMaxHeight * -3.0f * gravity);
            isJumping = false;
        }
        velocity.y += gravity * Time.fixedDeltaTime;
    }

    /// <summary>
    /// Apply the velocity to the character
    /// </summary>
    private void AddForce()
    {
        char_characterControler.Move(velocity);
    }
}
