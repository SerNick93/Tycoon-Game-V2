using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_SubObject_Button : MonoBehaviour, IPointerClickHandler
{
    GameObject go;

    public GameObject Go { get => go; set => go = value; }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (GameManager.ActiveModels.Count == 0)
            {
                GameManager.MyInstance.ActivateModel(go);

            }

        }
    }
}
