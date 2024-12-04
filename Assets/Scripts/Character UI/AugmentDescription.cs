using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static GlobalEnums;

public class AugmentDescription : MonoBehaviour
{
    public TextMeshProUGUI augmentName;
    public TextMeshProUGUI augmentDescription;
    public TextMeshProUGUI augmentCost;
    public Image icon;
    public AbilityAugment augment;
    public Button unlockButton;
    
    UpgradeTree uptree;

    public void Setup(AbilityAugment newAugment, UpgradeTree tree)
    {
        augment = newAugment;
        augmentName.text = newAugment.upgradeName;
        augmentDescription.text = newAugment.description;
        if (newAugment.ability.icon != null)
        {
            icon.sprite = newAugment.ability.icon;
        }
        augmentCost.text = augment.cost + " AP";
        uptree = tree;

        if (uptree.CharacterHasAbilityOrAugment(augment.baseAbility) is AbilityUpgradeStatus.Augmented)
        {
            unlockButton.interactable = false;
            unlockButton.GetComponentInChildren<TextMeshProUGUI>().color = Color.gray;
        }
        else
        {
            unlockButton.interactable = true;
            unlockButton.GetComponentInChildren<TextMeshProUGUI>().color = Color.white;
        }
    }

    public void OnUnlockButton()
    {
        uptree.PurchaseAugment(augment);
    }
}
