using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellTurret : MonoBehaviour
{

    [SerializeField] Transform spawner;
    [SerializeField] EnnemySpell [] spells;
    [SerializeField] EnnemySpell.CastType testCastType;
    private Dictionary<AbstractSpell.Element, EnnemySpell> ennemySpells;

    EnnemySpell currentSpell;
    GameModeSingleton gm;
    private bool disabled = false;

    private void Start()
    {
        ennemySpells = new Dictionary<AbstractSpell.Element, EnnemySpell>();
        if(spells == null || spells.Length == 0)
        {
            Debug.LogError("Spell turret : no spell to Launch");
            disabled = true;
        }
        else
        {
            foreach(EnnemySpell s in spells)
            {
                ennemySpells.Add(s.element, s);
            }
        }
        gm = GameModeSingleton.GetInstance();
    }


    // Update is called once per frame
    void Update()
    {
        transform.LookAt(gm.GetPlayerReference.transform);
        if(!disabled)
        {
            if (Input.GetKeyDown(KeyCode.F1))
            {
                if(ennemySpells.TryGetValue(AbstractSpell.Element.Fire, out currentSpell))
                {
                    currentSpell = Instantiate(currentSpell, spawner.position, Quaternion.identity);
                    currentSpell.Charge(testCastType, spawner);
                }
                else
                {
                    Debug.LogWarning("Spell turret : no fire spell");
                }             
            }
            else if (Input.GetKeyDown(KeyCode.F2))
            {
                if (ennemySpells.TryGetValue(AbstractSpell.Element.Water, out currentSpell))
                {
                    currentSpell = Instantiate(currentSpell, spawner.position, Quaternion.identity);
                    currentSpell.Charge(testCastType, spawner);
                }
                else
                {
                    Debug.LogWarning("Spell turret : no water spell");
                }
            }
            else if (Input.GetKeyDown(KeyCode.F3))
            {
                if (ennemySpells.TryGetValue(AbstractSpell.Element.Earth, out currentSpell))
                {
                    currentSpell = Instantiate(currentSpell, spawner.position, Quaternion.identity);
                    currentSpell.Charge(testCastType, spawner);
                }
                else
                {
                    Debug.LogWarning("Spell turret : no earth spell");
                }
            }
        }        
    }
}
