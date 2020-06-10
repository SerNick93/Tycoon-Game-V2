using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
public class UI_SubObject : MonoBehaviour
{
    public static UI_SubObject myInstance;
    public static UI_SubObject MyInstance
    {
        get
        {
            if (myInstance == null)
            {
                myInstance = FindObjectOfType<UI_SubObject>();
            }
            return myInstance;
        }
    }

    public Button btnPrefab;
    public CanvasGroup cg;
    GameObject childModel;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PopulateSubObjects(GameObject modelPrefab)
    {
        if (cg.alpha == 0)
        {
            cg.alpha = 1;
            cg.blocksRaycasts = true;
        }
        else
        {
            cg.alpha = 0;
            cg.blocksRaycasts = false;

        }

        foreach (GameObject go in modelPrefab.GetComponent<RootObject>().ChildObjects)
        {
            Button btn = Instantiate(btnPrefab, transform);
            btn.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = go.name;
            btn.GetComponent<UI_SubObject_Button>().Go = go;
        }
    }

}
