using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
    private static GameManagerScript instance;
    private GameManagerScript() { }
    public static GameManagerScript Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GameManagerScript();
            }
            return instance;
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
    private List<GameObject> gameObjects;
    private Dictionary<EnumMeshType, List<GameObject>> keyValuePairs;

    [Header("String path")]
    private readonly string MATERIAL_PATH = "Assets/Resources/Materials";

    private void Awake()
    {
        instance = this;
        gameObjects = new List<GameObject>();
        keyValuePairs = new Dictionary<EnumMeshType, List<GameObject>>();
        //materials = new Material[10];
        //materials[0] = Resources.Load<Material>("Assets/Resources/Materials/redMat.mat");
        ////materials = Resources.LoadAll<Material>(MATERIAL_PATH);
        //Debug.Log(materials[0]);
    }

    private void Update()
    {
        /// update the Dictionnary onChange
    }

    public void InstanciateShapes(GameObject myPrefab, int number)
    {
        for (int i = 0; i < number; i++)
        {
            if (myPrefab != null)
            {
                GameObject go = Instantiate(myPrefab, GetNewPosition(), Quaternion.identity);
                go.name = $"{GameManagerScript.Instance.GetMeshType.ToString().ToLower()} " + $"{gameObjects.Count}";
                gameObjects?.Add(go);
            }
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
}
