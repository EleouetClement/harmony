using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : AbstractSpell
{
    [Range(0f, 2f)] public float maxDelayToPerfectShield;
    [Range(0f, 20f)] public float walkSpeedInShield;
    
    private GameObject player;
    private float initialWalkSpeed;
    private bool canPerfectShield = true;
    private float timer = 0; // Start from 0 to maxDelayToPerfectShield

    private void Start()
    {
        //player = GameModeSingleton._instance.GetPlayerReference;
        player = GameObject.Find("Player");
        transform.position = player.transform.position;
        initialWalkSpeed = player.GetComponent<PlayerMotionController>().walkSpeed;
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        transform.position = player.transform.position;
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer > maxDelayToPerfectShield && canPerfectShield)
        {
            Debug.Log("You can not do perfect shield anymore");
            canPerfectShield = false;
        }

        if (!isReleased())
        {
            player.GetComponent<PlayerMotionController>().walkSpeed = walkSpeedInShield;
        }
    }

    public override void init(GameObject elemRef, Vector3 target)
    {
        base.init(elemRef, target.normalized);
    }

    public override void Terminate()
    {
        ElementaryController elemCtrl = elementary.GetComponent<ElementaryController>();
        elemCtrl.currentSpell = null;
        elemCtrl.computePosition = true;
        Destroy(gameObject);
    }

    protected override void onChargeEnd(float chargetime)
    {
        player.GetComponent<PlayerMotionController>().walkSpeed = initialWalkSpeed;
        Terminate();
    }
}
