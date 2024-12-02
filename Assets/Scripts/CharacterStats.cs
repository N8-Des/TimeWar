using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GlobalEnums;

[System.Serializable]
public class CharacterStats
{
    [Header("Stats")]
    public Stat Health = new(0, StatType.Health);
    public Stat Power = new(0, StatType.Power);
    public Stat Fortitude = new(0, StatType.Fortitude);
    public Stat Mind = new(0, StatType.Mind);
    public Stat Movement = new(0, StatType.Movement);
    public Stat Alacrity = new(0, StatType.Alacrity);
    public Stat Vision = new(0, StatType.Vision);
    public Stat Protection = new(0, StatType.Protection);
    public Stat Dodge = new(0, StatType.Dodge);
    public Stat Willpower = new(0, StatType.Willpower);
    [HideInInspector]
    public List<Stat> statlist;
    public int level = 1;
    public int classIndex;

    [Header("Abilities")]
    public int availableUpgradePoints = 0;
    public int usedUpgradePoints = 0;
    public List<int> abilityIndices = new();


    public void CreateStatList()
    {
        statlist = new List<Stat>()
        { Health, Power, Fortitude, Mind, Movement, Alacrity, Vision, Protection, Dodge, Willpower};
    }



    public float GetStatFromEnum(StatType testedStatType)
    {
        for(int i = 0; i < statlist.Count; i++)
        {
            Stat stat = statlist[i];
            if (stat.statType == testedStatType)
            {
                return stat.GetValue();
            }
        }
        return -1;
    }
}
