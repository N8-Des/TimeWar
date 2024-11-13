using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AbilityButton : MonoBehaviour
{
    public TextMeshProUGUI abilityName;
    public Ability ability;
    public Character character;

    public void SetAbility(Ability a, Character c)
    {
        abilityName.text = a.name;
        ability = a;
        character = c;
    }

    public void OnSelect()
    {

    }
}
