using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Floor Tile", menuName = "PlacableObjects/Floor Tile")]
public class SO_FloorMaterial : ScriptableObject
{
    [SerializeField]
    Material floorMat;
    [SerializeField]
    float cost;

    public Material FloorMat { get => floorMat; set => floorMat = value; }
    public float Cost { get => cost; set => cost = value; }

    public void SetMaterial()
    {
        MaterialManager.MyInstance.ActiveMaterial = FloorMat;
    }
}
