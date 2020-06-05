using System.IO;
using UnityEditor;
using UnityEngine;

public class GenerateObject : EditorWindow
{
    string objectName;
    string path;

    Object meshObject;
    GameObject newMesh;


    [Tooltip("Not currently Working")]
    LayerMask layerMask;
    static int defaultLayer = 0;
    static int flags = 0;
    static string[] options = new string[] { "Null0", "Null1", "Null2", "Null3", "Null4", "Null5", "Null6", "Null7", "Ramps"};
    bool placeUnderground = false;
    GameObject go;
    GameObject snapPoints;
    private void OnEnable()
    {
        path = "Assets/Resources/Prefabs";
        
    }

    [MenuItem("Generators/Generate Object")]
    public static void ShowWindow()
    {
        GetWindow<GenerateObject>("Generate Object");
    }

    private void OnGUI()
    {
        GUILayout.Label("Generate an object to use in game.", EditorStyles.boldLabel);
        objectName = EditorGUILayout.TextField("Object Name", objectName);
        
        //Not working
        //defaultLayer = EditorGUILayout.LayerField("Collision Ignore Layer", defaultLayer);

        //Use this for layermasks
        flags = EditorGUILayout.MaskField("Flags", flags, options);
        placeUnderground = EditorGUILayout.Toggle("Underground Object", placeUnderground);
        meshObject = EditorGUILayout.ObjectField("New Object Model", meshObject, typeof(Object), false);

        if (GUILayout.Button("Generate Game Object"))
        {
            if (objectName != "")
            {
                GenerateDirectory((objectName));
            }
            else
                Debug.Log("Please specify an object name");
        }
    }


    void GenerateDirectory(string objectName)
    {
        string fullPath = Path.Combine(path + "/", objectName);

        Directory.CreateDirectory(fullPath);
        GenerateGameObject(fullPath);
    }

    void GenerateGameObject(string path)
    {
        go = new GameObject(objectName);
        Transform t = go.GetComponent<Transform>();
        GameObject newMesh = (GameObject)Instantiate(meshObject, t);
        newMesh.transform.name = objectName + "Child";

        newMesh.transform.SetParent(t);

        go.AddComponent<BoxCollider>();
        go.AddComponent<PlaceObjectInWorld>();
        go.AddComponent<EditModel>().enabled = false;

        ScriptableObjectUtility.CreateAsset<SO_Object>(path, objectName);
        MoveToPivot(go);

    }
    void MoveToPivot(GameObject go)
    {
        Transform tChild = go.transform.GetChild(0).GetComponent<Transform>();
        tChild.position = new Vector3(tChild.localScale.x / 2, tChild.position.y, tChild.localScale.z / 2);
        //x position is localscale / 2
        //y position is the same
        //z position is local scale / 2
        SetupDependencies(go);

    }

    void SetupDependencies(GameObject go)
    {
        PlaceObjectInWorld p = go.GetComponent<PlaceObjectInWorld>();
        //Box Collider
        BoxCollider bc = go.GetComponent<BoxCollider>();
        p.ObjectBoxCollider = bc;
        Transform tChild = go.transform.GetChild(0).GetComponent<Transform>();
        MeshRenderer mr = tChild.GetComponent<MeshRenderer>();
        go.layer = 8;
        tChild.gameObject.layer = 8;

        bc.center = mr.bounds.center;
        bc.size = mr.bounds.size - new Vector3(0.1f, -0.1f, 0.1f);

        //Child's Transform
        p.ObjectTransform = tChild;
        //Child's MeshRenderer
        p.ObjectMeshRenderer = tChild.gameObject.GetComponent<MeshRenderer>();
        //Setting the collision flags
        p.LayerMask = flags;
        //Reference the material manager
        p.MaterialManager = MaterialManager.MyInstance;
        SetupScriptableObject(go);
    }
    void SetupScriptableObject(GameObject go)
    {
        PlaceObjectInWorld p = go.GetComponent<PlaceObjectInWorld>();
        SO_Object so = Resources.Load<SO_Object>("Prefabs/" + objectName + "/" + objectName) as SO_Object;
        Debug.Log                 ("Prefabs/" + objectName + "/" + objectName);
        Debug.Log(so);
        p.PlaceableObject = so;
        SetupSnapPoints(go);
    }
    void SetupSnapPoints(GameObject go)
    {
        Transform tChild = go.transform.GetChild(0).GetComponent<Transform>();
        Vector3Int[] localCoordinates = new Vector3Int[4];
        //localCoordinates[0] = new Vector3Int(0, 0, 0);
        //localCoordinates[1] = new Vector3Int(1, 0, 0);
        //localCoordinates[2] = new Vector3Int(0, 0, 1);
        //localCoordinates[3] = new Vector3Int(1, 0, 1);
        //localCoordinates[4] = new Vector3Int(0, 1, 0);
        //localCoordinates[5] = new Vector3Int(1, 1, 0);
        //localCoordinates[6] = new Vector3Int(0, 1, 1);
        //localCoordinates[7] = new Vector3Int(1, 1, 1);

        localCoordinates[0] = new Vector3Int(
            (int)tChild.localPosition.x,
            (int)tChild.localPosition.y,
            (int)tChild.localPosition.z
            );

        localCoordinates[1] = new Vector3Int(
            (int)tChild.localPosition.x - (int)tChild.localScale.x,
            (int)tChild.localPosition.y,
            (int)tChild.localPosition.z
            );
        localCoordinates[2] = new Vector3Int(
            (int)tChild.localPosition.x - (int)tChild.localScale.x,
            (int)tChild.localPosition.y,
            (int)tChild.localPosition.z - (int)tChild.localScale.z
            );
        localCoordinates[3] = new Vector3Int(
            (int)tChild.localPosition.x,
            (int)tChild.localPosition.y,
            (int)tChild.localPosition.z - (int)tChild.localScale.z
            );

        if (!placeUnderground)
        {
            localCoordinates = new Vector3Int[8];
            localCoordinates[4] = new Vector3Int(
                (int)tChild.localPosition.x,
                (int)tChild.localPosition.y,
                (int)tChild.localPosition.z
                );

            localCoordinates[5] = new Vector3Int(
                (int)tChild.localPosition.x - (int)tChild.localScale.x,
                (int)tChild.localPosition.y,
                (int)tChild.localPosition.z
                );
            localCoordinates[6] = new Vector3Int(
                (int)tChild.localPosition.x - (int)tChild.localScale.x,
                (int)tChild.localPosition.y,
                (int)tChild.localPosition.z - (int)tChild.localScale.z
                );
            localCoordinates[7] = new Vector3Int(
                (int)tChild.localPosition.x,
                (int)tChild.localPosition.y,
                (int)tChild.localPosition.z - (int)tChild.localScale.z
                );

        }


        snapPoints = new GameObject("SnapPoint");
        snapPoints.AddComponent<SphereCollider>().radius = 0.05f;
        for (int i = 0; i < localCoordinates.Length; i++)
        {
            Instantiate(snapPoints, localCoordinates[i], Quaternion.identity, tChild);
        }

    }
    //Add the snapping points
    //Create a prefab of the new object, then delete it from the scene.

}
