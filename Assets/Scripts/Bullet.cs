using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed = 20;
    [SerializeField] private int hitPower = 1;
    [SerializeField] private Vector2 direction = Vector2.right;
    [SerializeField] private GameObject impactFX;
    [SerializeField] private owner bulletOwner;

    private bool isTriggered;
    private float initSpeed;
    private Health health;
    private SpriteRenderer spriteRenderer;
    private GameObject impactInstance;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        initSpeed = speed;
    }

    private void OnEnable()
    {
        // Reset properties
        spriteRenderer.enabled = enabled;
        speed = initSpeed;
        isTriggered = false;
    }

    private void Start()
    {
        // Subscribe to event for speed increase
        PoolManager.instance.IncreaseSpeed += SpeedIncrease;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if collision is not with the bullet owner and if the bullet is not already triggered
        if (collision.tag != bulletOwner.ToString() && !isTriggered)
        {
            // Check if collision is with a player and the shield is active
            if (collision.gameObject.layer == LayerMask.NameToLayer("Player") && GameManager.instance.shieldOn)
            {
                isTriggered = true;
                HitEffect();
                return;
            }

            // Check if collision object has Health component
            if (collision.TryGetComponent<Health>(out health))
            {
                isTriggered = true;
                if (bulletOwner == owner.Player)
                {
                    AudioManager.instance.PBulletSFxPlay();
                }
                // Deal damage and trigger hit effect
                Damage();
                HitEffect();
            }
        }
    }

    private void Update()
    {
        // Move the bullet forward
        if (GameManager.instance.gameState == GameState.Running)
        {
            transform.Translate(direction * speed * Time.deltaTime);
            DestroyOnBoundary();
        }
    }

    private void SpeedIncrease()
    {
        initSpeed += 1;
    }

    // Method to destroy bullet when it goes out of bounds
    private void DestroyOnBoundary()
    {
        if (transform.position.y > 5 || transform.position.y < -6)
        {
            PoolManager.instance.PoolDestroyObj(gameObject);
        }
    }

    // Method to deal damage to the health component
    private void Damage()
    {
        health.TakeDamage(hitPower);
    }

    // Method to trigger hit effect and destroy the bullet
    private void HitEffect()
    {
        impactInstance = PoolManager.instance.PoolInstantiateObj(impactFX, transform.position, transform.rotation, ObjType.VFX);
        speed = 0;
        spriteRenderer.enabled = false;
        StartCoroutine(Delay(0.4f));
    }

    // Coroutine to delay destroying impact visual effect and bullet
    private IEnumerator Delay(float delay)
    {
        yield return new WaitForSeconds(delay);
        PoolManager.instance.PoolDestroyObj(impactInstance);
        PoolManager.instance.PoolDestroyObj(gameObject);
    }
}

// Enumeration for bullet owner
public enum owner
{
    Chick,
    Player
}