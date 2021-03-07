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
        ///// Check if the bullet exist inside the Object Pool
        Bullet bullet = ObjectPooling.Instance.Depool(bulletType, position, direction, speed);
        //// If it doesnt exist instanciate a new Gameobject
        if (bullet != null)
            BulletManager.Instance.GetBullets.Add(bullet);
        else
            bullet = CreateABullet(bulletType, position, direction, speed);
        return bullet;
    }

    private Bullet CreateABullet(BulletType bulletType, Vector2 position, Vector2 direction, float speed)
    {
        //// Instanciate a gameobject of the Bullet
        GameObject go = Instantiate(bulletPrefabsTypes[bulletType], position, Quaternion.identity);
        go.AddComponent<Bullet>();
        //// Add Bullet Component to the gameobject
        Bullet bullet = go.GetComponent<Bullet>();
        //// Initialize the gameobject variables
        bullet.InitilizeBullet(bulletType, position, direction, speed);
        //// add the Bullet to the BulletManager Script
        BulletManager.Instance.GetBullets.Add(bullet);
        //// Return the bullet
        return bullet;
    }
}

public enum BulletType
{
    RED,
    BLUE,
    GREEN
}
