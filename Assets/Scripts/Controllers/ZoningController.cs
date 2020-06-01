using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoningController : MonoBehaviour
{
    public static ZoningController myInstance;
    public static ZoningController MyInstance
    {
        get
        {
            if (myInstance == null)
            {
                myInstance = FindObjectOfType<ZoningController>();
            }
            return myInstance;
        }
    }


    // Start is called before the first frame update
    [SerializeField]
    SO_Zones activeZone;

    public SO_Zones ActiveZone { get => activeZone; set => activeZone = value; }

    private void Awake()
    {
        ActiveZone = null;
    }
    public void LateUpdate()
    {
        if (!ActiveZone && Input.GetMouseButtonUp(1))
        {
            ActiveZone = null;
        }
    }

}
