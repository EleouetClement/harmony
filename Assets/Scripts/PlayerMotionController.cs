using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerMotionController : MonoBehaviour
{
    [SerializeField] private bool debug = false;

    [Header("Character Stats")]
    public float currentSpeed;


    [Header("Character settings")]
    [Range(0, 100)] public float walkMaxSpeed;
    [Range(0, 100)] public float strafeMaxSpeed;
    [Range(0, 100)] public float backWalkMaxSpeed;
    [Range(0, 100)] public float shieldingMaxSpeed;
    /// <summary>
    /// speed at which the player turns towards velocity or towards aiming direction
    /// </summary>
    [Range(0, 100)] public float turnSpeed;
    [Range(0, 100)] public float timeToMaxSpeed;
    private float maxSpeed;
    private float maxSpeedPercent;
    private float walkSpeed;
    private float strafeSpeed;
    private float backWalkSpeed;
    private float walkAcceleration;
    private float strafeAcceleration;
    private float backWalkAcceleration;
    [Range(0, 20)] public float jumpForce = 1f;
    [Range(0, 90)] public float maxFloorAngle = 45;
    [Header("Character Physics settings")]
    private float friction;
    [Range(0, 100)] public float aimingFriction;
    [Range(0, 100)] public float maxTurnFriction;
    [Range(0, 100)] public float decelerationFriction;
    [Range(0, 10)] public float airFriction = 1f;
    [Range(0, 1)] public float airControl = 1f;
    [Range(1f, 10f)] public float fallGravityMultiplier = 1f;
    [Range(1f, 10f)] public float jumpGravityMultiplier = 1f;
    [SerializeField] private float slopeForce = 9.81f;
    [SerializeField] private float slideForce = 1;
    [Min(0)] public float gravity = 9.81f;
    public LayerMask layerMask;

    [Header("Dodge settings")]
    [SerializeField] [Min(0)] private float dodgeSpeed;
    [SerializeField] [Min(0)] private float dodgeDuration;
    [SerializeField] [Min(0)] private float dodgeCoolDown;
    [SerializeField] private bool isInvincible;

    [Header("SlopeAnglesDetection settings")]
    [SerializeField] private float groundTestRadiusFactor = 0.95f;
    [SerializeField] private float groundMaxDistance = 0.1f;

    public CinemachineCameraController cinemachineCamera;
    private ElementaryController elementary;
    public Transform playerMesh;

    private CharacterController controller;
    private Vector3 forwardDirection;
    private Vector3 rightDirection;
    private Vector2 inputAxis;
    public Vector2 lastInputAxis;
    public Vector3 velocity;
    [HideInInspector] public bool onGround;
    private float floorAngle;
    private RaycastHit surfaceInfo;
    private Transform groundTranform;
    private Vector3 lastGroundPos;
    public bool isMoving = false;
    private bool isDodging = false;
    private bool isFalling = false;
    private bool isJumping = false;
    public bool isTurning = false;
    public bool sliding = false;
    public bool isShielding = false;

    private bool movingForward = false;
    private bool movingBackward = false;
    private bool movingRight = false;
    private bool movingLeft = false;

   

    private float currentDodgeDuration = Mathf.Epsilon;
    private float dodgeTimer;

    /// <summary>
    /// Pointer to the last seen climbable ledge. May be nul until the player has encountered one.
    /// </summary>
    [HideInInspector]
    public ClimbableLedgeBehavior lastLedge;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        controller.slopeLimit = 90;


        movingForward = false;
        movingBackward = false;
        movingRight = false;
        movingLeft = false;

        walkSpeed = 0f;
        strafeSpeed = 0f;
        backWalkSpeed = 0f;
        elementary = GameModeSingleton.GetInstance().GetElementaryReference.GetComponent<ElementaryController>();
    }

    void Update()
    {
		#region Stuff
		currentSpeed = velocity.magnitude;
        walkAcceleration = walkMaxSpeed / timeToMaxSpeed;
        strafeAcceleration = strafeMaxSpeed / timeToMaxSpeed;
        backWalkAcceleration = backWalkMaxSpeed / timeToMaxSpeed;

        Vector3 playerOffset = Vector3.zero;
        if (onGround && groundTranform)
        {
            playerOffset = groundTranform.position - lastGroundPos;
            lastGroundPos = groundTranform.position;
        }


        
        controller.Move(velocity * Time.deltaTime + playerOffset);
        if(dodgeTimer > Mathf.Epsilon)
        {
           dodgeTimer -= Time.deltaTime;
        }
        isMoving = (Mathf.Abs(inputAxis.x) + Mathf.Abs(inputAxis.y)) != 0;
        
        
        if(debug)
            Debug.DrawLine(transform.position, transform.position + velocity);

		#endregion

	}

	private void FixedUpdate()
    {

        Vector3 dir = new Vector3(velocity.x, 0f, velocity.z).normalized;
        
        if(dir != Vector3.zero)
            isTurning = Quaternion.Angle(playerMesh.localRotation, Quaternion.LookRotation(dir)) > 1f;

        //smooth turning when moving
        if (isMoving)
        {
            if (!elementary.isAiming && dir != Vector3.zero)
                playerMesh.localRotation = Quaternion.Slerp(playerMesh.localRotation, Quaternion.LookRotation(dir), Time.deltaTime * turnSpeed);
            else
                playerMesh.localRotation = Quaternion.Slerp(playerMesh.localRotation, Quaternion.Euler(playerMesh.localRotation.x, cinemachineCamera.rotation.y, 0), Time.deltaTime * turnSpeed);
        }


        //player facing in front of them when aiming
        if (elementary.isAiming)
            playerMesh.localRotation = Quaternion.Slerp(playerMesh.localRotation, Quaternion.Euler(playerMesh.localRotation.x, GameModeSingleton.GetInstance().GetCinemachineCameraController.rotation.y, 0),Time.deltaTime *  turnSpeed);
		

		UpdateGroundState();
        sliding = floorAngle > maxFloorAngle;
        

       

        #region Friction Calculations
		if (onGround)
        {
            if (isMoving)
            {
                if (isTurning && !elementary.isAiming)
                {
                     friction = (Quaternion.Angle(playerMesh.localRotation, Quaternion.LookRotation(new Vector3(velocity.x, 0f, velocity.z).normalized)) / 180f) * maxTurnFriction; 
                }
                else if (isTurning)
                    friction = aimingFriction;
                else
                    friction = 0f;
            }
		    else
		    {
                friction = decelerationFriction;
		    }
        }
		else
		{
            friction = airFriction;
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

        #region Apply Direction Input

        if (/*!sliding &&*/ !isDodging)
        {
            CheckMovement();
            GetDirection();
            UpdateSpeeds();
            UpdateMaxSpeed();

            if (isMoving && !elementary.isAiming)
            {
                velocity += GetDirection() * walkAcceleration * Time.deltaTime * (onGround ? 1 : airControl);
                
            }
            else if (isMoving)
            {
                if (movingForward)
			    {
				    velocity += forwardDirection.normalized * walkAcceleration * Time.deltaTime * (onGround ? 1 : airControl);
                }
                if (movingRight || movingLeft)
                {
				    velocity += rightDirection.normalized * strafeAcceleration * Time.deltaTime * (onGround ? 1 : airControl);
                }
                if (movingBackward)
                {
                    velocity += forwardDirection.normalized * backWalkAcceleration * Time.deltaTime * (onGround ? 1 : airControl);
                }
                //prevent character from bouncing when going down a slope
                if (OnSlope())
                {
                    velocity += Vector3.down * slopeForce * Time.fixedDeltaTime;
                }
            }

        }



        #endregion

        #region Apply Gravity

        if (!onGround)
        {
            velocity.y += -gravity * Time.fixedDeltaTime;
            //falling
            if (controller.velocity.y < 0f || sliding)
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

                velocity += force * slideForce;
            }
        }

		#endregion

		#region Apply Friction

		Vector3 dragForce = -velocity * Time.fixedDeltaTime;
		dragForce *= onGround && !sliding ? friction : airFriction;
		dragForce = Vector3.ClampMagnitude(dragForce, velocity.magnitude);
		velocity += dragForce;

        #endregion

        //clamping depending on maxSpeed
        if (onGround)
        {
            velocity = Vector3.ClampMagnitude(velocity, maxSpeed);
        }

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
        if (lastLedge && lastLedge.cantrigger) {
            lastLedge.Trigger();
            // TODO: maybe se something here to know the player is climbing? Use states please, not a random bool flag.
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

    
    private void UpdateGroundState()
    {
        onGround = Physics.SphereCast(transform.position, controller.radius * groundTestRadiusFactor, Vector3.down,
            out surfaceInfo, controller.height / 2 - controller.radius + groundMaxDistance, layerMask, QueryTriggerInteraction.Ignore);

		if (onGround)
        {
            floorAngle = Vector3.Angle(surfaceInfo.normal, Vector3.up);

            if (groundTranform != surfaceInfo.transform)
            {
                groundTranform = surfaceInfo.transform;
                lastGroundPos = groundTranform.position;
            }
        }
        else
        {
            floorAngle = 0;
            groundTranform = null;
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

    public float GetMaxSpeed()
    {
        return maxSpeed;
    }
    /// <summary>
    /// Return current speed / max speed
    /// </summary>
    /// <returns></returns>
    public float GetMaxSpeedPercent()
    {
        return maxSpeedPercent;
    }

    public bool GetIsJumping()
    {
        return isJumping;
    }

    public bool GetIsFalling()
    {
        return isFalling;
    }

    public bool MovingForward 
    {
        get 
        {
            return movingForward;
        }
    }
    public bool MovingBackward 
    {
        get 
        {
            return movingBackward;
        }
    }
    public bool MovingRight 
    {
        get 
        {
            return movingRight;
        }
    }
    public bool MovingLeft 
    {
        get 
        {
            return movingLeft;
        }
    }

    public float MaxSpeed
    {
        get
        {
            return maxSpeed;
        }
        set
        {
            maxSpeed = value;
        }
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

    /// <summary>
    /// Update moving states based on player inputs
    /// </summary>
    private void CheckMovement()
    {
        if (isMoving && !elementary.isAiming)
        {
            movingForward = true;
            movingBackward = false;
            movingLeft = false;
            movingRight = false;
        }
        else if(isMoving)
        {
            if (Mathf.Abs(inputAxis.y) > 0f)
            {
                movingForward = inputAxis.y > float.Epsilon;
                movingBackward = inputAxis.y < float.Epsilon;
            }
            else
            {
                movingForward = false;
                movingBackward = false;
            }
            if (Mathf.Abs(inputAxis.x) > float.Epsilon)
            {
                movingRight = inputAxis.x > float.Epsilon;
                movingLeft = inputAxis.x < float.Epsilon;
            }
            else
            {
                movingRight = false;
                movingLeft = false;
            }
        }
		else
		{
            movingForward = false;
            movingBackward = false;
            movingLeft = false;
            movingRight = false;
        }
    }

    /// <summary>
    /// Update each speed variable based on current moving states
    /// </summary>
    private void UpdateSpeeds()
    {
        if (MovingForward)
        {
            walkSpeed = Mathf.MoveTowards(walkSpeed, walkMaxSpeed, walkAcceleration * Time.deltaTime);
        }
		else
		{
            walkSpeed = Mathf.MoveTowards(walkSpeed, 0f, walkAcceleration * Time.deltaTime);
        }

        if (movingRight || movingLeft)
        {
            strafeSpeed = Mathf.MoveTowards(strafeSpeed, strafeMaxSpeed, strafeAcceleration * Time.deltaTime);
        }
		else
		{
            strafeSpeed = Mathf.MoveTowards(strafeSpeed, 0f, strafeAcceleration * Time.deltaTime);
        }

        if (movingBackward)
        {
            backWalkSpeed = Mathf.MoveTowards(backWalkSpeed, backWalkMaxSpeed, backWalkAcceleration * Time.deltaTime);
        }
		else
		{
            backWalkSpeed = Mathf.MoveTowards(backWalkSpeed, 0f, backWalkAcceleration * Time.deltaTime);
        }
    }

    /// <summary>
    /// Update maxSpeed and maxSpeedPercent values
    /// </summary>
    private void UpdateMaxSpeed()
    {
        if (isJumping)
        {
            maxSpeed = jumpForce;
        }
        else if (isMoving && !elementary.isAiming)
        {
            if (isShielding)
                maxSpeed = shieldingMaxSpeed;
            else
                maxSpeed = walkMaxSpeed;
        }
        else if(isMoving)
        {
            if(isShielding)
                maxSpeed = shieldingMaxSpeed;
            else if (movingForward)
                maxSpeed = walkMaxSpeed;
            else if (movingBackward)
                maxSpeed = backWalkMaxSpeed;
            else if (movingRight || movingLeft)
                maxSpeed = strafeMaxSpeed;
        }
        
        maxSpeedPercent = currentSpeed / maxSpeed;
    }
}
