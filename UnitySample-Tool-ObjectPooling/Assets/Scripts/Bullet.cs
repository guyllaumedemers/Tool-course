using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D), typeof(Rigidbody2D), typeof(SpriteRenderer))]
public class Bullet : MonoBehaviour
{
    [Header("Bullet Information")]
    [SerializeField] private Rigidbody2D rb2D;
    private float bullet_speed;
    private Vector2 bullet_direction;

    private void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
        bullet_speed = 0;
        bullet_direction = new Vector2();
    }

    public void InitilizeBullet(Vector2 direction, float speed)
    {
        bullet_speed = speed;
        bullet_direction = direction;
    }

    public void UpdateBullet()
    {
        rb2D.velocity += bullet_direction * bullet_speed * Time.fixedDeltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Terrain"))
            Pool();
    }

    public void Pool()
    {
        // add the bullet to the pool of the Object Pool
    }

    public void Depool()
    {
        // remove the bullet from the Object Pool and Initialize its components values
    }
}
