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

    [Header("Aiming setups")]
    [SerializeField] [Min(0)]private float fovReductionPerFrame;

    private Vector3 targetPosition;
    private Vector2 lookInput;
    private Vector2 rotation;
    /// <summary>
    /// true if a spll needs a zoom to aim
    /// </summary>
    private bool isAiming = false;
    private float fovZoomValue;
    private float fovBaseValue;
    private bool needView;

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

    public Vector3 GetViewPosition
    {
        get
        {
            return cam.transform.position;
        }
    }

    public Vector3 GetViewForward
    {
        get
        {
            return transform.forward;
        }
    }

    public Vector3 GetViewRight
    {
        get
        {
            return transform.right;
        }
    }

    void Awake()
    {
        if (!cam)
            cam = GetComponentInChildren<Camera>();

        targetPosition = target.position;
        fovBaseValue = cam.fieldOfView;
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
                collisionMask, QueryTriggerInteraction.Ignore)
                ? sideHit.distance + cam.nearClipPlane
                : baseHorizontalDistance;

        float distance = 
            Physics.BoxCast(transform.position+ sideDirection * horizontalDistance, CameraHalfExtends, -cam.transform.forward, out RaycastHit backHit, cam.transform.rotation, baseDistance - cam.nearClipPlane,
                collisionMask, QueryTriggerInteraction.Ignore)
                ? backHit.distance + cam.nearClipPlane
                : baseDistance;

        Vector3 camOffset = new Vector3(leftSide ? -horizontalDistance : horizontalDistance, 0, -distance);

        cam.transform.localPosition = camRotation * camOffset;

        transform.rotation = Quaternion.Euler(0,rotation.y,0);
        #region Aiming
        if (isAiming)
        {
            Zoom();
        }
        else
        {
            if(cam.fieldOfView < fovBaseValue)
            {
                DeZoom();
            }
        }
        #endregion

        #region View
        if(needView)
        {
            SetGlobalView();
        }
        else
        {
            //TO DO...
            SetBaseView();
        }

        #endregion
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

    #region Crossair Aim

    private void Zoom()
    {
        if(cam.fieldOfView > fovZoomValue)
        {
            cam.fieldOfView -= fovReductionPerFrame;
        }
    }

    private void DeZoom()
    {
        if (cam.fieldOfView < fovBaseValue)
        {
            cam.fieldOfView += fovReductionPerFrame;
        }
    }

    /// <summary>
    /// Reduces fov while aiming for spell
    /// </summary>
    public void Aim(float newFovValue)
    {
        isAiming = true;
        fovZoomValue = newFovValue;
    }

    ///<summary>
    /// recover base fov
    /// </summary>
    public void StopAim()
    {
        isAiming = false;
    }
    #endregion

    #region Camera Global view
    /// <summary>
    /// Translate the camera upper end further from the player to get a global view
    /// </summary>
    public void GloabView()
    {
        needView = true;
    }
    /// <summary>
    /// Returns the camera to its base setup
    /// </summary>
    public void ResetView()
    {
        needView = false;
    }

    private void SetGlobalView()
    {
        //Debug.Log("Global view enabled");
        
    }

    private void SetBaseView()
    {
        //TO DO...
        //Debug.Log("Global view disabled");
    }

    #endregion

}
