using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UnlockArrayForEditorWindow : MonoBehaviour
{
    [SerializeField]
    private GameObject[] unlockObjects;

    public GameObject[] UnlockObjects { get => unlockObjects; set => unlockObjects = value; }
}
