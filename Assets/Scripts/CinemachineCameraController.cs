using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class CinemachineCameraController : MonoBehaviour
{
    public Transform PlayerMesh;

    [SerializeField] public GameObject exploCam;
    [SerializeField] GameObject aimingCam;
    [SerializeField] GameObject combatCam;

    private GameObject currentCam;

    [Range(0, 90)] public float verticalMaxAngle = 85.0f;
    [Min(0)] public float sensibility = 0.25f;

    private Vector3 targetPosition;
    private Vector2 lookInput;
    public Vector2 rotation;

    static private Vector3 groundVector = new Vector3(0, 1, 0);

    private CinemachineVirtualCamera cinemachineVirtualCamera;

	private void Start()
	{
        currentCam = exploCam;
        cinemachineVirtualCamera = currentCam.GetComponent<CinemachineVirtualCamera>();

    }

	private void Update()
	{

        AddYaw(lookInput.x * Time.deltaTime);
        AddPitch(-lookInput.y * Time.deltaTime);

        Quaternion camRotation = Quaternion.Euler(rotation.x, rotation.y, 0);
        transform.localRotation = camRotation;
    }

	public Vector3 GetViewDirection
    {
        get
        {
            return transform.forward;
        }
    }

    public Vector3 GetViewPosition
    {
        get
        {
            return exploCam.activeInHierarchy? exploCam.transform.position : combatCam.transform.position;
        }
    }

    public Vector3 GetViewForward
    {
        get
        {
            return Vector3.ProjectOnPlane(transform.forward, Vector3.up);
        }
    }

    public Vector3 GetViewRight
    {
        get
        {
            return transform.right;
        }
    }

    public void ZoomIn()
    {
        currentCam.SetActive(false);
        aimingCam.SetActive(true);
        currentCam = aimingCam;
    }

    public void ZoomOut()
    {
        currentCam.SetActive(false);
        combatCam.SetActive(true);
        currentCam = exploCam;
    }

    public void CombatCam()
    {
        currentCam.SetActive(false);
        combatCam.SetActive(true);
        currentCam = combatCam;
    }

    public void ExploCam()
    {
        currentCam.SetActive(false);
        exploCam.SetActive(true);
        currentCam = exploCam;
    }

    void LateUpdate()
    {
       
        
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
        rotation.x += pitch * sensibility;
        rotation.x = Mathf.Clamp(rotation.x, -verticalMaxAngle, verticalMaxAngle);
    }

    public void OnLook(InputValue value)
    {
        lookInput = value.Get<Vector2>() * 100;
    }


}
