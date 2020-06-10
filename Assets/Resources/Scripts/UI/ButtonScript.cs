using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonScript : MonoBehaviour, IPointerClickHandler
{ 
    public enum placmentTypeEnum { FloorMaterial, Object, Zoning };
    [SerializeField]
    private placmentTypeEnum placementType;
    public placmentTypeEnum PlacementType { get => placementType; set => placementType = value; }
    public GameObject ModelPrefab { get => modelPrefab; set => modelPrefab = value; }

    [SerializeField]
    Material materialToBePlaced;
    [SerializeField]
    GameObject modelPrefab;
    [SerializeField]
    SO_Zones zone;
    Button button;


    // Start is called before the first frame update
    void Start()
    {
        button = GetComponent<Button>();
        
    }



    public void OnPointerClick(PointerEventData eventData)
    {
        int i = (int)placementType;


        if (eventData.button == PointerEventData.InputButton.Left )
        {
            switch (i)
            {
                case 0:
                    MaterialManager.MyInstance.ActiveMaterial = materialToBePlaced;
                    break;
                case 1:
                    GameManager.MyInstance.ActivateModel(ModelPrefab);
                    break;
                case 2:
                    if (ZoningController.MyInstance.ActiveZone == null)
                    {
                        ZoningController.MyInstance.ActiveZone = zone;
                    }
                    break;
                default:
                    break;
            }

        }
    }
}
