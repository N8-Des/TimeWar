using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterLevelSet : MonoBehaviour
{
    [HideInInspector] public UpgradeTree uptree;
    public TextMeshProUGUI levelDisplay;
    public Slider slider;

    public void SetLevel()
    {
        int level = (int)slider.value;
        levelDisplay.text = "LEVEL " + slider.value;
        uptree.SetCharacterLevel(level);
    }

}
