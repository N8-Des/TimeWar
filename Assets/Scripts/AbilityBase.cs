using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GlobalEnums;
using UnityEngine.Rendering.Universal;

public class AbilityBase : MonoBehaviour
{

    public AbilityConfig config;

    DecalProjector projector;
    LineRenderer abilityLine;
    public AbilityBase(AbilityConfig config)
    {
        this.config = config;
    }
    public AbilityBase()
    { }

    public virtual void Execute(Character caster, Character target, Vector3 targetLocation)
    {
        int toHit = Mathf.CeilToInt(caster.stats.Power.GetValue() / 2) + Random.Range(1, 20);
        toHit += config.toHitModifier;
        if (toHit >= target.stats.Dodge.GetValue())
        {
            foreach (DamageRoll dRoll in config.damageRolls)
            {
                int damage = dRoll.Roll();
                target.TakeDamage(damage, caster, dRoll.damageType);
            }
        }
    }

    public virtual void DisplaySpell(Vector3 startPosition, Vector3 mousePosition, Character caster)
    {
        if (abilityLine == null)
        {
            GameObject lineObject = new GameObject();
            abilityLine = lineObject.AddComponent<LineRenderer>();
            abilityLine.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            abilityLine.material = FindObjectOfType<BattleManager>().abilityLineMat;
            abilityLine.textureMode = LineTextureMode.Tile;
            abilityLine.widthMultiplier = 1.4f;

        }
        if (projector == null)
        {
            GameObject projectorObject = new GameObject();
            projector = projectorObject.AddComponent<DecalProjector>();
            projectorObject.transform.position = caster.transform.position + (Vector3.up * 10);
            projectorObject.transform.rotation = Quaternion.Euler(90, 0, 0);
            projector.material = FindObjectOfType<BattleManager>().radiusMat;
            projector.size = new(config.attackRange * 2, config.attackRange * 2, 30);

        }


        abilityLine.SetPosition(0, startPosition + Vector3.up);
        abilityLine.SetPosition(1, mousePosition + Vector3.up);
    }

    public virtual bool IsValidSpell(Vector3 start, Vector3 direction, Character target)
    {
        if (target == null)
        {
            return false;
        }
        else if (config.attackRange > 3)
        {
            if (Physics.Raycast(start, direction, config.attackRange, FindObjectOfType<BattleManager>().groundLayerMask))
            {
                return false;
            }
        }
        return true;
    }

    public virtual void EndDisplay()
    {
        if (abilityLine != null)
        {
            Destroy(abilityLine.gameObject);
        }
        if (projector != null)
        {
            Destroy(projector.gameObject);
        }
    }
}
