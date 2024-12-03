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
    public Image abilityHoverSpin;
    public float rotationSpeed = 1f;

    public Sprite unlockedAbilityIcon;

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

    public void OnClick()
    {
        myTree.SelectAbility(upgrade, this);
        SetSelected(true);
    }

    public void SetAsUpgraded()
    {
        GetComponent<Image>().sprite = unlockedAbilityIcon;
    }

    public void SetAsAugmented()
    {
        //nothing yet, maybe ever  
    }

    public void SetSelected(bool selected)
    {
        isSelected = selected;
        abilityHoverSpin.gameObject.SetActive(selected);
    }

    private void Update()
    {
        if (isSelected)
        {
            abilityHoverSpin.rectTransform.eulerAngles -= Vector3.forward * Time.deltaTime * rotationSpeed;
            abilityHoverSpin.gameObject.SetActive(true);
        }
    }
}
