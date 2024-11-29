using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharacterSaveLoadManager : MonoBehaviour
{
    public GameObject defaultPlayerPrefab;
    GlobalValues globalValues;

    [System.Serializable]
    public class CharacterBattleData 
    {
        public List<CharacterStats> characterList;
    }

    private void Awake()
    {
        DontDestroyOnLoad(this);
        globalValues = GetComponent<GlobalValues>();
    }
    public void SaveCharacterData(List<CharacterStats> characters)
    {
        CharacterBattleData characterBattleData = new CharacterBattleData();
        characterBattleData.characterList = characters;
        string json = JsonUtility.ToJson(characterBattleData);
        string path = Path.Combine(Application.persistentDataPath, "characterBattleData.json");

        File.WriteAllText(path, json);
    }

    public void LoadDataIntoBattle(Vector3 startPosition, BattleManager battleManager)
    {
        string path = Path.Combine(Application.persistentDataPath, "characterBattleData.json");

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            CharacterBattleData characters = JsonUtility.FromJson<CharacterBattleData>(json);
            battleManager.characters.Clear();
            foreach (CharacterStats stats in characters.characterList)
            {
                //spawn character gameobject
                GameObject go = GameObject.Instantiate(defaultPlayerPrefab);
                go.transform.position = startPosition + new Vector3(Random.Range(-5, 5), 0, Random.Range(-5, 5));
                Character defaultCharacter = go.GetComponent<Character>();
                defaultCharacter.SetStats(stats);
                battleManager.characters.Add(defaultCharacter);


                //setup abilities
                List<AbilityConfig> abilities = new List<AbilityConfig>();
                foreach (int index in stats.abilityIndices)
                {
                    abilities.Add(globalValues.abilities[index]);
                }
                defaultCharacter.GetComponent<AbilityManager>().InitializeAbilities(abilities);
            }
        }

        battleManager.StartBattle();
    }
}
