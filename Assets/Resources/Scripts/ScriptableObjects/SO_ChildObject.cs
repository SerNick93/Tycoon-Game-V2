using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Child Object", menuName ="PlacableObjects/Child Object")]
public class SO_ChildObject : ScriptableObject
{
    public int cost;
    public SO_NewUnlock rootObject;
}
