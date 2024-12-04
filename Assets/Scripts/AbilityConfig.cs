using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using static GlobalEnums;

[System.Serializable]
[CreateAssetMenu(menuName = "Ability")]
public class AbilityConfig : ScriptableObject
{

    [Header("General Settings")]
    public bool isPassive = false;
    public bool isStunning = false;
    public bool isShoving = false;
    public bool passiveInUpdate = false;

    public int stunDuration;
    public float shoveDistance;


    [Header("Stat Calculations")]
    public StatScaling toHitFormula;
    public StatScaling damageFormula;
    public StatScaling saveFormula;


    [Header("Stats")]
    public string abilityName;
    [TextArea(10, 10)]
    public string abilityDescription;
    public int cost;
    public int toHitModifier;
    public int damageModifier;
    public Sprite icon;
    public List<DamageRoll> damageRolls = new List<DamageRoll>();
    public DamageType damageType;
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
