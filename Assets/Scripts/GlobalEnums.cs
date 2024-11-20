
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

    public enum Stat
    {
        Strength,
        Dexterity,
        Mind,
        Endurance,
        Resolve
    }


}
