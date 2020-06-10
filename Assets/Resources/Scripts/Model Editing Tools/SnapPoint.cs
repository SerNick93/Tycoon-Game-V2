using UnityEngine;

public class SnapPoint : MonoBehaviour
{
    private void OnDisable()
    {
        transform.GetComponent<MeshRenderer>().material = MaterialManager.MyInstance.StandardMat;
    }

    Transform parent;
    bool parentFound = false;

    public Transform Parent { get => parent; set => parent = value; }
    public bool ParentFound { get => parentFound; set => parentFound = value; }

    private void OnEnable()
    {
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("SnapPoint"))
        {
            if (other.transform.parent.parent != null)
            {
                if (other.transform.parent.parent.gameObject.layer == 8)
                {
                    Parent = other.transform.parent.parent;
                    Debug.Log(other.transform.parent.parent);
                    ParentFound = true;
                }
                //Debug.Log(transform.parent.GetComponent<PlaceObjectInWorld>());
                //Debug.Log(other.transform.parent.name);
            }

        }

    }
    private void OnTriggerExit(Collider other)
    {
        ParentFound = false;
        Parent = null;
    }

}
