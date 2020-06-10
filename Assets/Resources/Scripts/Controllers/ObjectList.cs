using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ObjectList : MonoBehaviour
{
    [SerializeField]
    public List<GameObject> objectList;
    public Button btnPrefab;
    public Transform buttonParent;

    // Start is called before the first frame update
    void Awake()
    {
        foreach (GameObject obj in objectList)
        {
            Button btn = Instantiate(btnPrefab, buttonParent);
            ButtonScript btnScript = btn.GetComponent<ButtonScript>();
            btnScript.PlacementType = ButtonScript.placmentTypeEnum.Object;
            btnScript.ModelPrefab = obj;
            //This can be changed to an image later down the line
            btnScript.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = obj.GetComponent<PlaceObjectInWorld>().PlaceableObject.unlockName;
        }   
    }

}
