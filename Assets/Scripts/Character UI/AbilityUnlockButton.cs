using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AbilityUnlockButton : MonoBehaviour
{
    public AbilityUnlock upgrade;
    public UpgradeTree myTree;
    public Image abilityIcon;
    int abilityTier;
    bool isSelected = false;

    public void Create(AbilityUnlock ability, UpgradeTree tree, int tier)
    {
        upgrade = ability;
        myTree = tree;
        abilityTier = tier;
        if (ability.ability.icon != null)
        {
            abilityIcon.sprite = ability.ability.icon;
        }
    }

    private void Update()
    {
        
    }

    public void OnClick()
    {
        myTree.SelectAbility(this);
        isSelected = true;
    }

    public void SetSelected(bool selected)
    {
        isSelected = selected;
    }
}
