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
    private Vector2 bullet_direction;
    private BulletType bulletType;

    private void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
        bullet_speed = 0;
        bullet_direction = new Vector2();
    }

    public void InitilizeBullet(BulletType type, Vector2 direction, float speed)
    {
        bullet_speed = speed;
        bullet_direction = direction;
        bulletType = type;
        dispose = false;
    }

    public void UpdateBullet()
    {
        // do a check to see if its inside the bounds of the camera
        if (dispose)
            Pool();
        // update its position if it is
        // otherwise pool the bullet
        rb2D.velocity += bullet_direction * bullet_speed * Time.fixedDeltaTime;
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
        bullets.Remove(this);
    }

    public Bullet Depool()
    {
        // remove the bullet from the Object Pool and Initialize its components values
        List<Bullet> bullets = ObjectPooling.Instance.GetBullets;
        foreach (Bullet b in bullets)
        {
            if (b.Equals(this))
                return b.Reset();
        }
        return null;
    }

    private Bullet Reset()
    {

        return new Bullet();
    }

    private Bullet GetBullet(HashSet<Bullet> bullets)
    {
        foreach (Bullet b in bullets)
        {
            if (b.Equals(this))
                return b;
        }
        return null;
    }

    public BulletType GetBulletType { get => bulletType; set { bulletType = value; } }
}
