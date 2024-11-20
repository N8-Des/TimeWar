using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StatBlock : MonoBehaviour
{
    public TextMeshProUGUI power;
    public TextMeshProUGUI fortitude;
    public TextMeshProUGUI mind;

    public TextMeshProUGUI moveSpeed;
    public TextMeshProUGUI alacrity;

    public TextMeshProUGUI protection;
    public TextMeshProUGUI dodge;


    public void SetStatDisplay(Character c)
    {
        power.text = "Power: " + c.power;
        fortitude.text = "Fortitude: " + c.fortitude;
        mind.text = "Mind: " + c.mind;
        moveSpeed.text = "Movement: " + c.moveDistance;
        alacrity.text = "Alacrity: " + c.alacrity;
        protection.text = "Protection: " + c.protection;
        dodge.text = "Dodge: " + c.dodge;
    }
}
