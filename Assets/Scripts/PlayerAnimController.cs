using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimController : MonoBehaviour
{
    public Animator animator;

    private PlayerMotionController playerMotionController;

    private bool movingForward;
    private bool movingBackward;
    private bool movingRight;
    private bool movingLeft;

	private void Awake()
	{
        playerMotionController = GetComponent<PlayerMotionController>();

        movingForward = false;
        movingBackward = false;
        movingRight = false;
        movingLeft = false;


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

        print(inputAxis+" "+velocity);
       

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

        float animVelocityX = animator.GetFloat("VelocityX");
        float animVelocityZ = animator.GetFloat("VelocityZ");

        if (movingForward && animVelocityZ < 1f)
        {
            animator.SetFloat("VelocityZ", animVelocityZ + inputAxis.y * Mathf.Abs(velocity.z) * Time.deltaTime);
        }
        else if (movingBackward && animVelocityZ > -1f)
        {
            animator.SetFloat("VelocityZ", animVelocityZ + inputAxis.y * Mathf.Abs(velocity.z) * Time.deltaTime);
        }
        else if (!movingForward && !movingBackward)
        {
            animator.SetFloat("VelocityZ", 0f);
        }

        if (movingRight && animVelocityX < 1f)
        {
            animator.SetFloat("VelocityX", animVelocityX + inputAxis.x * Mathf.Abs(velocity.x) * Time.deltaTime);
        }
        else if (movingLeft && animVelocityX > -1f)
        {
            animator.SetFloat("VelocityX", animVelocityX + inputAxis.x * Mathf.Abs(velocity.x) * Time.deltaTime);
        }
        else if (!movingRight && !movingLeft)
        {
            animator.SetFloat("VelocityX", 0f);
        }
    }
}
