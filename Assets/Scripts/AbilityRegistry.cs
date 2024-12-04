using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AbilityRegistry
{
    private static Dictionary<int, AbilityConfig> idToAbility  = new Dictionary<int, AbilityConfig>();

    public static AbilityConfig GetAbilityByID(int id)
    {
        idToAbility.TryGetValue(id, out AbilityConfig config);
        return config;
    }

    public static int GetIDByAbility(AbilityConfig ability)
    {
        foreach (var pair in idToAbility)
        {
            if(pair.Value == ability)
            {
                return pair.Key;
            }
        }
        return -1;
    }

    public static void RegisterAbility(int id, AbilityConfig ability)
    {
        if (!idToAbility.ContainsKey(id))
        {
            idToAbility.Add(id, ability);
        }
    }

    public static void InitializeAbilities()
    {
        AbilityConfig[] abilities = Resources.LoadAll<AbilityConfig>("Abilities");

        for(int i = 0; i < abilities.Length; i++)
        {
            RegisterAbility(i, abilities[i]);
        }
    }

}
