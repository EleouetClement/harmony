using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BasicController : MonoBehaviour
{
    private CharacterController controller;
    private Vector2 moveInput;
    private float speed = 7;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    private void FixedUpdate()
    {
        controller.Move((transform.forward * moveInput.y + transform.right * moveInput.x)*Time.fixedDeltaTime* speed);
    }

    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }
}
