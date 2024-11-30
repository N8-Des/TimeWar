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
    public abstract void ApplyUpgrade(CharacterStats character);
}

[CreateAssetMenu(menuName = "Upgrades/StatUpgrade")]
public class StatUpgrade : Upgrade
{
    public int ValueIncrease;
    public StatType statType;

    public override void ApplyUpgrade(CharacterStats character)
    {
        //EWWWWWWW LOOK AWAY UGLY CODE
        switch (statType)
        {
            case StatType.Health:
                character.Health.baseValue += ValueIncrease;
                break;
            case StatType.Power:
                character.Power.baseValue += ValueIncrease;
                break;
            case StatType.Mind:
                character.Mind.baseValue += ValueIncrease;
                break;
            case StatType.Fortitude:
                character.Fortitude.baseValue += ValueIncrease;
                break;
            case StatType.Movement:
                character.Movement.baseValue += ValueIncrease;
                break;
            case StatType.Alacrity:
                character.Alacrity.baseValue += ValueIncrease;
                break;
            case StatType.Vision:
                character.Vision.baseValue += ValueIncrease;
                break;
            case StatType.Protection:
                character.Protection.baseValue += ValueIncrease;
                break;
            case StatType.Dodge:
                character.Dodge.baseValue += ValueIncrease;
                break;
            case StatType.Willpower:
                character.Willpower.baseValue += ValueIncrease;
                break;
            default:
                break;
        }
    }
}

[CreateAssetMenu(menuName = "Upgrades/AbilityUnlock")]
public class AbilityUnlock : Upgrade
{
    public AbilityConfig ability;
    public List<AbilityAugment> augments = new List<AbilityAugment>();

    public override void ApplyUpgrade(CharacterStats character)
    {
        throw new System.NotImplementedException();
    }
}

[CreateAssetMenu(menuName = "Upgrades/AbiltyAugment")]
public class AbilityAugment : Upgrade
{
    public AbilityConfig ability;
    public AbilityConfig baseAbility;

    public override void ApplyUpgrade(CharacterStats character)
    {
        throw new System.NotImplementedException();
    }
}



