using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    private static BulletManager instance;
    public static BulletManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<BulletManager>();
                if (instance == null)
                {
                    GameObject go = new GameObject();
                    instance = go.AddComponent<BulletManager>();
                    DontDestroyOnLoad(go);
                }
            }
            return instance;
        }
    }

    [Header("Manager Informations")]
    [SerializeField] private HashSet<Bullet> bullets;

    private void Awake()
    {
        bullets = new HashSet<Bullet>();
    }

    private void Update()
    {
        foreach (Bullet b in bullets)
        {
            b.UpdateBullet();
        }
    }

    public HashSet<Bullet> GetBullets { get => bullets; set { bullets = value; } }
}
