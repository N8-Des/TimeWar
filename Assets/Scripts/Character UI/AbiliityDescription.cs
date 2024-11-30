using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AbiliityDescription : MonoBehaviour
{
    public TextMeshProUGUI abilityName;
    public TextMeshProUGUI abilityDescription;
    public Image abilityIcon;
    public Button selectButton;
    public HorizontalLayoutGroup augmentHandler;
    public UpgradeTree uptree;
    public Upgrade selectedUpgrade;

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

        }

        if (uptree.CharacterHasAbilityOrAugment(ability))
        {
            selectButton.enabled = false;
        }

    }
}
