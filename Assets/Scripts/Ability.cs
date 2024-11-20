using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using static GlobalEnums;

[System.Serializable]
[CreateAssetMenu(menuName = "Abilities/StandardAbility")]
public class Ability : ScriptableObject
{
    public bool isPassive = false;
    public string abilityName;
    public string abilityDescription;
    public int cost;
    public int toHitModifier;
    public int damageModifier;
    public Sprite icon;
    public List<DamageRoll> damageRolls = new List<DamageRoll>();
    public ActionType actionType;
    public float attackRange;

    DecalProjector projector;
    LineRenderer abilityLine;

    public bool stunning = false;

    //default ability: make attack roll and deal damage to target
    public virtual void Execute(Character caster, Character target, Vector3 targetLocation)
    {
        int toHit = Mathf.CeilToInt(caster.power / 2) + Random.Range(1, 20);
        toHit += toHitModifier;
        if(toHit >= target.dodge)
        {
            foreach(DamageRoll dRoll in damageRolls)
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
            projector.size = new(attackRange * 2, attackRange * 2, 30);
            
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
        else if (attackRange > 5)
        {
            if (Physics.Raycast(start, direction, attackRange, FindObjectOfType<BattleManager>().groundLayerMask))
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
