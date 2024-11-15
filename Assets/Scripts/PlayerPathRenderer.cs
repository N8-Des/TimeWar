using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPathRenderer : MonoBehaviour
{
    public LineRenderer pathDisplay;
    public LineRenderer extraPathDisplay;

    public Material bluePath;
    public Material redPath;

    public void SetPathMaterial(bool isBlue)
    {
        pathDisplay.material = isBlue? bluePath : redPath;
    }
}
