using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CharacterCreator : MonoBehaviour
{
    public GameObject classSelectorPanel;
    public List<UpgradeTree> classTrees = new List<UpgradeTree>();
    GlobalValues globalValues;
    public TextMeshProUGUI className;
    int classSelected;
    
    void Start()
    {
        globalValues = FindObjectOfType<GlobalValues>();
        for(int i = 0; i < globalValues.classes.Count; i++)
        {
            GameObject go = Resources.Load<GameObject>("ClassButton");
            go = Instantiate(go);
            go.transform.parent = classSelectorPanel.transform;
            ClassSelector classButton = go.GetComponent<ClassSelector>();
            classButton.classIndex = i;
            classButton.className.text = globalValues.classes[i].className;
            classButton.charCreator = this;
        }
    }

    public void SelectClass(int classIndex)
    {
        classSelected = classIndex;
        className.text = globalValues.classes[classIndex].className;
    }

    public void ConfirmClass()
    {
        classTrees[classSelected].gameObject.SetActive(true);
        classSelectorPanel.SetActive(false);
    }
}
