using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletFactory : MonoBehaviour
{
    private static BulletFactory instance;
    public static BulletFactory Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<BulletFactory>();
                if (instance == null)
                {
                    GameObject go = new GameObject();
                    instance = go.AddComponent<BulletFactory>();
                    DontDestroyOnLoad(go);
                }
            }
            return instance;
        }
    }

    [Header("Caching Resources")]
    [SerializeField] private GameObject[] bulletsPrefabs;
    private string BULLET_PATH = "Prefabs/Bullets";
    [SerializeField] private Dictionary<BulletType, GameObject> bulletPrefabsTypes;

    private void Awake()
    {
        bulletsPrefabs = Resources.LoadAll<GameObject>(BULLET_PATH);
        if (bulletsPrefabs != null)
            Create();
    }

    private void Create()
    {
        bulletPrefabsTypes = new Dictionary<BulletType, GameObject>()
        {
            { BulletType.BLUE, bulletsPrefabs[0]},
            { BulletType.GREEN, bulletsPrefabs[1]},
            { BulletType.RED, bulletsPrefabs[2]}
        };
    }

    public Bullet InstanciateABullet(BulletType bulletType, Vector2 position, Vector2 direction, float speed)
    {
        // Create a bullet from prefab Dict<>
        GameObject go = Instantiate(bulletPrefabsTypes[bulletType], position, Quaternion.identity);
        go.AddComponent<Bullet>();
        Bullet bullet = go.GetComponent<Bullet>();
        // Initialize its components values
        bullet.InitilizeBullet(direction, speed);
        // Add the bullet to the bullet manager
        BulletManager.Instance.GetBullets.Add(bullet);
        return bullet;
    }
}

public enum BulletType
{
    RED,
    BLUE,
    GREEN
}
