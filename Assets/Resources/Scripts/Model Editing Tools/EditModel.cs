using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Nick.controllers;
public class EditModel : MonoBehaviour
{ 
    PlaceObjectInWorld placeObjectinWorld;
    bool rotation = false;
    float rotationSpeed = 8f;


    public void OnEnable()
    {
        placeObjectinWorld = transform.GetComponent<PlaceObjectInWorld>();
    }

    public void EnableTransformTools()
    {
        if (!enabled)
        {
            //Enable placment tools
            GameObject TChild = transform.GetChild(0).gameObject;
            //if the nodes are not active, activate them
            for (int i = 0; i < TChild.transform.childCount; i++)
            {
                if (!TChild.transform.GetChild(i).gameObject.activeSelf)
                {
                    TChild.transform.GetChild(i).gameObject.SetActive(true);
                }
            }

            enabled = true;
            placeObjectinWorld.ObjectMeshRenderer.material = MaterialManager.MyInstance.GreenMat;          
        }
    }
    public void LateUpdate()
    {
        float h =  Input.GetAxis("Horizontal") * Time.deltaTime * 2.0f;
        float v =  Input.GetAxis("Vertical") * Time.deltaTime * 2.0f;

        if (GameManager.ActiveModels != null)
        {
                //Disable placement tools as long as the model is not colliding with another object.
                if (Input.GetMouseButtonDown(1) || Input.GetKeyUp(KeyCode.Escape) && !placeObjectinWorld.ColliderAlert)
                {
                    placeObjectinWorld.ObjectMeshRenderer.material = MaterialManager.MyInstance.StandardMat;
                    enabled = false;

                    GameObject TChild = transform.GetChild(0).gameObject;
                    //if the nodes are not active, activate them
                    for (int i = 0; i < TChild.transform.childCount; i++)
                    {
                        if (TChild.transform.GetChild(i).gameObject.activeSelf)
                        {
                            TChild.transform.GetChild(i).gameObject.SetActive(false);
                        }
                    }

                    GameManager.ActiveModels.Remove(gameObject);
                }
                else if (placeObjectinWorld.ColliderAlert)
                {
                    Debug.Log("You cannot place this object here.");
                }
            }
            if (Input.GetKeyDown(KeyCode.R))
            {
                if (rotation == false)
                {
                    rotation = true;

                }
                else if (rotation == true)
                {
                    rotation = false;
                }
            }

    }
    public void OnMouseDrag()
    {
        //Drag the model around. 
        if (placeObjectinWorld != null)
        {
            if (GameManager.ActiveModels != null)
            {
                if (!EventSystem.current.IsPointerOverGameObject())
                {
                    if (rotation == false)
                    {
                        //Locked to grid.
                        if (TransformControls.MyInstance.SnapToGridToggle.isOn)
                        {
                            transform.position = CustomControllers.SnapToGrid(transform.position);
                        }
                        //Not locked to grid.
                        if (!TransformControls.MyInstance.SnapToGridToggle.isOn)
                        {
                            transform.position = CustomControllers.NonSnapToGrid(transform.position);
                        }

                    }
                    else if (rotation == true)
                    {
                        if (TransformControls.MyInstance.SnapToGridToggle.isOn)
                        {
                            float angle = Mathf.Atan2(transform.position.z - CustomControllers.SnapToGrid(transform.position).z, transform.position.x - CustomControllers.SnapToGrid(transform.position).x) * Mathf.Rad2Deg;
            
                            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, -angle, 0), rotationSpeed * Time.deltaTime);

                        }
                        if (!TransformControls.MyInstance.SnapToGridToggle.isOn)
                        {
                            float angle = Mathf.Atan2(transform.position.z -CustomControllers.NonSnapToGrid(transform.position).z, transform.position.x - CustomControllers.NonSnapToGrid(transform.position).x) * Mathf.Rad2Deg;

                            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, -angle, 0), rotationSpeed * Time.deltaTime);


                        }


                    }
                    placeObjectinWorld.MyCollisions();
                }

            }

        }
    }
    
}
