using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Requiered Components")]
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private Rigidbody2D rb2D;
    [SerializeField] private SpriteRenderer[] spriteRenderers;
    [SerializeField] private Sprite[] gun_sprites;
    [SerializeField] private Animator animator;

    [Header("Attach Objects")]
    private string MUZZLEFLASH_TAG = "MuzzleFlash";
    private string WEAPON_TAG = "Weapon";
    private string ARM_TAG = "Arm";

    [Header("Animation Properties")]
    private string SIDEWAYS = "Sideways";
    private string UP = "Up";
    private string DOWN = "Down";

    [Header("Player Information")]
    private Vector2 movement;
    private float speed = 5.0f;

    private void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
    }

    private void Update()
    {
        Animate();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Animate()
    {
        UpdateAnimationClips(WEAPON_TAG, ARM_TAG);
        if (movement.x > 0)
            Flip(true);
        else
            Flip(false);
    }

    private void Flip(bool value)
    {
        foreach (SpriteRenderer spriteRenderer in spriteRenderers)
        {
            spriteRenderer.flipX = value;
        }
    }

    private void UpdateAnimationClips(string tag, string tag2)
    {
        ResetGunSprite(tag);
        UpdateDirection();
        if (movement.y < 0)
            UpdateSpriteRendered(tag, tag2, true);
        else if (movement.y > 0)
            UpdateSpriteRendered(tag, tag2, false);
    }

    private void UpdateDirection()
    {
        animator.SetInteger(UP, (int)movement.y);
        animator.SetInteger(DOWN, (int)movement.y);
        animator.SetInteger(SIDEWAYS, (int)movement.x);
    }

    private void UpdateSpriteRendered(string tag, string tag2, bool value)
    {
        SpriteRenderer renderer = new SpriteRenderer();
        foreach (SpriteRenderer sR in spriteRenderers)
        {
            if (sR.CompareTag(tag))
                renderer = sR;
        }
        EnableArmSprite(tag2, value);
        UpdateGunSprite(renderer, value);
    }

    private void EnableArmSprite(string tag, bool state)
    {
        SpriteRenderer renderer = new SpriteRenderer();
        foreach (SpriteRenderer sR in spriteRenderers)
        {
            if (sR.CompareTag(tag))
                renderer = sR;
        }
        renderer.gameObject.SetActive(state);
    }

    private void UpdateGunSprite(SpriteRenderer renderer, bool value)
    {
        if (value)
            renderer.sprite = gun_sprites[1];
        else
            renderer.sprite = gun_sprites[2];
    }

    private void ResetGunSprite(string tag)
    {
        SpriteRenderer renderer = new SpriteRenderer();
        foreach (SpriteRenderer sR in spriteRenderers)
        {
            if (sR.CompareTag(tag))
                renderer = sR;
        }
        renderer.sprite = gun_sprites[0];
    }

    private void Move()
    {
        if (movement != Vector2.zero)
            rb2D.velocity += movement * speed * Time.fixedDeltaTime;
        else
            rb2D.velocity = Vector2.zero;
    }

    public void OnMovevement(InputAction.CallbackContext value)
    {
        movement = value.ReadValue<Vector2>();
    }

    public void OnAttack(InputAction.CallbackContext value)
    {
        Debug.Log("Fire");
    }
}
