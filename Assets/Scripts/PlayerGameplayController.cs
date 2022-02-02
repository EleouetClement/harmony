using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerGameplayController : MonoBehaviour
{
    [Header("Elementary")]
    [SerializeField] GameObject elementaryObjectReference;
    [SerializeField] GameObject playerMeshReference;
    [SerializeField] private CameraController playerCameraController;
    private ElementaryController elementaryController;

    private void Awake()
    {
        InitializeElementary();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       
    }


    private void OnSpellLeft(InputValue value)
    {
        Debug.Log("Attack");
        elementaryController.CastSpell(new WaterMissiles(gameObject, null, 100f));
    }



    /// <summary>
    /// Set the Elementary shoulder reference
    /// </summary>
    private void InitializeElementary()
    {
        if (elementaryObjectReference == null)
        {
            Debug.LogError("PlayerMotionController : Missing Elementary reference");
        }
        else
        {
            elementaryController = elementaryObjectReference.GetComponent<ElementaryController>();
            if (elementaryController == null)
            {
                Debug.LogError("PlayerMotionController : Current elementary hasn't any ElementaryController component");
            }
            else
            {
                if (playerMeshReference == null)
                {
                    Debug.LogError("PlayerMotionController : Missing Mesh reference");
                }
                else
                {
                    elementaryController.SetPLayerOrigin(playerMeshReference.transform);
                }
            }

        }
        
    }
}
