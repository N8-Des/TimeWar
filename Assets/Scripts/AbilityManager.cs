using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityManager : MonoBehaviour
{
    public List<AbilityBase> abilities = new List<AbilityBase>();

    public void InitializeAbilities(List<AbilityConfig> abilityConfigs)
    {
        foreach (AbilityConfig config in abilityConfigs)
        {
            AbilityBase ability = CreateAbility(config);
            if (ability != null)
            {

                abilities.Add(ability);
            }
        }
    }


    private AbilityBase CreateAbility(AbilityConfig config)
    {
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
