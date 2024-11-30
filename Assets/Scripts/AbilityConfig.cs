using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using static GlobalEnums;

[System.Serializable]
[CreateAssetMenu(menuName = "Abilities/StandardAbility")]
public class AbilityConfig : ScriptableObject
{

    [Header("General Settings")]
    public bool isPassive = false;
    public bool stunning = false;


    [Header("Stats")]
    public string abilityName;
    public string abilityDescription;
    public int cost;
    public int toHitModifier;
    public int damageModifier;
    public Sprite icon;
    public List<DamageRoll> damageRolls = new List<DamageRoll>();
    public ActionType actionType;
    public float attackRange;
    public MonoScript abilityType;

    [Header("Augments")]
    public bool isAugment;
    public AbilityConfig augmentedAbility;


    public Type GetAbilityType()
    {
        if (abilityType == null)
        {
            Debug.LogWarning($"AbilityType is null for {abilityName}");
            return null;
        }

        return abilityType.GetClass();
    }
}
