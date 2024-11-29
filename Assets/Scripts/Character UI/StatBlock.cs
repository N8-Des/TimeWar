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


    public void SetStatDisplay(CharacterStats c)
    {
        power.text = "Power: " + c.Power.GetValue();
        fortitude.text = "Fortitude: " + c.Fortitude.GetValue();
        mind.text = "Mind: " + c.Mind.GetValue();
        moveSpeed.text = "Movement: " + c.Movement.GetValue();
        alacrity.text = "Alacrity: " + c.Alacrity.GetValue();
        protection.text = "Protection: " + c.Protection.GetValue();
        dodge.text = "Dodge: " + c.Dodge.GetValue();
    }
}
