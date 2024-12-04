using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Upgrades/AbiltyAugment")]
[System.Serializable]
public class AbilityAugment : Upgrade
{
    public AbilityConfig baseAbility;

    public override void ApplyUpgrade(CharacterStats character, int index)
    {
        int baseID = AbilityRegistry.GetIDByAbility(baseAbility);
        int myID = AbilityRegistry.GetIDByAbility(ability);
        foreach (int i in character.abilityIndices)
        {
            if (i == baseID)
            {
                character.abilityIndices.Remove(i);
                character.abilityIndices.Add(myID);
                return;
            }
        }
    }
}