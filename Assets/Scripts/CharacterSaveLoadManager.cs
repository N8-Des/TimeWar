using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharacterSaveLoadManager : MonoBehaviour
{
    public GameObject defaultPlayerPrefab;
    [System.Serializable]
    public class CharacterBattleData 
    {
        public List<Character> characterList;
    }

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }
    public void SaveCharacterData(List<Character> characters)
    {
        CharacterBattleData characterBattleData = new CharacterBattleData();
        characterBattleData.characterList = characters;
        print(characterBattleData.characterList.Count);
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
            print(json);
            battleManager.characters = characters.characterList;
            foreach(Character character in characters.characterList)
            {
                GameObject go = GameObject.Instantiate(defaultPlayerPrefab);
                go.transform.position = startPosition + new Vector3(Random.Range(-5, 5), 0, Random.Range(-5, 5));
                Character defaultCharacter = go.GetComponent<Character>();
                defaultCharacter = character;
            }
        }
    }
}
