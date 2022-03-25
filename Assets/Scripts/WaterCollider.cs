using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterCollider : MonoBehaviour
{
    [SerializeField]
    private GameObject splashPrefab;
    private Vector3 _collisionPoint;
    private PlayerMotionController playerController;
    private float elapsedTime;
    private bool rippled;
    [SerializeField]
    private float rippleCooldown;
    
    private Vector3 CollisionPoint
    {
        get 
        {
            return _collisionPoint;
        }

        set
        {
            _collisionPoint = value;
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        playerController = GameModeSingleton.GetInstance().GetPlayerReference.GetComponent<PlayerMotionController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (rippled)
        {
            elapsedTime += Time.deltaTime;
            rippled = elapsedTime < rippleCooldown;
        }
		else
		{
            elapsedTime = 0f;
		}
		
    }

	//private void OnTriggerEnter(Collider other)
	//{
 //       _collisionPoint = other.transform.position;
 //       if(other.gameObject.layer == HarmonyLayers.LAYER_PLAYER)
 //           Instantiate(splashPrefab, other.transform.position, Quaternion.identity, transform);
 //   }

    private void OnTriggerStay(Collider other)
    {
        _collisionPoint = other.transform.position;
        if (other.gameObject.layer == HarmonyLayers.LAYER_PLAYER && playerController.Moving && !rippled)
        {
            Instantiate(splashPrefab, other.transform.position, Quaternion.identity, transform);
            rippled = true;
        }
    }
}
