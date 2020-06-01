using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIController : MonoBehaviour
{
    public static UIController myInstance;
    public static UIController MyInstance
    {
        get
        {
            if (myInstance == null)
            {
                myInstance = FindObjectOfType<UIController>();
            }
            return myInstance;
        }
    }

    [Header("Currency Text Ui Objects")]
    [SerializeField]
    TextMeshProUGUI moneyUIObject;
    [SerializeField]
    TextMeshProUGUI rpUIObject;
    [SerializeField]
    CanvasGroup parkManagment;

    public TextMeshProUGUI MoneyUIObject { get => moneyUIObject; set => moneyUIObject = value; }
    public TextMeshProUGUI RpUIObject { get => rpUIObject; set => rpUIObject = value; }

    public void UpdateMoneyUIObject(float totalMoney)
    {
        string totalMoneyText = null;
        totalMoneyText = string.Format("{0}{1}", "$", totalMoney.ToString());
        MoneyUIObject.text = totalMoneyText;
    }

    public void TurnOnUI()
    {
        if (parkManagment.blocksRaycasts == false)
        {
            parkManagment.alpha = 1;
            parkManagment.blocksRaycasts = true;
        }
        else
        {
            parkManagment.alpha = 0;
            parkManagment.blocksRaycasts = false;
        }
    }

}
