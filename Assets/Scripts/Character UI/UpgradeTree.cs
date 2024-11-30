using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeTree : MonoBehaviour
{
    public List<UpgradeColumn> columns = new();
    public List<GameObject> abilityTiers = new();
    public CharacterStats characterStats;
    public int classIndex;
    GlobalValues globalValues;
    AbiliityDescription currentAbilityDescription;
    AbilityUnlockButton currentAbilityButton;

    private void Awake()
    {
        globalValues = FindObjectOfType<GlobalValues>();
        for (int i = 0; i < columns.Count; i++)
        {
            for (int j = 0; j < columns[i].upgradesInColumn.Count; j++)
            {
                GameObject abilityButton = Resources.Load<GameObject>("AbilityUnlockButton");
                abilityButton = Instantiate(abilityButton);
            }
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

    public bool CharacterHasAbilityOrAugment(AbilityConfig ability)
    {
        foreach(int i in characterStats.abilityIndices)
        {
            //the ability on the character already
            AbilityConfig characterAbility = globalValues.abilities[i];

            //if the ability we are checking matches either the ability of the character, or the pre-augmented version of it.
            if (ability.abilityName == characterAbility.abilityName || 
                characterAbility.augmentedAbility.abilityName == ability.abilityName)
            {
                return true;
            }
        }
        return false;
    }




    public void FinalizeCharacter()
    {

    }

    public void SelectAbility(AbilityUnlockButton button)
    {
        if (currentAbilityButton != null)
        {
            
        }
        currentAbilityButton = button;


    }

    public void HoverAbility(AbilityUnlockButton button)
    {

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


