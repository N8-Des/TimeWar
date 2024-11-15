using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityDisplay : MonoBehaviour
{
    public List <AbilityButton> abilityButtons = new List <AbilityButton> ();

    private void Start()
    {
        for(int i = 0; i < abilityButtons.Count; i++)
        {
            abilityButtons[i].buttonIndex = i;
        }
    }
}
