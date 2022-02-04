using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMotionController : MonoBehaviour
{
    [Header("Character settings")]
    [SerializeField] [Range(0, 20)] private float walkSpeed = 1f;
    [SerializeField] [Range(0, 200)] private float rotationSpeed = 1f;
    [SerializeField] [Range(0, 10)] private float jumpMaxHeight = 1f;
    [Header("Character Physics settings")]
    [SerializeField] [Range(0, 10)] private float dragMultipler = 1f;
    [SerializeField] [Range(0, 10)] private float airDragMultipler = 1f;
    [SerializeField] [Range(0, 1)] private float airControl = 1f;
    [Header("SlopeAnglesDetection settings")]
    [SerializeField] private float sphereCastRadius = 1;
    [SerializeField] private float sphereCastDistance = 1;
    [SerializeField] private float rayCastDistance = 1;
    [SerializeField] private int layerMask;
    [SerializeField] private float startDistanceOffset = 0.2f;
    [SerializeField] private bool debug = false;


    public float gravity = -9.81f;
    private Rigidbody char_body;
    private PlayerInput char_playerInput;
    private CharacterController char_characterControler;
    public CameraController char_cameraController;
    public Animator char_animator;
    private Vector3 forwardDirection;
    private Vector3 rightDirection;
    private Vector2 inputAxis;
    private Vector3 velocity;
    private bool onGround;
    
    private bool isJumping = false;
    // Start is called before the first frame update
    private void Awake()
    {
        char_characterControler = gameObject.GetComponent<CharacterController>();
        char_animator = gameObject.GetComponent<Animator>();
        if (char_characterControler == null)
            Debug.LogError("PlayerMotionController : Missing characterController component on object");
        if(gameObject.GetComponent<PlayerInput>() == null)
            Debug.LogError("PlayerMotionController : Missing PlayerInput component on object");
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        onGround = char_characterControler.isGrounded && GetSlopeAngle() < char_characterControler.slopeLimit;
        Move();
        Jump();
        Rotate();
        AddForce();
        char_characterControler.Move(velocity * Time.fixedDeltaTime);
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
        if(onGround)
        {
            isJumping = true;
        }
        
    }

    /// <summary>
    /// Handles Transformations
    /// </summary>
    private void Move()
    {
        forwardDirection = inputAxis.y * char_cameraController.GetViewForward;
        rightDirection = inputAxis.x * char_cameraController.GetViewRight;
        Vector3 movement = forwardDirection + rightDirection;
        movement.Normalize();
        velocity += movement * walkSpeed * Time.fixedDeltaTime * (onGround ? 1 : airControl);     
    }

    /// <summary>
    /// Handles rotations
    /// </summary>
    private void Rotate()
    {
        Vector3 cameraDirection = char_cameraController.GetViewDirection;
    }

    /// <summary>
    /// Apply forces if the player needs to jump
    /// </summary>
    private void Jump()
    {
        if(isJumping)
        {
            velocity.y = Mathf.Sqrt(jumpMaxHeight * -3.0f * gravity);
            isJumping = false;
        }
        
    }

    /// <summary>
    /// Apply forces to the character
    /// </summary>
    private void AddForce()
    {
        
        if (!onGround)
        {
            velocity.y += gravity * Time.fixedDeltaTime;
        }
        velocity += DragForce();
    }

    /// <summary>
    /// Calculate drag force for the current movement
    /// </summary>
    /// <returns></returns>
    private Vector3 DragForce()
    {
        Vector3 force = -velocity;
        force *= Time.fixedDeltaTime;
        force *= onGround ? dragMultipler : airDragMultipler;
        force = Vector3.ClampMagnitude(force, velocity.magnitude);
        return force;
    }

    /// <summary>
    /// 
    /// </summary>
    private float GetSlopeAngle()
    {
        RaycastHit hit;
        float angleOneInDegrees = Mathf.Epsilon;
        float angleTwoInDegrees = Mathf.Epsilon;// FOR LATER (Edges & corners)
        Vector3 SphereCastOrigin = new Vector3(transform.position.x, transform.position.y - (char_characterControler.height / 2) + startDistanceOffset, transform.position.z);
        if (Physics.SphereCast(SphereCastOrigin, sphereCastRadius, Vector3.down, out hit, sphereCastDistance, layerMask))
        {
            angleOneInDegrees = Vector3.Angle(hit.normal, Vector3.up);
            if(angleOneInDegrees > char_characterControler.slopeLimit && debug)
            {
                
                Debug.Log(angleOneInDegrees);
            }
        }

        return angleOneInDegrees;
    }


    void OnDrawGizmosSelected()
    {
        if (debug)
        {
            // Visualize SphereCast with two spheres and a line
            Vector3 startPoint = new Vector3(transform.position.x, transform.position.y - (char_characterControler.height / 2) + startDistanceOffset, transform.position.z);
            Vector3 endPoint = new Vector3(transform.position.x, transform.position.y - (char_characterControler.height / 2) + startDistanceOffset - sphereCastDistance, transform.position.z);
            if(startPoint != null && endPoint != null)
            {
                Gizmos.color = Color.white;
                Gizmos.DrawWireSphere(startPoint, sphereCastRadius);

                Gizmos.color = Color.gray;
                Gizmos.DrawWireSphere(endPoint, sphereCastRadius);

                Gizmos.DrawLine(startPoint, endPoint);
            }
            
        }
    }

}
