using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Zone", menuName= "PlacableObjects/Zone")]
public class SO_Zones : ScriptableObject
{
    [SerializeField]
    string zoneName;
    [SerializeField]
    Material zoneOverlay;
    [SerializeField]
    float cost;

    public Material ZoneOverlay { get => zoneOverlay; set => zoneOverlay = value; }
    public float Cost { get => cost; set => cost = value; }
}
