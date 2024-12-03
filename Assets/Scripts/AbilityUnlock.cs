using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Upgrades/AbilityUnlock")]
[System.Serializable]
public class AbilityUnlock : Upgrade
{
    public List<AbilityAugment> augments = new List<AbilityAugment>();

    public override void ApplyUpgrade(CharacterStats character, int index)
    {
        character.abilityIndices.Add(index);
    }
}
