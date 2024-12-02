using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class GlobalValues : MonoBehaviour
{
    public List<CharacterClass> classes = new List<CharacterClass>();
    public List<AbilityConfig> abilities = new List<AbilityConfig>();


    void Start()
    {
        DontDestroyOnLoad(this);
    }

    public int GetAbilityLevelCap(int level)
    {
        if (level <= 0) return 0;
        if (level == 1 || level == 2) return 1;
        if (level >= 3 && level <= 5) return 2;
        if (level >= 6 && level <= 9) return 3;
        if (level >= 10 && level <= 14) return 4;
        return 5;
    }
}
