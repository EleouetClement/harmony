using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerMotionController : MonoBehaviour
{
    [Header("Character settings")]
    [Range(0, 20)] public float walkSpeed = 1f;
    [Range(0, 10)] public float jumpForce = 1f;
    [Range(0, 90)] public float maxFloorAngle = 45;
    [Header("Character Physics settings")]
    [Range(0, 10)] public float friction = 1f;
    [Range(0, 10)] public float airFriction = 1f;
    [Range(0, 1)] public float airControl = 1f;
    [Min(0)] public float gravity = -9.81f;

    [Header("SlopeAnglesDetection settings")] 
    [SerializeField] private float groundTestRadiusFactor = 0.95f;
    [SerializeField] private float groundMaxDistance = 0.1f;
    [SerializeField] private int layerMask;
    [SerializeField] private bool debug = false;


    public CameraController cameraController;
    
    private CharacterController controller;
    private Vector3 forwardDirection;
    private Vector3 rightDirection;
    private Vector2 inputAxis;
    private Vector3 velocity;
    private bool onGround;
    private float floorAngle;
    private RaycastHit surfaceInfo;


    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        controller.slopeLimit = 90;
    }

    void Start()
    {
        
    }

    void Update()
    {

        controller.Move(velocity * Time.deltaTime);
    }

    private void FixedUpdate()
    {
        UpdateGroundState();
        bool sliding = floorAngle > maxFloorAngle;

        #region Apply Direction Input

        if (!sliding)
        {
            forwardDirection = inputAxis.y * cameraController.GetViewForward;
            rightDirection = inputAxis.x * cameraController.GetViewRight;
            Vector3 movement = forwardDirection + rightDirection;
            movement.Normalize();
            velocity += movement * (walkSpeed * Time.fixedDeltaTime * (onGround ? 1 : airControl));
        }

        #endregion

        #region Apply Gravity

        if (!onGround)
        {
            velocity.y += gravity * Time.fixedDeltaTime;
        }
        else
        {
            if (sliding)
            {
                Vector3 force = Vector3.ProjectOnPlane(Vector3.up * (gravity * Time.fixedDeltaTime), surfaceInfo.normal);

                velocity += force;
            }
        }

        #endregion

        #region Apply Friction

        Vector3 dragForce = -velocity * Time.fixedDeltaTime;
        dragForce *= onGround && !sliding ? friction : airFriction;
        dragForce = Vector3.ClampMagnitude(dragForce, velocity.magnitude);
        velocity += dragForce;

        #endregion
        
    }

    /// <summary>
    /// Handles moving inputs using InputSystem
    /// </summary>
    /// <param name="value"></param>
    void OnMove(InputValue value)
    {
        inputAxis = value.Get<Vector2>();
        
    }

    /// <summary>
    /// Handles Jump inputs
    /// </summary>
    /// <param name="value"></param>
    private void OnJump(InputValue value)
    {
        if(onGround)
        {
            onGround = false;
            velocity.y = jumpForce;
        }
        
    }

    /// <summary>
    /// 
    /// </summary>
    private void UpdateGroundState()
    {
        onGround = Physics.SphereCast(transform.position, controller.radius * groundTestRadiusFactor, Vector3.down,
            out surfaceInfo, controller.height / 2 - controller.radius + groundMaxDistance, layerMask);

        if (onGround)
        {
            floorAngle = Vector3.Angle(surfaceInfo.normal, Vector3.up);
        }
        else
        {
            floorAngle = 0;
        }
    }


    void OnDrawGizmosSelected()
    {
        if (debug && Application.isPlaying)
        {
            Vector3 end = transform.position + Vector3.down * (controller.height / 2 + groundMaxDistance - controller.radius);

            Gizmos.color = Color.white;
            Gizmos.DrawWireSphere(transform.position, controller.radius * groundTestRadiusFactor);

            Gizmos.color = Color.gray;
            Gizmos.DrawWireSphere(end, controller.radius * groundTestRadiusFactor);

            Gizmos.DrawLine(transform.position, end);
            
        }
    }

}
