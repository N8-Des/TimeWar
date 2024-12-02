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
    public Sprite unlockedAbilityIcon;
    public Sprite augmentedAbilityIcon;

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

    public void OnClick()
    {
        myTree.SelectAbility(upgrade);
        isSelected = true;
    }

    public void SetAsUpgraded()
    {
        GetComponent<Image>().sprite = unlockedAbilityIcon;
    }

    public void SetAsAugmented()
    {
        GetComponent<Image>().sprite = augmentedAbilityIcon;
    }

    public void SetSelected(bool selected)
    {
        isSelected = selected;
    }
}
