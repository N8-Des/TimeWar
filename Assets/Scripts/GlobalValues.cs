using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalValues : MonoBehaviour
{
    public List<CharacterClass> classes = new List<CharacterClass>();
    public List<AbilityConfig> abilities = new List<AbilityConfig>();


    void Start()
    {
        DontDestroyOnLoad(this);
    }

}
