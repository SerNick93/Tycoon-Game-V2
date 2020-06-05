using UnityEngine.EventSystems;
using UnityEngine;
using Nick.controllers;

public class PlaceObjectInWorld : MonoBehaviour
{
    Vector3 pos, newPos;
    static int numOfRamps = 0;
    bool colliderAlert = false;
    
    [SerializeField]
    BoxCollider objectBoxCollider;
    [SerializeField]
    Transform objectTransform;
    [SerializeField]
    MeshRenderer objectMeshRenderer;
    
    [SerializeField] MaterialManager materialManager;
    
    [SerializeField] LayerMask layerMask;
    [SerializeField] SO_Object placeableObject;
    public SO_Object PlaceableObject { get => placeableObject; set => placeableObject = value; }
    public bool ColliderAlert { get => colliderAlert; set => colliderAlert = value; }
    public MeshRenderer ObjectMeshRenderer { get => objectMeshRenderer; set => objectMeshRenderer = value; }
    public BoxCollider ObjectBoxCollider { get => objectBoxCollider; set => objectBoxCollider = value; }
    public Transform ObjectTransform { get => objectTransform; set => objectTransform = value; }
    public LayerMask LayerMask { get => layerMask; set => layerMask = value; }
    public MaterialManager MaterialManager { get => materialManager; set => materialManager = value; }

    public void InitScripts()
    {
        ObjectBoxCollider = GetComponent<BoxCollider>();
        ObjectTransform = transform.GetChild(0).GetComponent<Transform>();
        ObjectMeshRenderer = ObjectTransform.GetComponent<MeshRenderer>();        

        //cc.isTrigger = true;
        ObjectBoxCollider.center = ObjectMeshRenderer.bounds.center;
        ObjectBoxCollider.size = ObjectMeshRenderer.bounds.size - new Vector3(0.1f, -0.1f, 0.1f);
        
        gameObject.layer = 8;

        MaterialManager = MaterialManager.MyInstance;

        SetRampActive();
    }

    void FixedUpdate()
    {
        
        //Move model's posision with mouse movment.
        if (TransformControls.MyInstance.SnapToGridToggle.isOn)
        {
            /*If locked to grid.*/
            newPos = CustomControllers.SnapToGrid(newPos);
        }
        else if (!TransformControls.MyInstance.SnapToGridToggle.isOn)
        {
            /*If not locked to grid.*/
            newPos = CustomControllers.NonSnapToGrid(newPos);
        }

        //update model position and set the y axis to 0
        newPos.y = (transform.position.y);
        transform.position = newPos;

        MyCollisions();
    }
    private void LateUpdate()
    {
        //Stop placing the model
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CustomControllers.DestroyGameObject(gameObject);
        }

        //Rotate the model by 90deg
        if (Input.GetMouseButtonDown(1) && GameManager.MyInstance.ActiveModel)
        {
            transform.RotateAround(ObjectMeshRenderer.bounds.center, Vector3.up, 90f);
        }

        if (Input.GetMouseButtonUp(0) && colliderAlert == false)
        {
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                PlaceObject(newPos);
            }
        }
        if (PlaceableObject.cost > GameManager.MyInstance.TotalMoney)
        {
            if (ObjectMeshRenderer.material != MaterialManager.RedMat)
            {
                ObjectMeshRenderer.material = MaterialManager.RedMat;
            }
        }
    }
    public void SetRampActive()
    {
        //initialize the Ramp stamp
        pos = CustomControllers.NonSnapToGrid(pos);

        Instantiate(gameObject, pos, Quaternion.identity, MaterialManager.ObjectWorldHolder);

    }

    //The model will not be able to be placed if it collides with another model in the world
    public void MyCollisions()
    {
        Collider[] otherElementColliders = Physics.OverlapBox(ObjectBoxCollider.bounds.center, ObjectTransform.localScale / 2, transform.rotation, LayerMask);

        if (otherElementColliders.Length <= 1)
        {
            ObjectMeshRenderer.material = MaterialManager.GreenMat;
            if (colliderAlert)
            {
                ColliderAlert = false;
            }
        }      
        else if (otherElementColliders.Length > 1)
        {
            ObjectMeshRenderer.material = MaterialManager.RedMat;
            if (!colliderAlert)
            {
                ColliderAlert = true;
            }
        }
        
    }

    //Place a new instance of the model in the world.
    public void PlaceObject(Vector3 clickPoint)
    {
        if (PlaceableObject.cost <= GameManager.MyInstance.TotalMoney)
        {
            numOfRamps++;
            GameObject go = Instantiate(gameObject, clickPoint, transform.rotation, MaterialManager.ObjectWorldHolder);
            go.transform.GetChild(0).GetComponent<MeshRenderer>().material = MaterialManager.StandardMat;
            go.GetComponent<PlaceObjectInWorld>().enabled = false;

            #region Only used in editor
#if UNITY_EDITOR
            go.transform.name = string.Format("{0}{1}", "Ramp Container: ", numOfRamps);
            go.transform.GetChild(0).GetComponent<Transform>().name = string.Format("{0}{1}", "Ramp: ", numOfRamps);
#endif
            #endregion

            GameManager.MyInstance.ReduceMoney(PlaceableObject.cost);

            if (!PlaceableObject.multiPlace)
            {
                CustomControllers.DestroyGameObject(gameObject);
                GameManager.MyInstance.ActiveModel = null;
            }
        }
    }
    private void OnCollisionStay(Collision collision)
    {
                    
        if (PlaceableObject.placeUnderground)
        {

            if (collision.transform.gameObject.layer == 9)
            {
                foreach (ContactPoint contact in collision.contacts)
                {
                    var floorBelowObject = collision.transform.GetComponent<FloorData>();
                    if (!GameManager.MyInstance.ActiveModel)
                    {
                        if (floorBelowObject.enabled)
                        {
                            floorBelowObject.DisableThis();
                        }
                    }
                    if (GameManager.MyInstance.ActiveModel)
                    {
                        floorBelowObject.EnableThis();
                    }
                }
            }
            
        }

    }

    #region Ramp container bounding box
    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red;

    //    Matrix4x4 rotationMatrix = Matrix4x4.TRS((newPos), transform.rotation, ObjectTransform.transform.localScale);

    //    Gizmos.matrix = transform.localToWorldMatrix;
    //    Gizmos.DrawWireCube(ObjectBoxCollider.center, ObjectTransform.localScale);
    //}
    #endregion
}
