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
        if (elementaryController.currentSpell == null)
        {
            if (value.isPressed)
            {
                AbstractSpell s = Instantiate(elementaryController.spells[0], elementaryController.transform.position, Quaternion.identity);
                s.init(elementaryController.gameObject, Vector3.zero);
                if (s is WaterMissiles)
                    ((WaterMissiles)s).targetTransform = null;
                elementaryController.CastSpell(s);
            }

        }
        if (!value.isPressed && elementaryController.currentSpell != null && !elementaryController.currentSpell.isReleased())
            elementaryController.currentSpell?.OnRelease();
    }

    private void OnSpellRight()
    {
        Debug.Log("FireBall");
        if (elementaryController.currentSpell == null)
        {          
            AbstractSpell spell = Instantiate(elementaryController.spells[1], elementaryController.transform.position, Quaternion.identity);
            spell.init(elementaryController.gameObject, playerCameraController.GetViewDirection);
            elementaryController.CastSpell(spell);
        }    
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
