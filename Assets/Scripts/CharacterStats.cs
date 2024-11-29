using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GlobalEnums;

[System.Serializable]
public class CharacterStats
{
    public Stat Health = new Stat(0);
    public Stat Power = new Stat(0);
    public Stat Fortitude = new Stat(0);
    public Stat Mind = new Stat(0);
    public Stat Movement = new Stat(0);
    public Stat Alacrity = new Stat(0);
    public Stat Vision = new Stat(0);
    public Stat Protection = new Stat(0);
    public Stat Dodge = new Stat(0);
    public Stat Willpower = new Stat(0);

    public int level = 1;

    public int classIndex;

    public List<int> abilityIndices = new();
}
