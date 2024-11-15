using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GlobalEnums;

public class Character : MonoBehaviour
{
    public int level;
    public int health;
    public int maxHealth;
    public List<Ability> abilities = new();


    //STAT BLOCK
    public float moveDistance = 30;
    public int alacrity = 0;
    public float visionRadius = 20;
    public float currentMoveDistance;

    public int strength;
    public int dexterity;
    public int mind;
    public int endurance;
    public int resolve;

    public int protectionScore;
    public int dodgeScore;
    public int fortitudeScore;
    public int willpowerScore;

    public bool isAlly = true;


    public void TakeDamage(int damage, Character source, DamageType damageType)
    {

    }


    public void StartRound()
    {
        currentMoveDistance = moveDistance;
    }

    public int GetScoreWithEnum(Stat stat)
    {
        switch (stat)
        {
            case Stat.Strength:
            
                return strength;
            
            case Stat.Dexterity:
            
                return dexterity;
            
            case Stat.Mind:
            
                return mind;
            
            case Stat.Endurance:
            
                return endurance;

            case Stat.Resolve:
      
                return resolve;
            
            default:

                return 0;
        }
    }
}
