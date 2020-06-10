using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootObject : MonoBehaviour
{
    public List<GameObject> childObjects;

    public List<GameObject> ChildObjects { get => childObjects; set => childObjects = value; }
}
