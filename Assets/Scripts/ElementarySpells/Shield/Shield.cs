using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : AbstractSpell
{
    [Range(0f, 2f)] public float maxDelayToPerfectShield;
    [Range(0f, 20f)] public float walkSpeedInShield;
    [SerializeField] GameObject shieldPrefab;
    private GameModeSingleton gms;
    private GameObject player;
    private float initialWalkSpeed; // Storage of the initial speed of the player
    private bool canPerfectShield = true;
    private float timer = 0; // Start from 0 to maxDelayToPerfectShield
    private ElementaryController elemController;
    private GameObject shieldReference;
    private Collider shieldCollider;
    private MeshRenderer shieldVisual;
    private void Awake()
    {
        
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        transform.position = player.transform.position;
        //shieldReference.transform.position = player.transform.position;
    }

    private void Update()
    {
        bool debug = elemController.IsElementaryAway();
        if (!debug)
        {
            if(shieldCollider.enabled == false)
            {
                shieldCollider.enabled = true;
                shieldVisual.enabled = true;
            }
            else
            {
                //Debug.Log("Timer On");
                timer += Time.deltaTime;
            }   
        }
        // If the shield has been activated for too long time, it can no longer maker a perfect shield
        if (timer > maxDelayToPerfectShield && canPerfectShield)
        {
            Debug.Log("You can not do perfect shield anymore");
            canPerfectShield = false;
        }

        // The player is slowed if his shield is activated
        if (!isReleased())
        {
            player.GetComponent<PlayerMotionController>().walkSpeed = walkSpeedInShield;
        }
    }

    public override void init(GameObject elemRef, Vector3 target)
    {
        base.init(elemRef, target.normalized);
        elemController = elementary.GetComponent<ElementaryController>();
        if(elemController.IsElementaryAway())
        {
            elemController.Recall();
        }
        gms = GameModeSingleton.GetInstance();
        player = gms.GetPlayerReference;
        transform.position = player.transform.position; // place the shield on the position of the player
        initialWalkSpeed = player.GetComponent<PlayerMotionController>().walkSpeed;
        shieldCollider = GetComponent<Collider>();
        shieldVisual = GetComponent<MeshRenderer>();
        if (elemController.IsElementaryAway())
        {
            shieldCollider.enabled = false;
            shieldVisual.enabled = false;
        }
    }

    public override void Terminate()
    {
        elementary.GetComponent<ElementaryController>().Reset();
        player.GetComponent<PlayerMotionController>().walkSpeed = initialWalkSpeed;
        //Destroy(shieldReference);
        Destroy(gameObject);
    }

    protected override void onChargeEnd(float chargetime)
    {
        player.GetComponent<PlayerMotionController>().walkSpeed = initialWalkSpeed;
        Terminate();
    }

    private void OnTriggerEnter(Collider collider)
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == HarmonyLayers.LAYER_ENEMYSPELL)
        {
            if (canPerfectShield)
            {
                Debug.Log("PERFECT SHIELD !");
            }
            else
            {
                Debug.Log("TOO LATE TO PERFECT SHIELD !");
            }

            Destroy(collision.gameObject);
        }
    }

}
