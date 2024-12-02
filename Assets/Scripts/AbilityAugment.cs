using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Upgrades/AbiltyAugment")]
[System.Serializable]
public class AbilityAugment : Upgrade
{
    public AbilityConfig ability;
    public AbilityConfig baseAbility;

    public override void ApplyUpgrade(CharacterStats character)
    {
        throw new System.NotImplementedException();
    }
}