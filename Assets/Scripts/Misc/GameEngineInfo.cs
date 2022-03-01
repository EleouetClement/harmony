using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Class thta contains general info and utilitary enums about the entire game.
/// </summary>
public static class GameEngineInfo
{

    public enum DamageType
    {
        Physical, Earth, Water, Fire, Wind
    }


}

/// <summary>
/// Contains layer values generated at game launch. Use pointers to this instead of hardcoding layer numbers, if possible.
/// </summary>
public static class HarmonyLayers
{
    public static readonly int LAYER_DEFAULT = LayerMask.NameToLayer("Default");
    public static readonly int LAYER_TRANSPARENTFX = LayerMask.NameToLayer("TransparentFX");
    public static readonly int LAYER_IGNORERAYCAST = LayerMask.NameToLayer("Ignore Raycast");
    public static readonly int LAYER_PLAYER = LayerMask.NameToLayer("Player");
    public static readonly int LAYER_WATER = LayerMask.NameToLayer("Water");
    public static readonly int LAYER_GROUND = LayerMask.NameToLayer("Ground");
    public static readonly int LAYER_WALL_ENABLE = LayerMask.NameToLayer("WallEnable");
    public static readonly int LAYER_UI = LayerMask.NameToLayer("UI");
    public static readonly int LAYER_PLAYERSPELL = LayerMask.NameToLayer("PlayerSpell");
    public static readonly int LAYER_ENEMYSPELL = LayerMask.NameToLayer("EnemySpell");
    public static readonly int LAYER_TARGETABLE = LayerMask.NameToLayer("Targetable");
    public static readonly int LAYER_INTERACTABLE = LayerMask.NameToLayer("Interactable");
    public static readonly int LAYER_PAYERTRIGGER = LayerMask.NameToLayer("PlayerTrigger");
    public static readonly int LAYER_SHIELD = LayerMask.NameToLayer("Shield");
}
