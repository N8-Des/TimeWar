using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static GlobalEnums;

public class UpgradeTree : MonoBehaviour
{

    public List<UpgradeColumn> columns = new();
    public List<GameObject> abilityTiers = new();
    public CharacterStats characterStats;
    public int classIndex;
    GlobalValues globalValues;
    AbilityDescription abilityDescription;

    private void Awake()
    {
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

                if (CharacterHasAbilityOrAugment(abilityUnlock.ability) == AbilityUpgradeStatus.Unlocked)
                {
                    abilityUnlockButton.SetAsUpgraded();
                }
                else if (CharacterHasAbilityOrAugment(abilityUnlock.ability) == AbilityUpgradeStatus.Augmented)
                {
                    abilityUnlockButton.SetAsAugmented();
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
            upgrade.ApplyUpgrade(characterStats);
            ClearUpgradeTree();
            PopulateUpgradeTree();

            return true;
        }
        else
        {
            return false;
        }
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
            AbilityConfig characterAbility = globalValues.abilities[i];

            //if the ability we are checking matches either the ability of the character, or the pre-augmented version of it.
            if (ability.abilityName == characterAbility.abilityName)
            {
                return AbilityUpgradeStatus.Unlocked;
            }
            else if (characterAbility.augmentedAbility.abilityName == ability.abilityName)
            {
                return AbilityUpgradeStatus.Augmented;
            }

        }
        return AbilityUpgradeStatus.Available;
    }




    public void FinalizeCharacter()
    {

    }

    public void SelectAbility(AbilityUnlock abilityUnlock)
    {
        abilityDescription.ClearAbilityDisplay();
        abilityDescription.SetAbilityDisplay(abilityUnlock);
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
            int index = -1;
            for (int i = 0; i < globalValues.abilities.Count; i++)
            {
                if (globalValues.abilities[i].name == abilityConfig.name)
                {
                    index = i;
                    break;
                }
            }
            if (index != -1)
            {
                c.abilityIndices.Add(index);
            }
        }
    }
}


