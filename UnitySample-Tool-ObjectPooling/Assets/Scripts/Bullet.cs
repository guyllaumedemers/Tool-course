using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D), typeof(Rigidbody2D), typeof(SpriteRenderer))]
public class Bullet : MonoBehaviour
{
    [Header("Bullet Information")]
    [SerializeField] private Rigidbody2D rb2D;
    [SerializeField] private float bullet_speed;
    private bool dispose;
    private BulletType bulletType;
    private Vector2 bullet_direction;
    private string LAYER_MASK = "Bullets";

    private void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
        bullet_speed = 0;
        bullet_direction = new Vector2();
        this.gameObject.layer = LayerMask.NameToLayer(LAYER_MASK);
    }

    private void FixedUpdate()
    {
        //rb2D.AddRelativeForce(bullet_direction * bullet_speed, ForceMode2D.Impulse);
        rb2D.velocity += bullet_direction * bullet_speed;
    }

    public void InitilizeBullet(BulletType type, Vector2 position, Vector2 direction, float speed)
    {
        dispose = false;
        bullet_speed = speed;
        bulletType = type;
        this.transform.position = position;
        bullet_direction = direction;
    }

    public void UpdateBullet()
    {
        //// Check if the Bullet is Rendered by the Camera
        if (dispose)
            Pool();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Terrain"))
            Pool();
    }

    private void OnBecameInvisible()
    {
        dispose = true;
    }

    public void Pool()
    {
        HashSet<Bullet> bullets = BulletManager.Instance.GetBullets;
        if (bullets.Contains(this))
            ObjectPooling.Instance.Pool(GetBullet(bullets));
        //// We remove the Bullet from the BulletManager
        bullets.Remove(this);
        //// need to set inactive the gameobject
        this.gameObject.SetActive(false);
    }

    public Bullet Depool(BulletType bulletType, Vector2 position, Vector2 direction, float speed)
    {
        //// We Re-Enable the Bullet Gameobject
        this.gameObject.SetActive(true);
        //// It Initialize the bullets Componenets
        InitilizeBullet(bulletType, position, direction, speed);
        //// It removes the bullet from the Object Pool Stack<>
        ObjectPooling.Instance.GetBullets.Remove(this);
        return this;
    }

    private Bullet GetBullet(HashSet<Bullet> bullets)
    {
        try
        {
            foreach (Bullet b in bullets)
            {
                if (b.Equals(this))
                    return b;
            }
        }
        catch (InvalidOperationException e)
        {
            Debug.Log("Invalid Operation Exception : " + e.Message);
        }
        return null;
    }

    public BulletType GetBulletType { get => bulletType; set { bulletType = value; } }
}