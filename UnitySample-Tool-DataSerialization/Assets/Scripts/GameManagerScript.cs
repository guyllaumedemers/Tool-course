using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class GameManagerScript : MonoBehaviour
{
    private static GameManagerScript _instance;
    public static GameManagerScript Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<GameManagerScript>();
                if (_instance == null)
                {
                    GameObject go = new GameObject("Game Manager");
                    _instance = go.AddComponent<GameManagerScript>();
                    DontDestroyOnLoad(go);
                }
            }

            return _instance;
        }
    }

    [Header("Game Info")]
    [SerializeField] private int nb_shapes;
    [SerializeField] private GameObject prefab;
    [SerializeField] private EnumMeshType meshType;
    [SerializeField] private Material[] materials;
    private readonly int PLANE_WIDTH = 10;
    private readonly int PLANE_DEPTH = 10;
    private readonly int SPAWN_HEIGHT = 5;

    [Header("Database")]
    private List<GameObject> list;
    private Dictionary<EnumMeshType, List<ShapeObjectDataInfo>> keyValuePairs;

    [Header("String path")]
    private readonly string MATERIAL_PATH = "Materials/";

    private void Awake()
    {
        list = new List<GameObject>();
        keyValuePairs = new Dictionary<EnumMeshType, List<ShapeObjectDataInfo>>();
        materials = Resources.LoadAll<Material>(MATERIAL_PATH);
    }

    private void Update()
    {
        // update the Dictionnary entries values
    }

    public void OnSerialization()
    {
        if (keyValuePairs != null)
            UpdateDictionnary();
        Serialization.SaveBinaryFile();
    }

    public void OnDeserilization()
    {
        Serialization.LoadBinaryFile();
        if (keyValuePairs != null)
            InstanciateLoadedAsset();
    }

    public void InstanciateShapes(GameObject myPrefab, int number)
    {
        for (int i = 0; i < number; i++)
        {
            if (myPrefab != null)
            {
                GameObject go = Instantiate(myPrefab, GetNewPosition(), Quaternion.identity);
                //#if !UNITY_EDITOR
                go.name = $"{GameManagerScript.Instance.GetMeshType.ToString().ToLower()} " + $"{list.Count}";
                list?.Add(go);
                //#endif
            }
        }
        //#if !UNITY_EDITOR
        AddToKVP(GameManagerScript.Instance.GetMeshType, list);
        //#endif
    }

    private void InstanciateLoadedAsset()
    {
        foreach (KeyValuePair<EnumMeshType, List<ShapeObjectDataInfo>> kvp in keyValuePairs)
        {
            EnumMeshType meshType = kvp.Key;
            foreach (ShapeObjectDataInfo entries in kvp.Value)
            {
                GameObject go = LoadGameObjectFromDictionnary(entries, meshType);
                list?.Add(go);
                Destroy(go);
            }
        }
    }

    /*************************UPDATING GAMEOBJECT****************************/
    private GameObject LoadGameObjectFromDictionnary(ShapeObjectDataInfo entries, EnumMeshType meshType)
    {
        GameObject go = GameObject.CreatePrimitive((PrimitiveType)meshType);
        go.AddComponent<ShapeObject>();
        go.GetComponent<ShapeObject>().GetDataInfo = entries;
        return go;
    }

    private ShapeObjectDataInfo UpdateGameObjectData(GameObject go, ShapeObjectDataInfo dataInfo)
    {
        MeshRenderer meshRenderer = go.GetComponent<MeshRenderer>();

        Color myColor = meshRenderer.material.color;
        dataInfo.GetColor = new MyColor(myColor.r, myColor.g, myColor.b, myColor.a);

        Vector3 vector3 = go.transform.position;
        dataInfo.GetPosition = new MyVector3(vector3.x, vector3.y, vector3.z);
        return dataInfo;
    }

    /*****************************DICTIONNARY********************************/
    private void AddToKVP(EnumMeshType meshType, List<GameObject> list)
    {
        List<ShapeObjectDataInfo> dataInfoList = new List<ShapeObjectDataInfo>();
        foreach (GameObject go in list)
        {
            ShapeObject dat = go.GetComponent<ShapeObject>();
            dat.GetDataInfo = UpdateGameObjectData(go, dat.GetDataInfo);
            dataInfoList.Add(dat.GetDataInfo);
        }
        if (!keyValuePairs.ContainsKey(meshType))
            keyValuePairs.Add(meshType, dataInfoList);
        else
            ExpandDictionnaryList(meshType, dataInfoList);
        dataInfoList.Clear();
    }

    private void ExpandDictionnaryList(EnumMeshType meshType, List<ShapeObjectDataInfo> shapeObjectDataInfos)
    {
        foreach (ShapeObjectDataInfo entry in shapeObjectDataInfos)
        {
            keyValuePairs[meshType].Add(entry);
        }
    }

    private void UpdateDictionnary()
    {
        foreach (KeyValuePair<EnumMeshType, List<ShapeObjectDataInfo>> kvp in keyValuePairs)
        {
            EnumMeshType meshType = kvp.Key;
            foreach (ShapeObjectDataInfo entries in kvp.Value)
            {

            }
        }
    }

    /*****************************UTILITIES************************************/
    private Vector3 GetNewPosition()
    {
        Vector3 myVec3 = new Vector3();
        myVec3.x = UnityEngine.Random.Range(-PLANE_WIDTH, PLANE_WIDTH);
        myVec3.y = SPAWN_HEIGHT;
        myVec3.z = UnityEngine.Random.Range(-PLANE_WIDTH, PLANE_DEPTH);
        return myVec3;
    }

    /*****************************PROPERTIES************************************/
    public EnumMeshType GetMeshType { get => meshType; set { meshType = value; } }

    public Material[] GetMaterials { get => materials; }

    public Dictionary<EnumMeshType, List<ShapeObjectDataInfo>> GetDictionnary { get => keyValuePairs; set { keyValuePairs = value; } }
    /**************************************************************************/
}