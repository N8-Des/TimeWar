using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GlobalEnums;

[CreateAssetMenu(menuName = "Abilities/StandardAbility")]
public class Ability : ScriptableObject
{
    public string abilityName;
    public string abilityDescription;
    public int cost;
    public int toHitModifier;
    public int damageModifier;
    public Sprite icon;
    public List<DamageRoll> damageRolls = new List<DamageRoll>();
    public ActionType actionType;
    public Stat scaledStat;
    public float attackRange;


    //default ability: make attack roll and deal damage to target
    public virtual void Execute(Character caster, Character target, Vector3 targetLocation)
    {
        //get stat bonuses
        int statBonus = caster.GetScoreWithEnum(scaledStat);
        statBonus = Mathf.CeilToInt(((float)statBonus - 10) / 2);
        int levelBonus = Mathf.FloorToInt((float)caster.level / 2);
        //TODO : extra to-hit bonuses from equipment and buffs

        int toHit = Random.Range(1, 20);
        toHit += statBonus + levelBonus;

        if (toHit >= target.protectionScore)
        {
            int totalDamage = 0;
            foreach(DamageRoll roll in damageRolls)
            {
                totalDamage += roll.Roll();
                target.TakeDamage(totalDamage + statBonus, caster, roll.damageType);
            }
        }

    }
}
