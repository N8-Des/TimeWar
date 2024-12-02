using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AugmentDescription : MonoBehaviour
{
    public TextMeshProUGUI augmentName;
    public TextMeshProUGUI augmentDescription;
    public TextMeshProUGUI augmentCost;
    public AbilityAugment augment;

    
    UpgradeTree uptree;

    public void Setup(AbilityAugment newAugment, UpgradeTree tree)
    {
        augment = newAugment;
        augmentName.text = newAugment.upgradeName;
        augmentDescription.text = newAugment.description;
        augmentCost.text = augment.cost + " AP";
        uptree = tree;
    }

    public void OnUnlockButton()
    {
        uptree.PurchaseUpgrade(augment);
    }
}
