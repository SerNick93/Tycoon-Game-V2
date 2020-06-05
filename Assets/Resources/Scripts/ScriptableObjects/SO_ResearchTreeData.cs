using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Research Tree", menuName = "ResearchTrees/New Research Tree")]
public class SO_ResearchTreeData : ScriptableObject
{
    public string treeName;
    public List<SO_NewUnlock> numberOfUnlocksInThisTree;
}
