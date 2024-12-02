using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AB_AuraOfProtection : AbilityBase
{
    public override void Initialize(Character c)
    {
        base.Initialize(c);

        c.eventSystem.OnMove += OnMove;
        
    }


    public void OnMove(Vector3 start, Vector3 end)
    {
        print("does this work?");
    }

    private void OnDisable()
    {
        character.eventSystem.OnMove -= OnMove;
    }

}
