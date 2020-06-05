using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorData : MonoBehaviour
{
    BoxCollider bc;
    MeshRenderer mr;
    [SerializeField]
    SO_Zones zone;

    public SO_Zones Zone { get => zone; set => zone = value; }

    private void Awake()
    {
        bc = GetComponent<BoxCollider>();
        mr = GetComponent<MeshRenderer>();
    }
    //Remove floor for undergroundItem
    public void DisableThis()
    {
        //bc.enabled = false;
        mr.enabled = false;
    }

    public void EnableThis()
    {
        mr.enabled = true;
    }
    public void Update()
    {

    }
}
