
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GlobalEnums : MonoBehaviour
{

    [System.Serializable]
    public struct DamageRoll
    {
        public int numDice;
        public int diceValue;
        public DamageType damageType;

        public int Roll()
        {
            int damage = 0;
            for (int i = 0; i < numDice; i++)
            {
                damage += Random.Range(1, diceValue);
            }
            return damage;
        }
    }


    [System.Serializable]
    public struct StatScaling
    { 
        public StatType statType;
        public float multiplier;
        public float offset;
        public ScalingFormula formula;

        public float GetScaledValue(CharacterStats cStats)
        {
            float scaledStat = cStats.GetStatFromEnum(statType);
            switch (formula)
            {
                case ScalingFormula.Standard:
                    return (scaledStat + offset) * multiplier;
                case ScalingFormula.Multiplication:
                    return scaledStat * multiplier;
                default: 
                    return scaledStat;            
            }
        }
    }

    [System.Serializable]
    public class Stat
    {
        public float baseValue;
        public List<Modifier> modifiers = new List<Modifier>();
        public string name;
        public StatType statType;

        public Stat(float value, StatType type)
        {
            baseValue = value;
            this.statType = type;
        }

        public void AddModifier(Modifier mod)
        {
            modifiers.Add(mod);
        }
        public void RemoveModifier(string modName)
        {
            int removeIndex = -1;
            for (int i = 0; i < modifiers.Count; i++)
            {
                if (modifiers[i].name == modName)
                {
                    removeIndex = i;
                    break;
                }
            }
            modifiers.RemoveAt(removeIndex);
        }

        public float GetValue()
        {
            float modification = 0;
            foreach (Modifier mod in modifiers)
            {
                modification += mod.modifierValue;
            }
            return modification + baseValue;
        }
    }

    [System.Serializable]
    public class Modifier
    { 
        public float modifierValue;
        public int modifierDuration;
        public string name;
    }


    public enum DamageType
    { 
        Physical,
        Holy,
        Lightning,
        Fire,
        Cold,
        Blight,
        Mental,
        True
    }

    public enum AbilityUpgradeStatus
    { 
        Inaccessible,
        Available,
        Unlocked,
        Augmented
    }

    public enum ActionType
    {
        Free,
        Primary,
        Secondary
    }

    public enum StatType
    { 
        Health,
        Power,
        Fortitude, 
        Mind,
        Movement,
        Alacrity,
        Vision,
        Protection,
        Dodge,
        Willpower
    }

    public enum ScalingFormula
    { 
        Standard,
        Multiplication,
    }

}
