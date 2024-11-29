
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GlobalEnums : MonoBehaviour
{

    public class DamageRoll
    {
        int numDice;
        int diceValue;
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
    public class Stat
    {
        public float baseValue;
        public List<Modifier> modifiers = new List<Modifier>();
        public string name;


        public Stat(float m_baseValue) => baseValue = m_baseValue;

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
        True, 
        Physical,
        Holy,
        Lightning,
        Fire,
        Cold,
        Blight,
        Mental,
    }

    public enum ActionType
    {
        Free,
        Primary,
        Secondary
    }
}
