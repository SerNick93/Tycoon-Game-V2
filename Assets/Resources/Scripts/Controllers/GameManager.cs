using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Nick.controllers;


public class GameManager : MonoBehaviour
{
    public static GameManager myInstance;
    public static GameManager MyInstance
    {
        get
        {
            if (myInstance == null)
            {
                myInstance = FindObjectOfType<GameManager>();
            }
            return myInstance;
        }
    }

    public GameObject ActiveModel { get => activeModel; set => activeModel = value; }
    public Vector3 MousePoisition { get => mousePoisition; set => mousePoisition = value; }
    public Grid Grid { get => grid; set => grid = value; }
    public float TotalMoney { get => totalMoney; set => totalMoney = value; }

    [SerializeField]
    GameObject activeModel = null;
    PlaceObjectInWorld placeObject;
    Vector3 mousePoisition;
    [SerializeField]
    Grid grid;
    float totalMoney;
    int radPoints;
     


    public void Awake()
    {
        TotalMoney = 100;
        UIController.MyInstance.UpdateMoneyUIObject(TotalMoney);
    }
    public void ActivateModel(GameObject go)
    {

        placeObject = go.GetComponent<PlaceObjectInWorld>();
        activeModel = go;

        placeObject.InitScripts();
    }
    public void FixedUpdate()
    {
        MousePoisition = CustomControllers.GetWorldPositionOnPlane(Input.mousePosition, 0f);
    }

    public void IncreaseMoney(float profit)
    {
        TotalMoney += profit;
        UIController.MyInstance.UpdateMoneyUIObject(TotalMoney);

    }

    public void ReduceMoney(float cost)
    {
        TotalMoney -= cost;
        UIController.MyInstance.UpdateMoneyUIObject(TotalMoney);
    }
    public void UpdateRadPointsUI()
    {

    }


}


