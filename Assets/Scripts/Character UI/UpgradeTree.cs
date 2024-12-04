using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static GlobalEnums;

public class UpgradeTree : MonoBehaviour
{

    public List<UpgradeColumn> columns = new();
    public List<GameObject> abilityTiers = new();
    public CharacterStats characterStats;
    public CharacterLevelSet characterLevelManager;
    public int classIndex;
    GlobalValues globalValues;
    public AbilityDescription abilityDescription;
    public TextMeshProUGUI pointDisplay;
    AbilityUnlockButton abilityButton;

 
    private void Awake()
    {
        characterLevelManager.uptree = this;
        globalValues = FindObjectOfType<GlobalValues>();
        ClearUpgradeTree();
        PopulateUpgradeTree();
    }


    public void ClearUpgradeTree()
    {
        foreach (GameObject go in abilityTiers)
        {
            for (int i = 0; i < go.transform.childCount; i++)
            {
                Destroy(go.transform.GetChild(i).gameObject);
            }
            abilityDescription.gameObject.SetActive(false);
            abilityDescription.augmentDescription.gameObject.SetActive(false);
        }
    }

    public void PopulateUpgradeTree()
    {
        int abilityLevelCap = globalValues.GetAbilityLevelCap(characterStats.level);
        for (int i = 0; i < columns.Count; i++)
        {
            for (int j = 0; j < columns[i].upgradesInColumn.Count; j++)
            {
                GameObject abilityButton = Resources.Load<GameObject>("AbilityUnlockButton");
                abilityButton = Instantiate(abilityButton);
                AbilityUnlockButton abilityUnlockButton = abilityButton.GetComponent<AbilityUnlockButton>();
                AbilityUnlock abilityUnlock = columns[i].upgradesInColumn[j];
                abilityUnlockButton.Create(abilityUnlock, this, i);
                abilityButton.transform.parent = abilityTiers[i].transform;

                if (CharacterHasAbilityOrAugment(abilityUnlock.ability) == AbilityUpgradeStatus.Unlocked)
                {
                    abilityUnlockButton.SetAsUpgraded();
                }
                else if (CharacterHasAbilityOrAugment(abilityUnlock.ability) == AbilityUpgradeStatus.Augmented)
                {
                }


                if (abilityLevelCap < i + 1)
                {
                    //can't use it yet
                    abilityButton.GetComponent<Button>().enabled = true;
                }
            }
        }
    }

    public bool PurchaseUpgrade(Upgrade upgrade)
    {
        //can we actually purchase it?
        if (characterStats.availableUpgradePoints >= upgrade.cost)
        {
            characterStats.availableUpgradePoints -= upgrade.cost;
            characterStats.usedUpgradePoints += upgrade.cost;
            int upgradeIndex = AbilityRegistry.GetIDByAbility(upgrade.ability);

            upgrade.ApplyUpgrade(characterStats, upgradeIndex);

            ClearUpgradeTree();
            PopulateUpgradeTree();
            UpdatePointDisplay();

            return true;
        }
        else
        {
            return false;
        }
    }

    public bool PurchaseAugment(AbilityAugment augment)
    {
        int baseAbility = AbilityRegistry.GetIDByAbility(augment.baseAbility);

        if(characterStats.availableUpgradePoints >= augment.cost && characterStats.abilityIndices.Contains(baseAbility))
        {
            characterStats.availableUpgradePoints -= augment.cost;
            characterStats.usedUpgradePoints += augment.cost;
            int upgradeIndex = AbilityRegistry.GetIDByAbility(augment.ability);

            augment.ApplyUpgrade(characterStats, upgradeIndex);

            ClearUpgradeTree();
            PopulateUpgradeTree();
            UpdatePointDisplay();
            return true;
        }

        return false;
    }


    public void CreateCharacter()
    {
        //TODO: this whole section needs work when I implement the gameplay loop
        //instead of creating a new character, it should read from an existing CharacterStats
        characterStats = new()
        {
            classIndex = classIndex
        };
        GenerateStats(characterStats);
    }

    public AbilityUpgradeStatus CharacterHasAbilityOrAugment(AbilityConfig ability)
    {
        foreach(int i in characterStats.abilityIndices)
        {
            //the ability on the character already
            AbilityConfig characterAbility = AbilityRegistry.GetAbilityByID(i);
            //check if the ability in the characterStats ability list is an augmented ability
            if (characterAbility.augmentedAbility != null)
            {
                if (characterAbility.augmentedAbility.abilityName == ability.abilityName)
                {
                    return AbilityUpgradeStatus.Augmented;
                }
            }

            //if the ability we are checking matches either the ability of the character, or the pre-augmented version of it.
            if (ability.abilityName == characterAbility.abilityName)
            {
                return AbilityUpgradeStatus.Unlocked;
            }

           

        }
        return AbilityUpgradeStatus.Available;
    }




    public void FinalizeCharacter()
    {

    }

    public void SelectAbility(AbilityUnlock abilityUnlock, AbilityUnlockButton button)
    {
        if (abilityButton != null)
        {
            abilityButton.SetSelected(false);
        }
        abilityDescription.ClearAbilityDisplay();
        abilityDescription.gameObject.SetActive(true);
        abilityDescription.SetAbilityDisplay(abilityUnlock);
        abilityButton = button;
    }

    public void GenerateStats(CharacterStats c)
    {
        CharacterClass cClass = globalValues.classes[c.classIndex];

        c.Health.baseValue = cClass.maxHealth + Random.Range(-2, 3);
        c.Power.baseValue = cClass.power + Random.Range(-2, 3);
        c.Fortitude.baseValue = cClass.fortitude + Random.Range(-2, 3);
        c.Mind.baseValue = cClass.mind + Random.Range(-2, 3);
        c.Movement.baseValue = cClass.moveDistance;
        c.Alacrity.baseValue = cClass.alacrity;
        c.Dodge.baseValue = cClass.dodgeScore;


        //ability lookup
        //this system sucks but it works at the scale I have
        foreach (AbilityConfig abilityConfig in cClass.defaultAbilities)
        {
            int index = AbilityRegistry.GetIDByAbility(abilityConfig);
            c.abilityIndices.Add(index);

        }
    }

    public void SetCharacterLevel(int level)
    {
        characterStats.level = level;
        characterStats.availableUpgradePoints = level * 4;
        characterStats.usedUpgradePoints = 0;
        characterStats.abilityIndices.Clear();
        ClearUpgradeTree();
        PopulateUpgradeTree();
        UpdatePointDisplay();
    }

    public void UpdatePointDisplay()
    {
        pointDisplay.text = "Available Points: " + characterStats.availableUpgradePoints;
    }
}


