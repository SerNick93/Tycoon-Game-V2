using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public enum OPTIONS
{
    AboveGround = 0,
    Subterranean = 1,
    StraightRail = 2,
    CornerRail = 3,
    Custom = 4
}

public class GenerateObject : EditorWindow
{

    //TODO:: Refactoring of this script
    //TODO::Objects which are offgrid also will not work with the snapping points(VertSlope)
    //TODO::If the scriptable object already exists, then overwrite it

    bool[] success;

    string objectName;
    string path;

    UnityEngine.Object meshObject;
    GameObject newMesh;
    UnlockArrayForEditorWindow unlockArray;


    [Tooltip("Not currently Working")]
    LayerMask layerMask;
    static int defaultLayer = 0;
    static int flags = 0;
    static string[] options = new string[] { "Null0", "Null1", "Null2", "Null3", "Null4", "Null5", "Null6", "Null7", "Ramps" };
    GameObject go;
    UnityEngine.Object thumbnail;
    Image image;

    private OPTIONS objectType;
    static string description;
    static float researchTime;
    static int researchCost, buildCost;
    static bool multiPlace = false;
    static bool isUnlocked;
    static bool isSnappable;
    static int customSnapPointsNo;

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
        scriptableObj = new ScriptableObject();
        serialObj = new SerializedObject(this);
        newUnlockProperty = serialObj.FindProperty("newUnlocks");
        prerequisitUnlocks = serialObj.FindProperty("prerequisits");
        success = new bool[7];
        //image = (Image)thumbnail;
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

        //Use this for layermasks
        flags = EditorGUILayout.MaskField("Flags", flags, options);

        meshObject = EditorGUILayout.ObjectField("New Object Model", meshObject, typeof(UnityEngine.Object), false);
        //Determines the type of pivot and snappoints set.
        objectType = (OPTIONS)EditorGUILayout.EnumPopup("Object Type", objectType);
        if (objectType == OPTIONS.Custom)
        {
            customSnapPointsNo = EditorGUILayout.IntField("Number of Snap Points", customSnapPointsNo);
        }
        isSnappable = EditorGUILayout.Toggle("Snappable", isSnappable);
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

        if (GUILayout.Button("Generate Game Object"))
        {
            if (objectName != "")
            {
                GenerateDirectory(objectName);
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
        success[0] = true;
    }

    void GenerateGameObject(string path)
    {
        go = new GameObject(objectName + "Prefab");
        Transform t = go.GetComponent<Transform>();
        GameObject newMesh = (GameObject)Instantiate(meshObject, t);
        newMesh.transform.name = objectName + "Child";

        newMesh.transform.SetParent(t);

        go.AddComponent<BoxCollider>();
        go.AddComponent<PlaceObjectInWorld>();
        go.AddComponent<EditModel>().enabled = false;
        Transform tChild = go.transform.GetChild(0).GetComponent<Transform>();
        ScriptableObjectUtility.CreateAsset<SO_Object>(path, objectName);
        success[1] = true;

        if (isSnappable)
        {
            switch (objectType)
            {
                case OPTIONS.AboveGround:
                    OvergroundSnapPoints(go, tChild);
                    break;
                case OPTIONS.Subterranean:
                    BelowGroundSnapPoints(go, tChild);
                    break;
                case OPTIONS.StraightRail:
                    RailSnapPoints(go, tChild);
                    break;
                case OPTIONS.CornerRail:
                    CornerRailSnapPoints(go, tChild);
                    break;
                case OPTIONS.Custom:
                    customSnapPoints(go, tChild);
                    break;
                default:
                    break;
            }
        }
        else
        {
            switch (objectType)
            {
                case OPTIONS.AboveGround:
                    tChild.position = new Vector3(tChild.localScale.x / 2, tChild.position.y, tChild.localScale.z / 2);
                    break;
                case OPTIONS.Subterranean:
                    tChild.position = new Vector3(tChild.localScale.x / 2, tChild.position.y, tChild.localScale.z / 2);
                    break;
                case OPTIONS.StraightRail:
                    tChild.position = new Vector3(0, tChild.position.y, tChild.localScale.z / 2);
                    break;
                case OPTIONS.CornerRail:
                    tChild.position = new Vector3(tChild.localScale.x / 2, tChild.position.y, tChild.localScale.z / 2);

                    break;
                default:
                    break;
            }
            SetupDependencies(go);
        }
    }

    private void customSnapPoints(GameObject go, Transform tChild)
    {
        tChild.position = new Vector3(tChild.localScale.x / 2, tChild.position.y, tChild.localScale.z / 2);
        Vector3Int[] localCoordinates = new Vector3Int[customSnapPointsNo];

        for (int i = 0; i < customSnapPointsNo; i++)
        {
            localCoordinates[i] = new Vector3Int(
            (int)tChild.localPosition.x,
            (int)tChild.localPosition.y,
            (int)tChild.localPosition.z
            );
        }
        SetupSnapPoints(go, localCoordinates, tChild);
    }

    private void RailSnapPoints(GameObject go, Transform tChild)
    {
        tChild.position = new Vector3(0, tChild.position.y, tChild.localScale.z / 2);

        Vector3Int[] localCoordinates = new Vector3Int[2];
        localCoordinates[0] = new Vector3Int(
            (int)tChild.localPosition.x,
            (int)tChild.localPosition.y,
            (int)tChild.localPosition.z
            );
        localCoordinates[1] = new Vector3Int(
            (int)tChild.localPosition.x,
            (int)tChild.localPosition.y,
            (int)tChild.localPosition.z - (int)tChild.localScale.z
            );

        success[2] = true;

        SetupSnapPoints(go, localCoordinates, tChild);
    }

    private void BelowGroundSnapPoints(GameObject go, Transform tChild)
    {
        tChild.position = new Vector3(tChild.localScale.x / 2, tChild.position.y, tChild.localScale.z / 2);

        Vector3Int[] localCoordinates = new Vector3Int[4];
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
        success[2] = true;

        SetupSnapPoints(go, localCoordinates, tChild);
    }

    private void OvergroundSnapPoints(GameObject go, Transform tChild)
    {
        tChild.position = new Vector3(tChild.localScale.x / 2, tChild.position.y, tChild.localScale.z / 2);

        Vector3Int[] localCoordinates = new Vector3Int[8];
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
        success[2] = true;

        SetupSnapPoints(go, localCoordinates, tChild);

    }
    private void CornerRailSnapPoints(GameObject go, Transform tChild)
    {
        tChild.position = new Vector3(tChild.localScale.x / 2, tChild.position.y, tChild.localScale.z / 2);

        Vector3Int[] localCoordinates = new Vector3Int[2];
        localCoordinates[0] = new Vector3Int(
            (int)tChild.localPosition.x,
            (int)tChild.localPosition.y,
            (int)tChild.localPosition.z
            );

        localCoordinates[1] = new Vector3Int(
            (int)tChild.localPosition.x - (int)tChild.localScale.x,
            (int)tChild.localPosition.y,
            (int)tChild.localPosition.z - (int)tChild.localScale.z
            );
        success[2] = true;

        SetupSnapPoints(go, localCoordinates, tChild);

    }

    void SetupSnapPoints(GameObject go, Vector3Int[] localCoordinates, Transform tChild)
    {
        GameObject node;
        node = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        node.transform.parent = tChild.transform;
        node.transform.localScale = node.transform.localScale / 5 ;
        node.GetComponent<SphereCollider>().radius = 0.5f;

        for (int i = 0; i < localCoordinates.Length; i++)
        {
            GameObject sp = Instantiate(node, localCoordinates[i], Quaternion.identity, tChild);
            sp.transform.name = "SnapPoint" + i;
        }
        DestroyImmediate(node);
        success[3] = true;

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

        bc.isTrigger = true;
        bc.center = mr.bounds.center;
        bc.size = mr.bounds.size - new Vector3(0.1f, 0.1f, 0.1f);

        //Child's Transform
        p.ObjectTransform = tChild;
        //Child's MeshRenderer
        p.ObjectMeshRenderer = tChild.gameObject.GetComponent<MeshRenderer>();
        //Setting the collision flags
        p.LayerMask = flags;
        //Reference the material manager
        p.MaterialManager = MaterialManager.MyInstance;
        success[4] = true;

        SetupScriptableObject(go);
    }

    void SetupScriptableObject(GameObject go)
    {
        PlaceObjectInWorld p = go.GetComponent<PlaceObjectInWorld>();
        SO_Object so = Resources.Load<SO_Object>("Prefabs/" + objectName + "/" + objectName) as SO_Object;
        p.PlaceableObject = so;

        p.PlaceableObject.multiPlace = multiPlace;

        if ((int)objectType == 2)
        {
            p.PlaceableObject.placeUnderground = true;
        }
        else
            p.PlaceableObject.placeUnderground = false;

        p.PlaceableObject.cost = buildCost;
        //p.PlaceableObject.thumbnail = image;
        p.PlaceableObject.unlockName = objectName;
        p.PlaceableObject.description = description;
        p.PlaceableObject.researchTime = researchTime;
        p.PlaceableObject.researchCost = researchCost;
        p.PlaceableObject.unlockObject = newUnlocks;
        p.PlaceableObject.prerequisits = prerequisits;
        success[5] = true;

        CreatePrefab(go);
    }


    void CreatePrefab(GameObject go)
    {
        string path = "Assets/Resources/Prefabs/" + objectName + "/";
        PrefabUtility.SaveAsPrefabAssetAndConnect(go, "Assets/Resources/Prefabs/" + objectName + "/" + objectName + ".prefab", InteractionMode.UserAction);

        DestroyImmediate(go);
        success[6] = true;

        if (checkIfUnlocked())
        {
            Debug.Log("Prefab: " + objectName + " successfully generated.");
        }
    }
    private bool checkIfUnlocked()
    {
        bool[] isSuccess = new bool[success.Length];

        for (int i = 0; i < success.Length; i++)
        {
            if (success[i] == true)
            {
                isSuccess[i] = true;
            }
            else
                continue;
        }

        return isSuccess.All(x => x);

    }

}
