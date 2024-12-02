using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterGenerator : MonoBehaviour
{
    public List<CharacterStats> characterStats = new List<CharacterStats>();
    public List<StatBlock> statBlocks = new List<StatBlock>();
    GlobalValues globalValues;

    private void Start()
    {
        globalValues = FindObjectOfType<GlobalValues>();
        GenerateCharacters();
        FindObjectOfType<CharacterSaveLoadManager>().SaveCharacterData(characterStats);
        SceneManager.LoadScene("TestScene");
    }

    void GenerateCharacters()
    {
        for(int i = 0; i < 4; i++)
        {
            CharacterStats cStats = new()
            {
                classIndex = 0
            };
            GenerateStats(cStats);

            statBlocks[i].SetStatDisplay(cStats);
            characterStats.Add(cStats);
            cStats.CreateStatList();
        }
    }


    void GenerateStats(CharacterStats c)
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
