using System.Collections;
using System.Collections.Generic;
using Harmony.AI;
using UnityEngine;

public class StaticWizardAI : AIAgent
{
    [SerializeField] Transform spawner;
    [SerializeField] EnnemySpell spell;

    EnnemySpell currentSpell;

    public void FireQuick()
    {
        spawner.LookAt(GameModeSingleton.GetInstance().GetPlayerReference.transform);
        currentSpell = Instantiate(spell, spawner.position, Quaternion.identity);
        currentSpell.Charge(EnnemySpell.CastType.quick, spawner);
    }

    public void FireCharged()
    {
        spawner.LookAt(GameModeSingleton.GetInstance().GetPlayerReference.transform);
        currentSpell = Instantiate(spell, spawner.position, Quaternion.identity);
        currentSpell.Charge(EnnemySpell.CastType.charge, spawner);
    }
}
