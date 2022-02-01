using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    public Transform target;
    [SerializeField] private Camera cam;
    public Vector3 cameraRigOffet = Vector3.zero;
    [Min(0)] public float baseHorizontalDistance = 1.5f;
    [Min(0)] public float baseDistance = 3;
    public bool leftSide = false;
    [Range(0,90)] public float verticalMaxAngle = 85.0f;
    [Min(0)] public float sensibility = 0.25f;
    public LayerMask collisionMask;

    private Vector3 targetPosition;
    private Vector2 lookInput;
    private Vector2 rotation;

    private Vector3 CameraHalfExtends
    {
        get
        {
            Vector3 halfExtends;
            halfExtends.y =
                cam.nearClipPlane *
                Mathf.Tan(0.5f * Mathf.Deg2Rad * cam.fieldOfView);
            halfExtends.x = halfExtends.y * cam.aspect;
            halfExtends.z = 0f;
            return halfExtends;
        }
    }

    public Vector3 GetViewDirection
    {
        get
        {
            return cam.transform.forward;
        }
    }

    public Vector3 GetViewXZDirection
    {
        get
        {
            return transform.forward;
        }
    }

    void Awake()
    {
        if (!cam)
            cam = GetComponentInChildren<Camera>();

        targetPosition = target.position;
    }

    public void LateUpdate()
    {
        UpdateTargetPosition();
        transform.position = targetPosition + cameraRigOffet;

        AddYaw(lookInput.x * Time.deltaTime);
        AddPitch(-lookInput.y * Time.deltaTime);

        Quaternion camRotation = Quaternion.Euler(rotation.x, 0, 0);
        cam.transform.localRotation = camRotation;

        Vector3 sideDirection = leftSide ? -transform.right : transform.right;
        
        float horizontalDistance =
            Physics.BoxCast(transform.position, CameraHalfExtends, sideDirection, out RaycastHit sideHit, cam.transform.rotation, baseHorizontalDistance-cam.nearClipPlane,
                collisionMask)
                ? sideHit.distance + cam.nearClipPlane
                : baseHorizontalDistance;

        float distance = 
            Physics.BoxCast(transform.position+ sideDirection * horizontalDistance, CameraHalfExtends, -cam.transform.forward, out RaycastHit backHit, cam.transform.rotation, baseDistance - cam.nearClipPlane,
                collisionMask)
                ? backHit.distance + cam.nearClipPlane
                : baseDistance;

        Vector3 camOffset = new Vector3(leftSide ? -horizontalDistance : horizontalDistance, 0, -distance);

        cam.transform.localPosition = camRotation * camOffset;

        transform.rotation = Quaternion.Euler(0,rotation.y,0);
    }

    public void UpdateTargetPosition()
    {
        targetPosition = target.position;
    }

    public void AddYaw(float yaw)
    {
        rotation.y += yaw * sensibility;

        if (rotation.y < 0f)
            rotation.y += 360f;
        else if (rotation.y >= 360f)
            rotation.y -= 360f;
    }

    public void AddPitch(float pitch)
    {
        rotation.x += pitch* sensibility;
        rotation.x = Mathf.Clamp(rotation.x, -verticalMaxAngle, verticalMaxAngle);
    }

    public void OnLook(InputValue value)
    {
        lookInput = value.Get<Vector2>()*100;
    }
}
