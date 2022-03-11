using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimController : MonoBehaviour
{
    public Animator animator;

    private PlayerMotionController playerMotionController;

    private bool isJumping;
    private bool isFalling;

    Transform playerMesh;
    private Vector3 velocity;

    /// <summary>
    /// adjusts speed to which character goes back to idling animation when
    /// the player no longer press anything
    /// </summary>
    public float idlingSpeed;

    public float animSpeed;

	private void Awake()
	{
        playerMotionController = GetComponent<PlayerMotionController>();
        playerMesh = GameModeSingleton.GetInstance().GetPlayerMesh;
        idlingSpeed = 2f;
        animSpeed = 1f;
    }

	// Start is called before the first frame update
	void Start()
    {
        
    }
    
    // Update is called once per frame
    void Update()
    {
        Vector2 inputAxis = playerMotionController.GetInputAxis();
        
        velocity = playerMesh.localRotation * playerMotionController.GetVelocity();

        //print(inputAxis+" "+velocity);
        bool movingForward = playerMotionController.MovingForward;
        bool movingBackward = playerMotionController.MovingBackward;
        bool movingRight = playerMotionController.MovingRight;
        bool movingLeft = playerMotionController.MovingLeft;



        float animVelocityX = animator.GetFloat("VelocityX");
        float animVelocityZ = animator.GetFloat("VelocityZ");
        float maxSpeed = playerMotionController.GetMaxSpeed();
        float maxSpeedRatio = playerMotionController.GetMaxSpeedPercent();
        float maxSpeedRatioX = Mathf.Abs(velocity.x) / playerMotionController.GetMaxSpeed();
        float maxSpeedRatioZ = Mathf.Abs(velocity.z) / playerMotionController.GetMaxSpeed();

        if (movingForward && animVelocityZ < maxSpeedRatio)
        {
            animator.SetFloat("VelocityZ", Mathf.MoveTowards(animator.GetFloat("VelocityZ"), 1f, inputAxis.y * maxSpeedRatioZ * animSpeed * Time.deltaTime));
        }
        else if (movingBackward && animVelocityZ > -maxSpeedRatio)
        {
            animator.SetFloat("VelocityZ", Mathf.MoveTowards(animator.GetFloat("VelocityZ"), 1f, inputAxis.y * maxSpeedRatioZ * animSpeed * Time.deltaTime));
        }
        else if (!movingForward && !movingBackward)
        {
            animator.SetFloat("VelocityZ", Mathf.MoveTowards(animator.GetFloat("VelocityZ"),0f, Time.deltaTime * idlingSpeed));
        }

        if (movingRight && animVelocityX < maxSpeedRatio)
        {
            animator.SetFloat("VelocityX", Mathf.MoveTowards(animator.GetFloat("VelocityX"), 1f, inputAxis.x * maxSpeedRatioX * animSpeed * (Mathf.Abs(animator.GetFloat("VelocityX")) / 1f) * Time.deltaTime));
        }
        else if (movingLeft && animVelocityX > -maxSpeedRatio)
        {
            animator.SetFloat("VelocityX", Mathf.MoveTowards(animator.GetFloat("VelocityX"), 1f, inputAxis.x * maxSpeedRatioX * animSpeed * (Mathf.Abs(animator.GetFloat("VelocityX")) / 1f) * Time.deltaTime));
        }
        else if (!movingRight && !movingLeft)
        {
            animator.SetFloat("VelocityX", Mathf.MoveTowards(animator.GetFloat("VelocityX"), 0f, Time.deltaTime * idlingSpeed));
        }

        animator.SetBool("Moving", playerMotionController.isMoving);
        animator.SetBool("Grounded", playerMotionController.onGround);
        animator.SetBool("Jumping", playerMotionController.GetIsJumping());
        animator.SetBool("Falling", playerMotionController.GetIsFalling());

    }
}
