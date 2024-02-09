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
    private GameObject impactInstance;
    private float initSpeed;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        initSpeed = speed;
    }

    private void OnEnable()
    {
        spriteRenderer.enabled = enabled;
        speed = initSpeed;
        isTriggered = false;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag != this.tag && isTriggered == false) {
            if (collision.transform.TryGetComponent<Health>(out health))
            {
                isTriggered = true;
                Damage();
            }
        }
    }

    void Update()
    {
        if (GameManager.instance.gameState == GameState.Running)
        {
            transform.Translate(direction * speed * Time.deltaTime);    //   Fire/Move Forwards
            DestroyOnBoundary();
        }
    }

    void DestroyOnBoundary()
    {
        if (transform.position.y > 6 || transform.position.y < -6)
        {
            //Destroy(gameObject);
            PoolManager.instance.poolDestroyObj(gameObject);
        }
    }

    void Damage() {
        health.TakeDamage(hitPower);
        //Instantiate(impactFX, transform.position,transform.rotation);
        impactInstance = PoolManager.instance.poolInstantiateObj(impactFX, transform.position, transform.rotation, ObjType.VFX);
        speed = 0;
        spriteRenderer.enabled = false;
        //Destroy(this.gameObject, .4f);
        StartCoroutine(Delay(.4f));
    }

    IEnumerator Delay(float delay)
    {
        yield return new WaitForSeconds(delay);
        PoolManager.instance.poolDestroyObj(impactInstance);
        PoolManager.instance.poolDestroyObj(gameObject);
    }


}
