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
    private List<GameObject> list;
    private Dictionary<EnumMeshType, List<ShapeObjectDataInfo>> keyValuePairs;

    [Header("String path")]
    private readonly string MATERIAL_PATH = "Assets/Resources/Materials";

    private void Awake()
    {
        list = new List<GameObject>();
        keyValuePairs = new Dictionary<EnumMeshType, List<ShapeObjectDataInfo>>();
    }

    private void Update()
    {
        // update the Dictionnary entries values
    }

    private void OnDeserilizationLoad()
    {
        if (keyValuePairs != null)
            InstanciateLoadedAsset();
    }

    private void InstanciateLoadedAsset()
    {
        foreach (KeyValuePair<EnumMeshType, List<ShapeObjectDataInfo>> kvp in keyValuePairs)
        {
            EnumMeshType meshType = kvp.Key;
            foreach (ShapeObjectDataInfo entries in kvp.Value)
            {
                GameObject go = GameObject.CreatePrimitive((PrimitiveType)meshType);
                ShapeObjectDataInfo dataInfo = go.GetComponent<ShapeObjectDataInfo>();
                LoadDataToGameObject(go, dataInfo);
            }
        }
    }

    private void LoadDataToGameObject(GameObject go, ShapeObjectDataInfo dataInfo)
    {
        Vector3 vector3 = new Vector3();
        vector3.x = dataInfo.GetPosition.X;
        vector3.y = dataInfo.GetPosition.Y;
        vector3.z = dataInfo.GetPosition.Z;
        go.transform.position = vector3;

    }

    public void InstanciateShapes(GameObject myPrefab, int number)
    {
        for (int i = 0; i < number; i++)
        {
            if (myPrefab != null)
            {
                GameObject go = Instantiate(myPrefab, GetNewPosition(), Quaternion.identity);
#if !UNITY_EDITOR
                go.name = $"{GameManagerScript.Instance.GetMeshType.ToString().ToLower()} " + $"{list.Count}";
#endif
                list?.Add(go);
            }
        }
#if !UNITY_EDITOR
        AddToKVP(GameManagerScript.Instance.GetMeshType, list);
#endif
    }

    private void AddToKVP(EnumMeshType meshType, List<GameObject> list)
    {
        List<ShapeObjectDataInfo> dataInfoList = new List<ShapeObjectDataInfo>();
        foreach (GameObject go in list)
        {
            ShapeObject dat = go.GetComponent<ShapeObject>();
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

    private Vector3 GetNewPosition()
    {
        Vector3 myVec3 = new Vector3();
        myVec3.x = UnityEngine.Random.Range(-PLANE_WIDTH, PLANE_WIDTH);
        myVec3.y = SPAWN_HEIGHT;
        myVec3.z = UnityEngine.Random.Range(-PLANE_WIDTH, PLANE_DEPTH);
        return myVec3;
    }

    public EnumMeshType GetMeshType { get => meshType; set { meshType = value; } }

    public Dictionary<EnumMeshType, List<ShapeObjectDataInfo>> GetDictionnary { get => keyValuePairs; set { keyValuePairs = value; } }
}
