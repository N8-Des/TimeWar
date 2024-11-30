using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GlobalEnums;

[System.Serializable]
public class CharacterStats
{
    [Header("Stats")]
    public Stat Health = new(0);
    public Stat Power = new(0);
    public Stat Fortitude = new(0);
    public Stat Mind = new(0);
    public Stat Movement = new(0);
    public Stat Alacrity = new(0);
    public Stat Vision = new(0);
    public Stat Protection = new(0);
    public Stat Dodge = new(0);
    public Stat Willpower = new(0);
    public int level = 1;
    public int classIndex;

    [Header("Abilities")]
    public int availableUpgradePoints = 0;
    public int usedUpgradePoints = 0;
    public List<int> abilityIndices = new();
}
