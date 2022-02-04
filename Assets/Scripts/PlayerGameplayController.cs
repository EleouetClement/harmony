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
                // TODO : Find a smarter way to instanciate the right spell here.
                AbstractSpell s = Instantiate(elementaryController.spells[0], elementaryController.transform.position, Quaternion.identity);
                s.init(elementaryController.gameObject, Vector3.zero);
                if (s is WaterMissiles)
                {
                    Collider[] enemies = Physics.OverlapSphere(Vector3.zero, 200f, 1 << HarmonyLayers.LAYER_TARGETABLE);
                    if (enemies.Length >= 1)
                        ((WaterMissiles)s).targetTransform = enemies[0].gameObject.transform;
                }
                elementaryController.CastSpell(s);
            }

        }
        if (!value.isPressed && elementaryController.currentSpell != null && !elementaryController.currentSpell.isReleased())
            elementaryController.currentSpell?.OnRelease();
    }

    private void OnSpellRight(InputValue value)
    {
        if(elementaryController.currentSpell == null)
        {
            if (value.isPressed)
            {
                AbstractSpell spell = Instantiate(elementaryController.spells[1], elementaryController.transform.position, Quaternion.identity);
                spell.init(elementaryController.gameObject, playerCameraController.GetViewDirection);
                elementaryController.CastSpell(spell);
            }
        }
        Debug.Log(value.isPressed);
        
        if (!value.isPressed && elementaryController.currentSpell != null && !elementaryController.currentSpell.isReleased())
        {
            Debug.Log("liberation tim�e");
            elementaryController.currentSpell?.OnRelease();
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
