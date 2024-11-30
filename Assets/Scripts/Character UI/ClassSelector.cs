using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ClassSelector : MonoBehaviour
{
    public CharacterCreator charCreator;
    public int classIndex;
    public TextMeshProUGUI className;

    public void Click()
    {
        charCreator.SelectClass(classIndex);
    }
}
