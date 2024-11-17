using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Character Class")]

public class CharacterClass : ScriptableObject
{
    //BASE STATS
    public int strength = 10;
    public int dexterity = 10;
    public int mind = 10;
    public int endurance = 10;
    public int resolve = 10;
    public int maxHealth = 10;


    public float moveDistance = 25;
    public int alacrity = 0;
    public float visionRadius = 20;

    public int protectionScore = 10;

}
