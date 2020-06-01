using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;



public class PopulateResearchPanel : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private Image researchThumbnail, panelImage;
    [SerializeField]
    private TextMeshProUGUI researchName;
    [SerializeField]
    private TextMeshProUGUI researchCost;
    [SerializeField]
    private TextMeshProUGUI researchTime;

    bool isUnlocked = true;
    SO_NewUnlock newUnlock;

    public void PopulatePanel(SO_NewUnlock newUnlock)
    {
        this.newUnlock = newUnlock;
        //researchThumbnail.sprite = newUnlock.thumbnail;
        researchName.text = newUnlock.unlockName;
        researchCost.text = newUnlock.researchCost.ToString();
        researchTime.text = newUnlock.researchTime.ToString();

        if (!newUnlock.isUnlocked)
        {
            panelImage.color = Color.gray;
            isUnlocked = false;
        }
    }

    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left && isUnlocked)
        {
            Debug.Log(newUnlock.unlockName);
            panelImage.color = Color.green;
        }
    }
}
