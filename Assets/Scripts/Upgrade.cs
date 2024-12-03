using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GlobalEnums;

[System.Serializable]

public abstract class Upgrade :  ScriptableObject
{
    public string upgradeName;
    public string description;
    public int cost;
    public AbilityConfig ability;

    public abstract void ApplyUpgrade(CharacterStats character, int index);
}







