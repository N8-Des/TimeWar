using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisionNode : MonoBehaviour
{
    public enum AllyVisionStatus
    { 
        Unseen, 
        Explored,
        Seen
    }

    public AllyVisionStatus currentNodeStatus;
}
