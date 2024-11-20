using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GlobalEnums;

[System.Serializable]
public class Character : MonoBehaviour
{
    public int level;
    public int health;
    public int maxHealth;
    public List<Ability> abilities = new();
    public CharacterClass myClass;
    public List <DamageType> resistedTypes = new List<DamageType>();
    public List<DamageType> immuneTypes = new List<DamageType>();



    //STAT BLOCK
    public float moveDistance = 30;
    public int alacrity = 0;
    public float visionRadius = 20;
    public float currentMoveDistance;

    public int power = 10;
    public int fortitude = 10;
    public int mind = 10;
    

    public float dodge = 10;
    public float protection = 0;
    public bool isAlly = true;


    int turnsStunned = 0;



    public void TakeDamage(int damage, Character source, DamageType damageType)
    {
        if (resistedTypes.Contains(damageType))
        {
            damage = Mathf.FloorToInt(damage);
        }else if (immuneTypes.Contains(damageType))
        {
            damage = 0;
        }

        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }


    public void Die()
    {

    }

    public void StartRound()
    {
        currentMoveDistance = moveDistance;
    }

    
}
