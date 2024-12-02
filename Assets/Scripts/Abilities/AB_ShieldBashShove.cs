using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GlobalEnums;

public class AB_ShieldBashShove : AbilityBase
{
    public override void Execute(Character caster, Character target, Vector3 targetLocation)
    {
        base.Execute(caster, target, targetLocation);
        target.GetPushed(config.shoveDistance);
    }
}
