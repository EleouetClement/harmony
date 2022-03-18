using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimController : MonoBehaviour
{
    public Animator animator;

    private PlayerMotionController playerMotionController;
    private ElementaryController elementary;

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
        
        idlingSpeed = 2f;
        animSpeed = 1f;
    }

    // Start is called before the first frame update
    void Start()
    {
        elementary = GameModeSingleton.GetInstance().GetElementaryReference.GetComponent<ElementaryController>();
        playerMesh = GameModeSingleton.GetInstance().GetPlayerMesh;
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

        if (!elementary.isAiming)
        {
            if (movingForward)
                animator.SetFloat("VelocityZ", Mathf.MoveTowards(animator.GetFloat("VelocityZ"), maxSpeedRatio, inputAxis.magnitude * animSpeed * Time.deltaTime));
            else
                animator.SetFloat("VelocityZ", Mathf.MoveTowards(animator.GetFloat("VelocityZ"), 0f, Time.deltaTime * idlingSpeed));
        }
        else
        {
            if (movingForward)
            {
                animator.SetFloat("VelocityZ", Mathf.MoveTowards(animator.GetFloat("VelocityZ"), maxSpeedRatioZ, inputAxis.y * animSpeed * Time.deltaTime));
            }
            else if (movingBackward)
            {
                animator.SetFloat("VelocityZ", Mathf.MoveTowards(animator.GetFloat("VelocityZ"), maxSpeedRatioZ, inputAxis.y * animSpeed * Time.deltaTime));
            }
            else if (!movingForward && !movingBackward)
            {
                animator.SetFloat("VelocityZ", Mathf.MoveTowards(animator.GetFloat("VelocityZ"), 0f, Time.deltaTime * idlingSpeed));
            }

            if (movingRight)
            {
                animator.SetFloat("VelocityX", Mathf.MoveTowards(animator.GetFloat("VelocityX"), maxSpeedRatioX, inputAxis.x * animSpeed * Time.deltaTime));
            }
            else if (movingLeft)
            {
                print(maxSpeedRatioX);
                animator.SetFloat("VelocityX", Mathf.MoveTowards(animator.GetFloat("VelocityX"), -maxSpeedRatioX, -inputAxis.x * animSpeed * Time.deltaTime));
            }
            else if (!movingRight && !movingLeft)
            {

                animator.SetFloat("VelocityX", Mathf.MoveTowards(animator.GetFloat("VelocityX"), 0f, Time.deltaTime * idlingSpeed));
            }
        }

        animator.SetBool("Moving", playerMotionController.isMoving);
        animator.SetBool("Grounded", playerMotionController.onGround);
        animator.SetBool("Jumping", playerMotionController.GetIsJumping());
        animator.SetBool("Falling", playerMotionController.GetIsFalling());

    }
}
