using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooling : MonoBehaviour
{
    private static ObjectPooling instance;
    public static ObjectPooling Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<ObjectPooling>();
                if (instance == null)
                {
                    GameObject go = new GameObject();
                    instance = go.AddComponent<ObjectPooling>();
                    DontDestroyOnLoad(go);
                }
            }
            return instance;
        }
    }

    [Header("Pool Stack")]
    private List<Bullet> bullets;

    private void Awake()
    {
        bullets = new List<Bullet>();
    }

    public void Pool(Bullet bullet)
    {
        bullets.Add(bullet);
        bullet.gameObject.SetActive(false);
    }

    public Bullet Depool(BulletType bulletType)
    {
        foreach (Bullet b in bullets)
        {
            if (b.GetBulletType.Equals(bulletType))
                return b.Depool();
        }
        return null;
    }

    public List<Bullet> GetBullets { get => bullets; set { bullets = value; } }
}
