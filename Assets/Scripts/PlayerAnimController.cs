using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimController : MonoBehaviour
{
    public Animator animator;

    private PlayerMotionController playerMotionController;

    private bool isJumping;
    private bool isFalling;

    /// <summary>
    /// adjusts speed to which character goes back to idling animation when
    /// the player no longer press anything
    /// </summary>
    public float idlingInterpolationValue = 4f;

	private void Awake()
	{
        playerMotionController = GetComponent<PlayerMotionController>();


    }

	// Start is called before the first frame update
	void Start()
    {
        
    }
    
    // Update is called once per frame
    void Update()
    {
        Vector2 inputAxis = playerMotionController.GetInputAxis();
        Vector3 velocity = playerMotionController.GetVelocity();

        //print(inputAxis+" "+velocity);
        bool movingForward = playerMotionController.movingForward;
        bool movingBackward = playerMotionController.movingBackward;
        bool movingRight = playerMotionController.movingRight;
        bool movingLeft = playerMotionController.movingLeft;



        float animVelocityX = animator.GetFloat("VelocityX");
        float animVelocityZ = animator.GetFloat("VelocityZ");
        float playerVelocityX = Mathf.Abs(velocity.x);
        float playerVelocityZ = Mathf.Abs(velocity.z);
        float maxSpeed = playerMotionController.GetMaxSpeedApprox();
        float maxSpeedRatio = playerMotionController.GetMaxSpeedRatio();
        //print(playerVelocityZ + " " + playerVelocityX);

        if (movingForward && animVelocityZ < maxSpeedRatio)
        {
            animator.SetFloat("VelocityZ", animVelocityZ + inputAxis.y * playerVelocityZ * Time.deltaTime);
        }
        else if (movingBackward && animVelocityZ > -maxSpeedRatio)
        {
            animator.SetFloat("VelocityZ", animVelocityZ + inputAxis.y * playerVelocityZ * Time.deltaTime);
        }
        else if (!movingForward && !movingBackward)
        {
            animator.SetFloat("VelocityZ", Mathf.Lerp(animator.GetFloat("VelocityZ"),0f, Time.deltaTime * idlingInterpolationValue));
        }

        if (movingRight && animVelocityX < maxSpeedRatio)
        {
            animator.SetFloat("VelocityX", animVelocityX + inputAxis.x * playerVelocityX * Time.deltaTime);
        }
        else if (movingLeft && animVelocityX > -maxSpeedRatio)
        {
            animator.SetFloat("VelocityX", animVelocityX + inputAxis.x * playerVelocityX * Time.deltaTime);
        }
        else if (!movingRight && !movingLeft)
        {
            animator.SetFloat("VelocityX", Mathf.Lerp(animator.GetFloat("VelocityX"), 0f, Time.deltaTime * idlingInterpolationValue));
        }

        animator.SetBool("Moving", playerMotionController.isMoving);
        animator.SetBool("Grounded", playerMotionController.onGround);
        animator.SetBool("Jumping", playerMotionController.GetIsJumping());
        animator.SetBool("Falling", playerMotionController.GetIsFalling());

    }
}
