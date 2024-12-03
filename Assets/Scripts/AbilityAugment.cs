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
        throw new System.NotImplementedException();
    }
}