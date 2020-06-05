using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FindModelInWorld : MonoBehaviour
{
    public static FindModelInWorld myInstance;
    public static FindModelInWorld MyInstance
    {
        get
        {
            if (myInstance == null)
            {
                myInstance = FindObjectOfType<FindModelInWorld>();
            }
            return myInstance;
        }
    }


    EditModel editModel;
    PlaceObjectInWorld placeObject;
    [SerializeField]
    TransformControls tc;


    // Update is called once per frame
    void FixedUpdate()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        //Find objects in the world
        if (Input.GetMouseButtonUp(0))
        {
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                if (Physics.Raycast(ray, out hit))
                {
                    var heading = hit.point - ray.origin;
                    var distance = heading.magnitude;
                    var direction = heading / distance;

                    //Debug.DrawRay(ray.origin, direction * distance, Color.green);

                    editModel = hit.transform.GetComponent<EditModel>();
                    placeObject = hit.transform.GetComponent<PlaceObjectInWorld>();

                    if (editModel && !placeObject.enabled && 
                        !GameManager.MyInstance.ActiveModel)
                    {
                        GameManager.MyInstance.ActiveModel = hit.transform.gameObject;
                        Debug.Log("Enabling Transform");
                        editModel.EnableTransformTools();
                    }
                }
            }
        }
        //Replace materials on the floor tiles
        if (Input.GetMouseButton(0))
        {
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                if (Physics.Raycast(ray, out hit))
                {
                    var heading = hit.point - ray.origin;
                    var distance = heading.magnitude;
                    var direction = heading / distance;

                    Debug.DrawRay(ray.origin, direction * distance, Color.red);

                    MeshRenderer mr = hit.transform.GetComponent<MeshRenderer>();
                    FloorData fd = hit.transform.GetComponent<FloorData>();

                    if (hit.transform.gameObject.layer == 9 && !GameManager.MyInstance.ActiveModel)
                    {
                        if (MaterialManager.MyInstance.ActiveMaterial)
                        {
                            mr.materials[0] = MaterialManager.MyInstance.ActiveMaterial;
                        }
                        if (ZoningController.MyInstance.ActiveZone)
                        {
                            fd.Zone = ZoningController.MyInstance.ActiveZone;
                        }
                    }

                }
            }
        }
    }
}
