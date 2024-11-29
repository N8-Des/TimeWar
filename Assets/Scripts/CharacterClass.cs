using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Character Class")]

[System.Serializable]
public class CharacterClass : ScriptableObject
{
    //BASE STATS
    public int power = 10;
    public int fortitude = 10;
    public int mind = 10;
    public int maxHealth = 10;

    public float moveDistance = 25;
    public int alacrity = 0;
    public float visionRadius = 20;

    public int dodgeScore = 10;
    public int willpowerScore = 10; 
    
    public List<AbilityConfig> defaultAbilities = new();
}
