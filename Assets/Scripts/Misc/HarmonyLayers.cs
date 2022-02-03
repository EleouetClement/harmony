using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class HarmonyLayers
{

    public static readonly int LAYER_DEFAULT = LayerMask.NameToLayer("Default");
    public static readonly int LAYER_TRANSPARENTFX = LayerMask.NameToLayer("TransparentFX");
    public static readonly int LAYER_IGNORERAYCAST = LayerMask.NameToLayer("Ignore Raycast");
    public static readonly int LAYER_PLAYER = LayerMask.NameToLayer("Player");
    public static readonly int LAYER_WATER = LayerMask.NameToLayer("Water");
    public static readonly int LAYER_UI = LayerMask.NameToLayer("UI");
    public static readonly int LAYER_PLAYERSPELL = LayerMask.NameToLayer("PlayerSpell");
    public static readonly int LAYER_ENEMYSPELL = LayerMask.NameToLayer("EnemySpell");
    public static readonly int LAYER_TARGETABLE = LayerMask.NameToLayer("Targetable");
    public static readonly int LAYER_INTERACTABLE = LayerMask.NameToLayer("Interactable");
    public static readonly int LAYER_PAYERTRIGGER = LayerMask.NameToLayer("PlayerTrigger");

}
