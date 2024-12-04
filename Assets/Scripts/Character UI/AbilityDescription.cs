using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static GlobalEnums;
using static UnityEngine.Rendering.HableCurve;

public class AbilityDescription : MonoBehaviour
{
    public TextMeshProUGUI abilityName;
    public TextMeshProUGUI abilityDescription;
    public Image abilityIcon;
    public Button selectButton;
    public TextMeshProUGUI abilityCost;
    public VerticalLayoutGroup augmentHandler;
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
        abilityCost.text = upgrade.cost + " AP";

        foreach (AbilityAugment augment in upgrade.augments)
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
            if (uptree.CharacterHasAbilityOrAugment(ability) == AbilityUpgradeStatus.Augmented)
            {
                if (uptree.characterStats.abilityIndices.Contains(AbilityRegistry.GetIDByAbility(augment.ability)))
                {
                    button.SetUpgraded();
                }
            }
        }

        if (uptree.CharacterHasAbilityOrAugment(ability) is AbilityUpgradeStatus.Unlocked or AbilityUpgradeStatus.Augmented)
        {
            selectButton.interactable = false;
            selectButton.GetComponentInChildren<TextMeshProUGUI>().color = Color.gray;
        }
        else
        {
            selectButton.interactable = true;
            selectButton.GetComponentInChildren<TextMeshProUGUI>().color = Color.white;
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

    public void OnUnlockButton()
    {
        uptree.PurchaseUpgrade(selectedUpgrade);
    }

    public void SetAugmentDescription(AbilityAugment augment)
    {
        augmentDescription.gameObject.SetActive(true);
        augmentDescription.Setup(augment, uptree);

    }
}
