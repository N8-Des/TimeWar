using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static GlobalEnums;

public class AbilityDescription : MonoBehaviour
{
    public TextMeshProUGUI abilityName;
    public TextMeshProUGUI abilityDescription;
    public Image abilityIcon;
    public Button selectButton;
    public HorizontalLayoutGroup augmentHandler;
    public UpgradeTree uptree;
    public Upgrade selectedUpgrade;

    public AugmentDescription augmentDescription;

    public void SetAbilityDisplay(AbilityUnlock upgrade)
    {
        AbilityConfig ability = upgrade.ability;
        selectedUpgrade = upgrade;
        abilityName.text = ability.abilityName;
        abilityDescription.text = ability.abilityDescription;
        if(ability.icon != null)
        {
            abilityIcon.sprite = ability.icon;
        }
        foreach(AbilityAugment augment in upgrade.augments)
        {
            //create augment buttons
            GameObject augmentButton = Resources.Load<GameObject>("AugmentButton");
            augmentButton = Instantiate(augmentButton);
            augmentButton.transform.parent = augmentHandler.transform;
            AugmentButton button = augmentButton.GetComponent<AugmentButton>();
            button.augment = augment;
            button.description = this;

            if (augment.ability.icon != null)
            {
                button.icon.sprite = augment.ability.icon;
            }
        }

        if (uptree.CharacterHasAbilityOrAugment(ability) is AbilityUpgradeStatus.Unlocked or AbilityUpgradeStatus.Augmented)
        {
            selectButton.enabled = false;
        }
    }

    public void ClearAbilityDisplay()
    {
        for (int i = 0; i < augmentHandler.transform.childCount; i++)
        {
            Destroy(augmentHandler.transform.GetChild(i).gameObject);
        }
        augmentDescription.gameObject.SetActive(false);
    }



    public void SetAugmentDescription(AbilityAugment augment)
    {
        augmentDescription.gameObject.SetActive(true);
        augmentDescription.Setup(augment, uptree);

    }
}
