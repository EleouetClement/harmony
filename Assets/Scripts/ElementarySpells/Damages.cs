using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName ="DamagesValues", menuName ="Harmony/Damages", order = 1)]
public class Damages : ScriptableObject
{
    [Min(0)] public float baseDamages;
    [Min(0)] public float maxMultiplier;
    [Min(0)] public float blinkMultiplier;
}
