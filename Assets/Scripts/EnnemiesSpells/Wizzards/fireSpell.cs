using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fireSpell : EnnemySpell
{

    [SerializeField] EnnemiFireBall fireOrbReference;
    private EnnemiFireBall fireOrbInstance;

    private void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Charge(float chargeTime, Transform spellOrigin)
    {
        base.Charge(chargeTime, spellOrigin);
        fireOrbInstance = Instantiate(fireOrbReference, summonerPosition.position, Quaternion.identity);
    }

    public override void Launch()
    {
        
    }
}
