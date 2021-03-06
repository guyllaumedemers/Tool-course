using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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
    private List<GameObject> gameobject_list;
    private Dictionary<EnumMeshType, List<ShapeObject>> keyValuePairs;
    private List<ShapeObjectDataInfo> dataInfos;

    [Header("String path")]
    private readonly string MATERIAL_PATH = "Materials/";

    private void Awake()
    {
        gameobject_list = new List<GameObject>();
        keyValuePairs = new Dictionary<EnumMeshType, List<ShapeObject>>();
        dataInfos = new List<ShapeObjectDataInfo>();
        materials = Resources.LoadAll<Material>(MATERIAL_PATH);
    }

    public void OnSerialization()
    {
        if (keyValuePairs != null)
            UpdateDictionnaryEntries();
        Serialization.SaveBinaryFile();
    }

    public void OnDeserilization()
    {
        Serialization.LoadBinaryFile();
        if (dataInfos != null)
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
                go.name = $"{GameManagerScript.Instance.GetMeshType.ToString().ToLower()} " + $"{gameobject_list.Count}";
                gameobject_list?.Add(go);
                //#endif
            }
        }
        //#if !UNITY_EDITOR
        AddToKVP(GameManagerScript.Instance.GetMeshType, gameobject_list);
        //#endif
    }

    private void InstanciateLoadedAsset()
    {
        foreach (var entries in dataInfos)
        {
            GameObject go = CreateAsset(entries);
            gameobject_list?.Add(go);
        }
    }

    private GameObject CreateAsset(ShapeObjectDataInfo entries)
    {
        GameObject go = GameObject.CreatePrimitive((PrimitiveType)entries.GetEnumType);
        go.AddComponent<MeshCollider>();
        go.GetComponent<MeshCollider>().convex = true;
        go.AddComponent<ShapeObject>();
        go.GetComponent<ShapeObject>().GetDataInfo = entries;
        go.transform.position = ToVector3(entries.GetPosition);
        return go;
    }

    /*************************UPDATING GAMEOBJECT****************************/

    private ShapeObjectDataInfo UpdateGameObjectData(GameObject go, ShapeObjectDataInfo dataInfo, EnumMeshType meshType)
    {
        MeshRenderer meshRenderer = go.GetComponent<MeshRenderer>();
        dataInfo.GetEnumType = meshType;

        Color myColor = meshRenderer.material.color;
        dataInfo.GetColor = new MyColor(myColor.r, myColor.g, myColor.b, myColor.a);

        Vector3 vector3 = go.transform.position;
        dataInfo.GetPosition = new MyVector3(vector3.x, vector3.y, vector3.z);
        return dataInfo;
    }

    /*****************************DICTIONNARY********************************/
    private void AddToKVP(EnumMeshType meshType, List<GameObject> list)
    {
        List<ShapeObject> dataInfoList = new List<ShapeObject>();
        foreach (GameObject go in list)
        {
            ShapeObject dat = go.GetComponent<ShapeObject>();
            dat.GetDataInfo = UpdateGameObjectData(go, dat.GetDataInfo, meshType);
            dataInfoList.Add(dat);
        }
        if (!keyValuePairs.ContainsKey(meshType))
            keyValuePairs.Add(meshType, dataInfoList);
        else
            ExpandDictionnaryList(meshType, dataInfoList);
    }

    private void ExpandDictionnaryList(EnumMeshType meshType, List<ShapeObject> shapeObjectDataInfos)
    {
        foreach (ShapeObject entry in shapeObjectDataInfos)
        {
            keyValuePairs[meshType].Add(entry);
        }
    }

    private void UpdateDictionnaryEntries()
    {
        foreach (var entryTypeKey in keyValuePairs)
        {
            foreach (var shapeObject in entryTypeKey.Value)
            {
                MeshRenderer meshRenderer = shapeObject.GetComponent<MeshRenderer>();
                Transform transform = shapeObject.GetComponent<Transform>();

                MyColor myColor = shapeObject.ToCustomColor(meshRenderer.material.color);
                MyVector3 myVector3 = shapeObject.ToCustomVector(transform.position);

                shapeObject.UpdateDataInfo(myVector3, myColor);
                dataInfos.Add(shapeObject.GetDataInfo);
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

    private Vector3 ToVector3(MyVector3 pos)
    {
        Vector3 vector3 = new Vector3();
        vector3.x = pos.GetX;
        vector3.y = pos.GetY;
        vector3.z = pos.GetZ;
        return vector3;
    }

    /*****************************PROPERTIES************************************/
    public EnumMeshType GetMeshType { get => meshType; set { meshType = value; } }

    public Material[] GetMaterials { get => materials; }

    public Dictionary<EnumMeshType, List<ShapeObject>> GetDictionnary { get => keyValuePairs; set { keyValuePairs = value; } }

    public List<ShapeObjectDataInfo> GetShapeObjectDataInfos { get => dataInfos; set { dataInfos = value; } }
    /**************************************************************************/
}
