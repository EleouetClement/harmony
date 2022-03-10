using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementaryRepulse : MonoBehaviour
{
    [Header("Repulsion")]
    public float repulseStrength = 1f;

    Transform playerMesh;

    // Start is called before the first frame update
    void Start()
    {
        playerMesh = GameModeSingleton.GetInstance().GetPlayerMesh;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	private void OnCollisionEnter(Collision collision)
	{
        print("Repulsio!");
        transform.parent.position += collision.GetContact(0).normal * repulseStrength;
	}
}
