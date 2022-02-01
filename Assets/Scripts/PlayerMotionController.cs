using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMotionController : MonoBehaviour
{
    [SerializeField] [Range(0, 20)] private float walkSpeed = 1f;
    [SerializeField] [Range(0, 200)] private float rotationSpeed = 1f;
    [SerializeField] [Range(0, 10)] private float jumpForce = 1f;
    private Rigidbody char_body;
    private PlayerInput char_playerInput;
    private CharacterController char_characterControler;
    public Animator char_animator;
    private float forwardDirection;
    private float rightDirection;
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
        forwardDirection = Input.GetAxis("Vertical");
        rightDirection = Input.GetAxis("Horizontal");

    }

    private void FixedUpdate()
    {
        Move();
        Rotate();
    }

    void OnMove(InputValue value)
    {
        Debug.Log(value.Get<Vector2>());
    }

    /// <summary>
    /// Handles Transformations
    /// </summary>
    private void Move()
    {
        //normalize

    }



    /// <summary>
    /// Handles rotations
    /// </summary>
    private void Rotate()
    {

    }
}
