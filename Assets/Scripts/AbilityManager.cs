using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityManager : MonoBehaviour
{
    public List<AbilityBase> abilities = new List<AbilityBase>();
    public List<AbilityBase> passives = new List<AbilityBase>();
    public Character character;

    public void InitializeAbilities(List<AbilityConfig> abilityConfigs)
    {
        foreach (AbilityConfig config in abilityConfigs)
        {
            AbilityBase ability = CreateAbility(config);
            if (ability != null)
            {
                if (ability.config.isPassive)
                {
                    passives.Add(ability);
                }
                else
                {
                    abilities.Add(ability);
                }
                ability.Initialize(character);
            }
        }
    }


    private AbilityBase CreateAbility(AbilityConfig config)
    {
        //get the type of the ability's AbilityBase
        Type abilityType = config.GetAbilityType();
        if (config.abilityType == null || !typeof(AbilityBase).IsAssignableFrom(abilityType))
        {
            Debug.LogWarning($"Invalid AbilityType for {config.name}");
            return null;
        }

        //create new instance of the script
        AbilityBase ability = (AbilityBase)gameObject.AddComponent(abilityType);
        ability.config = config;
        return ability;
    }
}
