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

    public static List<GameObject> ActiveModels { get => activeModels; set => activeModels = value; }
    public Vector3 MousePoisition { get => mousePoisition; set => mousePoisition = value; }
    public Grid Grid { get => grid; set => grid = value; }
    public float TotalMoney { get => totalMoney; set => totalMoney = value; }

    [SerializeField]
    public static List<GameObject> activeModels = new List<GameObject>();
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
        ActiveModels.Add(go);
        placeObject = go.GetComponent<PlaceObjectInWorld>();

        placeObject.InitScripts();
    }
    public void FixedUpdate()
    {
        MousePoisition = CustomControllers.GetWorldPositionOnPlane(Input.mousePosition, 0f);
        if (Input.GetKeyDown(KeyCode.L))
        {
            Debug.Log(ActiveModels.Count);
            foreach (GameObject item in ActiveModels)
            {
                Debug.Log(item.name);
            }
        }
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


