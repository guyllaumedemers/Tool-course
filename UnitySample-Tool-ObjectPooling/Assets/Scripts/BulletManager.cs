using System;
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
    [SerializeField] private List<Bullet> bullets;

    private void Awake()
    {
        bullets = new List<Bullet>();
    }

    private void Update()
    {
        for (int i = bullets.Count - 1; i >= 0; i--)
        {
            bullets[i].UpdateBullet();
        }
    }

    public List<Bullet> GetBullets { get => bullets; set { bullets = value; } }
}
