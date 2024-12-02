using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AugmentButton : MonoBehaviour
{
    public AbilityAugment augment;
    public AbilityDescription description;
    public Image icon;

    public void OnClick()
    {
        description.SetAugmentDescription(augment);
    }

}
