using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CharacterGenerator : MonoBehaviour
{
    public List<Character> characters = new List<Character>();
    public List <CharacterClass> classes = new List<CharacterClass>();

    private void Start()
    {
        GenerateCharacters();
    }

    void GenerateCharacters()
    {
        for(int i = 0; i < 4; i++)
        {
            Character newChar = new Character();
            newChar.myClass = classes[0];
            GenerateStats(newChar);



        }
    }


    void GenerateStats(Character c)
    {
        //main stats
        c.strength = c.myClass.strength + Random.Range(-2, 3);
        c.dexterity = c.myClass.dexterity + Random.Range(-2, 3);
        c.mind = c.myClass.mind + Random.Range(-2, 3);
        c.endurance = c.myClass.endurance + Random.Range(-2, 3);
        c.resolve = c.myClass.resolve + Random.Range(-2, 3);
        c.maxHealth = c.myClass.maxHealth + Random.Range(-3, 4);
        c.moveDistance = c.myClass.moveDistance;
        c.alacrity = c.myClass.alacrity + c.dexterity - 10;
        c.visionRadius = c.myClass.visionRadius;
        c.protectionScore = c.myClass.protectionScore;
        c.dodgeScore = c.dexterity + 4;
        c.fortitudeScore = c.endurance + 4;
        c.willpowerScore = c.resolve + 4;

    }
}
