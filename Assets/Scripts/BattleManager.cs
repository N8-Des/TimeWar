using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public Gradient pathValidGradient;
    public Color pathInvalidColor;

    public LayerMask groundLayerMask;
    public LayerMask abilityLayerMask;

    public Material lineMat;
    public Material abilityLineMat;
    public Material radiusMat;

    public AbilityDisplay abilityDisplay;

    public List<Character> characters = new List<Character>();
    List<(Character, int)> initiative = new();
    public int currentTurn;
    public int currentRound;

    public Transform allyStartPosition;


    Character currentCharacter;
    AllyCharacterMovement characterMovement;

    private void Start()
    {
        FindObjectOfType<CharacterSaveLoadManager>().LoadDataIntoBattle(allyStartPosition.position, this);
        StartBattle();
    }

    public void StartBattle()
    {
        initiative.Clear();
        foreach (Character character in characters)
        {
            int alacrity = Random.Range(1, 20) + character.alacrity;
            (Character, int) initiativeTuple = (character, alacrity);
            initiative.Add(initiativeTuple);
        }
        initiative.Sort((x, y) => y.Item2.CompareTo(x.Item2));
        currentRound = 1;
        currentTurn = -1;
        NextTurn();
    }

    
    public void NextTurn()
    {
        currentTurn++;
        SetCurrentCharacter(initiative[currentTurn].Item1);
    }

    public void SelectAbility(int index)
    {
        characterMovement.StartAbilitySelection(index);
    }


    public void SetCurrentCharacter(Character character)
    {
        currentCharacter = character;
        characterMovement = character.GetComponent<AllyCharacterMovement>();
        for (int i = 0; i < character.abilities.Count; i++)
        {
            abilityDisplay.abilityButtons[i].SetAbility(character.abilities[i], character);
        }
        for (int i = abilityDisplay.abilityButtons.Count - 1; i >= character.abilities.Count; i--)
        {
            abilityDisplay.abilityButtons[i].enabled = false;
        }
        currentCharacter.StartRound();
    }
}
