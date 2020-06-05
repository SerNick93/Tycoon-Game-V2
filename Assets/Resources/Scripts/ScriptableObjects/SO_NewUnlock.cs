using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Unlocks", menuName="ResearchTrees/New Unlocks")]
public class SO_NewUnlock : ScriptableObject
{
    public Image thumbnail;
    public string unlockName;
    public string description;
    public float researchTime;
    public SO_NewUnlock[] unlockObject;
    public SO_NewUnlock[] prerequisits;
    public bool isUnlocked;
    public int researchCost;
}
