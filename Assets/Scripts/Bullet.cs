using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed = 20;
    [SerializeField] private int hitPower = 1;
    [SerializeField] private Vector2 direction = Vector2.right;
    [SerializeField] private GameObject impactFX;
    private bool isTriggered;
    private Health health;
    private SpriteRenderer spriteRenderer;

    private void OnEnable()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();    
    }

    void Start()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag != this.tag && isTriggered == false) {
            Debug.Log(collision.name);
            if (collision.transform.TryGetComponent<Health>(out health))
            {
                isTriggered = true;
                Damage();
            }
        }
    }

    void Update()
    {
        transform.Translate(direction*speed*Time.deltaTime);    //   Fire/Move Forwards

        DestroyOnBoundary();
    }

    void DestroyOnBoundary()
    {
        if (transform.position.y > 6 || transform.position.y < -6)
        {
            Destroy(gameObject);
        }
    }

    void Damage() {
        health.TakeDamage(hitPower);
        Instantiate(impactFX, this.transform);
        speed = 0;
        spriteRenderer.enabled = false;
        Destroy(this.gameObject, .4f);
    }


}
