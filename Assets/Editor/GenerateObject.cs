using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;


public class GenerateObject : EditorWindow
{

    //TODO::Rails and objects with a width of less than one need the snapping options changed to account for their
    //small size.
    //TODO:: Refactoring of this script
    //TODO::Objects which are offgrid also will not work with the snapping points(VertSlope)
    //TODO::If the scriptable object already exists, then overwrite it

    string objectName;
    string path;

    Object meshObject;
    GameObject newMesh;
    UnlockArrayForEditorWindow unlockArray;


    [Tooltip("Not currently Working")]
    LayerMask layerMask;
    static int defaultLayer = 0;
    static int flags = 0;
    static string[] options = new string[] { "Null0", "Null1", "Null2", "Null3", "Null4", "Null5", "Null6", "Null7", "Ramps"};
    bool placeUnderground = false;
    GameObject go;
    GameObject snapPoints;
    Object thumbnail;
    Image image;

    static string description;
    static float researchTime;
    static int researchCost, buildCost;
    static bool multiPlace = false;
    static bool isUnlocked;

    [SerializeField]
    SO_NewUnlock[] newUnlocks = new SO_NewUnlock[0];
    [SerializeField]
    SO_NewUnlock[] prerequisits = new SO_NewUnlock[0];

    ScriptableObject scriptableObj;
    SerializedObject serialObj;
    SerializedProperty newUnlockProperty;
    SerializedProperty prerequisitUnlocks;

    private void OnEnable()
    {
        path = "Assets/Resources/Prefabs";
        //ScriptableObject scriptableObj = this;
        scriptableObj = new ScriptableObject();
        serialObj = new SerializedObject(this);
        newUnlockProperty = serialObj.FindProperty("newUnlocks");
        prerequisitUnlocks = serialObj.FindProperty("prerequisits");
        image = (Image)thumbnail;    }

    [MenuItem("Generators/Generate Object")]
    public static void ShowWindow()
    {
        GetWindow<GenerateObject>("Generate Object");
    }

    private void OnGUI()
    {

        GUILayout.Label("Generate an object to use in game.", EditorStyles.boldLabel);
        objectName = EditorGUILayout.TextField("Object Name", objectName);
        
        //Use this for layermasks
        flags = EditorGUILayout.MaskField("Flags", flags, options);

        placeUnderground = EditorGUILayout.Toggle("Underground Object", placeUnderground);
        meshObject = EditorGUILayout.ObjectField("New Object Model", meshObject, typeof(Object), false);


        EditorGUILayout.Space(30f);
        GUILayout.Label("Assign the object's properties", EditorStyles.boldLabel);
        EditorGUILayout.PrefixLabel("Object Description");
        GUIStyle wrapText = EditorStyles.textArea;
        wrapText.wordWrap = true;
        description = EditorGUILayout.TextArea(description, wrapText, GUILayout.Height(50f));
        thumbnail = EditorGUILayout.ObjectField("Thumbnail", thumbnail, typeof(Image), false);
        researchTime = EditorGUILayout.FloatField("Research Time (Float)", researchTime);
        researchCost = EditorGUILayout.IntField("Research Cost (Integer)", researchCost);
        buildCost = EditorGUILayout.IntField("Build Cost (Integer)", buildCost);

        isUnlocked = EditorGUILayout.Toggle("Already Unlocked?", isUnlocked);



        EditorGUILayout.PropertyField(newUnlockProperty, true);
        EditorGUILayout.PropertyField(prerequisitUnlocks, true);
        serialObj.ApplyModifiedProperties();
            //prerequisits = EditorGUILayout.ObjectField("Thumbnail", prerequisits, typeof(SO_NewUnlock), true);






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
        p.PlaceableObject = so;

        p.PlaceableObject.multiPlace = multiPlace;
        p.PlaceableObject.placeUnderground = placeUnderground;
        p.PlaceableObject.cost = buildCost;
        p.PlaceableObject.thumbnail = image;
        p.PlaceableObject.unlockName = objectName;
        p.PlaceableObject.description = description;
        p.PlaceableObject.researchTime = researchTime;
        p.PlaceableObject.researchCost = researchCost;
        p.PlaceableObject.unlockObject = newUnlocks;
        p.PlaceableObject.prerequisits = prerequisits;

        SetupSnapPoints(go);
    }
    void SetupSnapPoints(GameObject go)
    {
        Transform tChild = go.transform.GetChild(0).GetComponent<Transform>();

        Vector3Int[] localCoordinates = new Vector3Int[0];

        if (placeUnderground)
        {
            localCoordinates = new Vector3Int[4];

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
        }
        else
        {
            localCoordinates = new Vector3Int[8];

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
            localCoordinates[4] = new Vector3Int(
                (int)tChild.localPosition.x,
                (int)tChild.localPosition.y + (int)tChild.localScale.y,
                (int)tChild.localPosition.z
                );

            localCoordinates[5] = new Vector3Int(
                (int)tChild.localPosition.x - (int)tChild.localScale.x,
                (int)tChild.localPosition.y + (int)tChild.localScale.y,
                (int)tChild.localPosition.z
                );
            localCoordinates[6] = new Vector3Int(
                (int)tChild.localPosition.x - (int)tChild.localScale.x,
                (int)tChild.localPosition.y + (int)tChild.localScale.y,
                (int)tChild.localPosition.z - (int)tChild.localScale.z
                );
            localCoordinates[7] = new Vector3Int(
                (int)tChild.localPosition.x,
                (int)tChild.localPosition.y + (int)tChild.localScale.y,
                (int)tChild.localPosition.z - (int)tChild.localScale.z
                );
        }

        snapPoints = new GameObject("SnapPoint");
        snapPoints.tag = "SnapPoint";
        snapPoints.AddComponent<SphereCollider>().radius = 0.05f;
        for (int i = 0; i < localCoordinates.Length; i++)
        {
           GameObject sp =  Instantiate(snapPoints, localCoordinates[i], Quaternion.identity, tChild);
           sp.transform.name = "SnapPoint" + i;
        }
        DestroyImmediate(snapPoints);
        CreatePrefab(go);
    }
    void CreatePrefab(GameObject go)
    {
        string path = "Assets/Resources/Prefabs/" + objectName + "/";
        Debug.Log(path);
        PrefabUtility.SaveAsPrefabAssetAndConnect(go, "Assets/Resources/Prefabs/" + objectName + "/" + objectName + ".prefab", InteractionMode.UserAction);

        DestroyImmediate(go);
    }

}
