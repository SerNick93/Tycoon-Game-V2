using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialManager : MonoBehaviour
{
    public static MaterialManager myInstance;
    public static MaterialManager MyInstance
    {
        get
        {
            if (myInstance == null)
            {
                myInstance = FindObjectOfType<MaterialManager>();
            }
            return myInstance;
        }
    }

    public Material RedMat { get => redMat; set => redMat = value; }
    public Material GreenMat { get => greenMat; set => greenMat = value; }
    public Material StandardMat { get => standardMat; set => standardMat = value; }
    public Transform ObjectWorldHolder { get => objectWorldHolder; set => objectWorldHolder = value; }
    public Material ActiveMaterial { get => activeMaterial; set => activeMaterial = value; }

    [SerializeField] Material redMat, greenMat, standardMat;
    [SerializeField] Transform objectWorldHolder;
    [SerializeField] Material activeMaterial;

    private void Awake()
    {
        ActiveMaterial = null;
    }
    public void LateUpdate()
    {
        if (!ActiveMaterial && Input.GetMouseButtonUp(1))
        {
            ActiveMaterial = null;
        }
    }
}
