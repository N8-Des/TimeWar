using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GlobalEnums;

[System.Serializable]
public class Character : MonoBehaviour
{
    public int health;
    public float currentMoveDistance;

    public CharacterStats stats;
    public CharacterClass myClass;
    public List <DamageType> resistedTypes = new List<DamageType>();
    public List<DamageType> immuneTypes = new List<DamageType>();
    public AbilityManager abilityManager;



    public bool isAlly = true;

    int turnsStunned = 0;


    private void Awake()
    {
        abilityManager = GetComponent<AbilityManager>();
    }

    public void SetStats(CharacterStats stats)
    {
        this.stats = stats;
    }

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
        currentMoveDistance = stats.Movement.GetValue();
    }

    
}
