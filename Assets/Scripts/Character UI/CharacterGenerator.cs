using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterGenerator : MonoBehaviour
{
    public List<Character> characters = new List<Character>();
    public List <CharacterClass> classes = new List<CharacterClass>();
    public List<StatBlock> statBlocks = new List<StatBlock>();

    private void Start()
    {
        GenerateCharacters();
        FindObjectOfType<CharacterSaveLoadManager>().SaveCharacterData(characters);
        SceneManager.LoadScene("TestScene");
    }

    void GenerateCharacters()
    {
        for(int i = 0; i < 4; i++)
        {
            Character newChar = new Character();
            newChar.myClass = classes[0];
            GenerateStats(newChar);

            statBlocks[i].SetStatDisplay(newChar);
            characters.Add(newChar);
        }
    }


    void GenerateStats(Character c)
    {
        c.maxHealth = c.myClass.maxHealth + Random.Range(-2, 3);
        c.power = c.myClass.power + Random.Range(-2, 3);
        c.fortitude = c.myClass.fortitude + Random.Range(-2, 3);
        c.mind = c.myClass.mind + Random.Range(-2, 3);
        c.protection = c.myClass.protectionScore;
        c.moveDistance = c.myClass.moveDistance;
        c.alacrity = c.myClass.alacrity;
        c.dodge = c.myClass.dodgeScore;
    }
}
