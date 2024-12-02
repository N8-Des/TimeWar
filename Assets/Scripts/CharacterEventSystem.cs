using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterEventSystem 
{
    public event Action<int, Character> OnDamageTaken;
    public event Action<Vector3, Vector3> OnMove;
    public event Action<int, Character> OnDealDamage;


    public void TriggerDamageTaken(int damage, Character perpetrator) => OnDamageTaken?.Invoke(damage, perpetrator);
    public void TriggerMove(Vector3 start, Vector3 end) => OnMove?.Invoke(start, end);
    public void TriggerDamageDealt(int damage, Character victim) => OnDealDamage?.Invoke(damage, victim);
}
