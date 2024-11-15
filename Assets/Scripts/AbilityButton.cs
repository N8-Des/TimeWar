using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AbilityButton : MonoBehaviour
{
    public TextMeshProUGUI abilityName;
    public Ability ability;
    public Character character;
    BattleManager battleManager;
    public int buttonIndex;

    private void Start()
    {
        battleManager = FindObjectOfType<BattleManager>();
    }

    public void SetAbility(Ability a, Character c)
    {
        abilityName.text = a.name;
        ability = a;
        character = c;
    }


    public void OnSelect()
    {
        battleManager.SelectAbility(buttonIndex);
    }
}
