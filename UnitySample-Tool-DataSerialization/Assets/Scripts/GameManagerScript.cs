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
    [SerializeField] private EnumMeshType meshType;
    [SerializeField] private Material[] materials;
    private readonly int PLANE_WIDTH = 10;
    private readonly int PLANE_DEPTH = 10;
    private readonly int SPAWN_HEIGHT = 10;

    [Header("Database")]
    private List<GameObject> gameobject_list;
    private List<ShapeObjectDataInfo> dataInfos;

    [Header("String path")]
    private readonly string MATERIAL_PATH = "Materials/";

    private void Awake()
    {
        gameobject_list = new List<GameObject>();
        materials = Resources.LoadAll<Material>(MATERIAL_PATH);
        dataInfos = new List<ShapeObjectDataInfo>();
    }

    public void OnSerialization()
    {
        if (dataInfos != null)
            dataInfos = OnBeforeSerialization();
        Serialization.SaveBinaryFile();
    }

    public void OnDeserilization()
    {
        Serialization.LoadBinaryFile();
        if (dataInfos != null)
            InstanciateLoadedAsset();
    }

    #region INSTANCIATION
    public void InstanciateShapes(int number)
    {
        for (int i = 0; i < number; i++)
        {
            GameObject go = InstanciateAsset(GetRandomPosition(), meshType);
            //#if !UNITY_EDITOR
            go.name = $"{meshType.ToString().ToLower()} " + $"{gameobject_list.Count}";
            //#endif
            gameobject_list?.Add(go);
        }
    }

    private GameObject InstanciateAsset(Vector3 pos, EnumMeshType meshType)
    {
        GameObject go = GameObject.CreatePrimitive((PrimitiveType)meshType);
        go.AddComponent<MeshCollider>();
        go.GetComponent<MeshCollider>().convex = true;
        go.AddComponent<ShapeObject>();
        go.AddComponent<Rigidbody>();
        go.transform.position = pos;

        ShapeObject dat = go.GetComponent<ShapeObject>();

        dat.GetDataInfo.GetPosition = new MyVector3(pos.x, pos.y, pos.z);

        MeshRenderer meshRenderer = go.GetComponent<MeshRenderer>();
        int index = UnityEngine.Random.Range(0, materials.Length);
        meshRenderer.material = materials[index];
        Color color = meshRenderer.material.color;
        dat.GetDataInfo.GetColor = new MyColor(color.r, color.g, color.b, color.a);

        dat.GetDataInfo.GetEnumType = meshType;

        return go;
    }

    private void InstanciateLoadedAsset()
    {
        foreach (var entries in dataInfos)
        {
            GameObject go = OnDeserializationLoad(entries);
            gameobject_list?.Add(go);
        }
    }

    #endregion

    #region UTILS
    private Vector3 GetRandomPosition()
    {
        Vector3 myVec3 = new Vector3();
        myVec3.x = UnityEngine.Random.Range(-PLANE_WIDTH, PLANE_WIDTH);
        myVec3.y = UnityEngine.Random.Range(SPAWN_HEIGHT / 2, SPAWN_HEIGHT);
        myVec3.z = UnityEngine.Random.Range(-PLANE_WIDTH, PLANE_DEPTH);
        return myVec3;
    }

    private Vector3 ToVector3(MyVector3 pos)
    {
        return new Vector3(pos.GetX, pos.GetY, pos.GetZ);
    }

    private MyVector3 ToCustomVector3(Vector3 vector3)
    {
        return new MyVector3(vector3.x, vector3.y, vector3.z);
    }

    private Color ToColor(MyColor myColor)
    {
        Color color = new Color();
        color.r = myColor.GetX;
        color.g = myColor.GetY;
        color.b = myColor.GetZ;
        color.a = myColor.GetW;

        return color;
    }

    private List<ShapeObjectDataInfo> OnBeforeSerialization()
    {
        List<ShapeObjectDataInfo> List = new List<ShapeObjectDataInfo>();
        foreach (GameObject go in gameobject_list)
        {
            ShapeObjectDataInfo dat = go.GetComponent<ShapeObject>().GetDataInfo;
            dat.GetPosition = ToCustomVector3(go.transform.position);
            dat.GetVelocity = ToCustomVector3(go.GetComponent<Rigidbody>().velocity);
            List?.Add(dat);
        }
        return List;
    }

    private GameObject OnDeserializationLoad(ShapeObjectDataInfo entries)
    {
        GameObject go = GameObject.CreatePrimitive((PrimitiveType)entries.GetEnumType);
        go.AddComponent<MeshCollider>();
        go.GetComponent<MeshCollider>().convex = true;
        go.AddComponent<ShapeObject>();
        go.GetComponent<ShapeObject>().GetDataInfo = entries;
        go.AddComponent<Rigidbody>();
        go.GetComponent<MeshRenderer>().material.color = ToColor(entries.GetColor);
        go.transform.position = ToVector3(entries.GetPosition);
        return go;
    }
    #endregion

    #region PROPERTIES
    public EnumMeshType GetMeshType { get => meshType; set { meshType = value; } }

    public Material[] GetMaterials { get => materials; }

    public List<ShapeObjectDataInfo> GetDataInfos { get => dataInfos; set { dataInfos = value; } }
    #endregion
}
