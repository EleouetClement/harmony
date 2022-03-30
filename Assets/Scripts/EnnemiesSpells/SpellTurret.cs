using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellTurret : MonoBehaviour
{

    [SerializeField] Transform spawner;
    [SerializeField] EnnemySpell [] spells;
    private Dictionary<AbstractSpell.Element, EnnemySpell> EnnemySpells;

    private bool disabled = false;

    private void Awake()
    {
        if(spells == null || spells.Length == 0)
        {
            Debug.LogError("Spell turret : no spell to Launch");
            disabled = true;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
