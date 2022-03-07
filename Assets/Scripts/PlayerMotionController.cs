using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerMotionController : MonoBehaviour
{
    public float currentSpeed;

    [Header("Character settings")]
    [Range(0, 100)] public float walkSpeed = 1f;
    [Range(0, 20)] public float jumpForce = 1f;
    [Range(0, 90)] public float maxFloorAngle = 45;
    public float turnSpeed;
    [Header("Character Physics settings")]
    private float friction;
    [Range(0, 100)] public float accelerationFriction;
    [Range(0, 100)] public float decelerationFriction;
    [Range(0, 10)] public float airFriction = 1f;
    [Range(0, 1)] public float airControl = 1f;
    [Min(0)] public float gravity = 9.81f;
    [Range(1f, 10f)] public float fallGravityMultiplier = 1f;
    [Range(1f, 10f)] public float jumpGravityMultiplier = 1f;
    [SerializeField] private float slopeForce = 9.81f;
    public LayerMask layerMask;

    [Header("Dodge settings")]
    [SerializeField] [Min(0)] private float dodgeSpeed;
    [SerializeField] [Min(0)] private float dodgeDuration;
    [SerializeField] [Min(0)] private float dodgeCoolDown;
    [SerializeField] private bool isInvincible;

    [Header("SlopeAnglesDetection settings")]
    [SerializeField] private float groundTestRadiusFactor = 0.95f;
    [SerializeField] private float groundMaxDistance = 0.1f;
    [SerializeField] private bool debug = false;

    public CinemachineCameraController cinemachineCamera;
    public Transform playerMesh;

    private CharacterController controller;
    private Vector3 forwardDirection;
    private Vector3 rightDirection;
    private Vector2 inputAxis;
    [HideInInspector]public Vector3 velocity;
    [HideInInspector] public bool onGround;
    private float floorAngle;
    private RaycastHit surfaceInfo;
    public bool isMoving = false;
    private bool isDodging = false;
    private bool isFalling = false;
    private bool isJumping = false;

    private float maxSpeedApprox;
    private float maxSpeedRatio;

    private float currentDodgeDuration = Mathf.Epsilon;
    private float dodgeTimer;


    

 

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        controller.slopeLimit = 90;
        maxSpeedApprox = (walkSpeed / accelerationFriction) - 0.3f;
    }

    void Update()
    {
        //prevent character from bouncing when going down a slope
        if (isMoving && OnSlope())
        {
            velocity += Vector3.down * slopeForce * Time.fixedDeltaTime;
        }

        maxSpeedRatio = currentSpeed / maxSpeedApprox;
        controller.Move(velocity * Time.deltaTime);
        if(dodgeTimer > Mathf.Epsilon)
        {
           dodgeTimer -= Time.deltaTime;
        }

        currentSpeed = controller.velocity.magnitude;
        isMoving = (Mathf.Abs(inputAxis.x) + Mathf.Abs(inputAxis.y)) != 0;

        //smooth turning when moving
        if (isMoving)
        {
            playerMesh.localRotation = Quaternion.Slerp(playerMesh.localRotation, Quaternion.Euler(playerMesh.localRotation.x, cinemachineCamera.rotation.y, 0), Time.deltaTime * turnSpeed);
        }

       

    }

    private void FixedUpdate()
    {
        if (isMoving)
        {
            friction = accelerationFriction;
        }
		else
		{
            friction = decelerationFriction;
		}
        UpdateGroundState();
        bool sliding = floorAngle > maxFloorAngle;
        

        #region Apply Direction Input

        if (!sliding && !isDodging)
        {           
            velocity += GetDirection() * (walkSpeed * Time.fixedDeltaTime * (onGround ? 1 : airControl));
        }
        


        #endregion


        #region Dodge force
        if (isDodging && dodgeTimer <= Mathf.Epsilon)
        {
            if(currentDodgeDuration < dodgeDuration)
            {
                //Vector3 newDir = new Vector3(inputAxis.x, Mathf.Epsilon, inputAxis.y);               
                Vector3 newDir = GetDirection() * dodgeSpeed * Time.fixedDeltaTime;
                newDir.y = Mathf.Epsilon;
                transform.Translate(newDir);
                currentDodgeDuration += Time.fixedDeltaTime;
            }
            else
            {
                currentDodgeDuration = Mathf.Epsilon;
                isDodging = false;
                dodgeTimer = dodgeCoolDown;
            }
            
        }

        #endregion

        #region Apply Gravity

        if (!onGround)
        {
            velocity.y += -gravity * Time.fixedDeltaTime;
            //falling
            if (controller.velocity.y < 0f)
            {
                isFalling = true;
                isJumping = false;
                velocity.y += -gravity * (fallGravityMultiplier - 1f) * Time.fixedDeltaTime;
            }
            //rising
            else if (controller.velocity.y > 0f)
            {
                velocity.y += -gravity * (jumpGravityMultiplier - 1f) * Time.fixedDeltaTime;
            }
        }
        else
        {
            isFalling = false;
            isJumping = false;
            if (sliding)
            {
                Vector3 force = Vector3.ProjectOnPlane(Vector3.up * (-gravity * Time.fixedDeltaTime), surfaceInfo.normal);

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

    void LateUpdate()
    {
		
	}

	/// <summary>
	/// Handles moving inputs using InputSystem
	/// </summary>
	/// <param name="value"></param>
	void OnMove(InputValue value)
    {
        inputAxis = value.Get<Vector2>();
        isMoving = true;
    }

    /// <summary>
    /// Handles Jump inputs
    /// </summary>
    /// <param name="value"></param>
    private void OnJump(InputValue value)
    {
        if(onGround)
        {
            isJumping = true;
            onGround = false;
            velocity.y = jumpForce;
        }
    }


    private void OnDodge(InputValue value)
    {
        if (isMoving && !isDodging && dodgeTimer <= Mathf.Epsilon)
        {
            Debug.Log("Dodge : " + isDodging);
            isDodging = true;
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

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if(hit.gameObject.tag.Equals("Platform"))
        {
            transform.parent = hit.transform;
        }
        else
        {
            transform.parent = null;
        }
    }

    

    //void OnDrawGizmosSelected()
    //{
    //    if (debug && Application.isPlaying)
    //    {
    //        Vector3 end = transform.position + Vector3.down * (controller.height / 2 + groundMaxDistance - controller.radius);

    //        Gizmos.color = Color.white;
    //        Gizmos.DrawWireSphere(transform.position, controller.radius * groundTestRadiusFactor);

    //        Gizmos.color = Color.gray;
    //        Gizmos.DrawWireSphere(end, controller.radius * groundTestRadiusFactor);

    //        Gizmos.DrawLine(transform.position, end);

    //    }
    //}

    private Vector3 GetDirection()
    {
        forwardDirection = inputAxis.y * cinemachineCamera.GetViewForward;
        rightDirection = inputAxis.x * cinemachineCamera.GetViewRight;
        Vector3 direction = forwardDirection + rightDirection;
        direction.Normalize();

        return direction;
    }


    public Vector2 GetInputAxis()
    {
        return inputAxis;
    }

    public Vector3 GetVelocity()
    {
        return velocity;
    }

    public float GetMaxSpeedApprox()
    {
        return maxSpeedApprox;
    }
    public float GetMaxSpeedRatio()
    {
        return maxSpeedRatio;
    }

    public bool GetIsJumping()
    {
        return isJumping;
    }

    public bool GetIsFalling()
    {
        return isFalling;
    }

    /// <summary>
    /// Returns true if the player is on a slope
    /// </summary>
    /// <returns></returns>
    private bool OnSlope()
    {
        if (isJumping)
            return false;

        RaycastHit hit;

        if (Physics.Raycast(transform.position + Vector3.down * controller.height / 2, Vector3.down, out hit, 0.1f, layerMask))
        {
            return hit.normal != Vector3.up;
        }

        return false;
    }
}
