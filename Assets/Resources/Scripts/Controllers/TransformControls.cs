using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Nick.controllers;


public class TransformControls : MonoBehaviour
{
    public static TransformControls myInstance;
    public static TransformControls MyInstance
    {
        get
        {
            if (myInstance == null)
            {
                myInstance = FindObjectOfType<TransformControls>();
            }
            return myInstance;
        }
    }

    public Toggle SnapToGridToggle { get => snapToGridToggle; set => snapToGridToggle = value; }
    public Toggle ZoneOverlay { get => zoneOverlay; set => zoneOverlay = value; }

    Toggle snapToGridToggle, zoneOverlay;
    CanvasGroup cg;
    GameManager gm;

    // Start is called before the first frame update
    void Start()
    {
        SnapToGridToggle = GetComponentInChildren<Toggle>();
        cg = GetComponent<CanvasGroup>();
        gm = GameManager.MyInstance;

    }

    // Update is called once per frame
    public void Update()
    {
        //if (gm.ActiveModel)
        //{
        //    cg.alpha = 1;
        //    cg.blocksRaycasts = true;
        //}
        //else
        //{
        //    cg.alpha = 0;
        //    cg.blocksRaycasts = false;
        //}

    }

    public void DestroyActiveObjects()
    {
        if (gm.ActiveModel)
        {
            CustomControllers.DestroyGameObject(gm.ActiveModel);
            
        }
    }

}
