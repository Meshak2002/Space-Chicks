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
    [SerializeField]private GameObject impactInstance;
    private float initSpeed;
    public owner bulletOwner;

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

    private void Start()
    {
        PoolManager.instance.IncreaseSpeed += SpeedIncrease;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag!=bulletOwner.ToString() && isTriggered == false)
        {
          

            if (collision.gameObject.CompareTag("Player") && GameManager.instance.shieldOn)
            {
                isTriggered = true;
                HitEffect();
                Debug.Log("Pealthh");
                return;
            }

            if (collision.transform.TryGetComponent<Health>(out health))
            {
               // Debug.Log(collision.transform.name+"  "+this.gameObject.name);
                isTriggered = true;
                if (bulletOwner == owner.Player)
                {
                    AudioManager.instance.PBulletSFxPlay();
                } 
                Damage();
                HitEffect();
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
    void SpeedIncrease()
    {
        initSpeed += 1;
    }

    void DestroyOnBoundary()
    {
        if (transform.position.y > 6 || transform.position.y < -6)
        {
            PoolManager.instance.PoolDestroyObj(gameObject);
        }
    }

    void Damage() {
        health.TakeDamage(hitPower);
    }

    void HitEffect()
    {
        impactInstance = PoolManager.instance.PoolInstantiateObj(impactFX, transform.position, transform.rotation, ObjType.VFX);
        speed = 0;
        spriteRenderer.enabled = false;
        StartCoroutine(Delay(.4f));
    }

    IEnumerator Delay(float delay)
    {
        yield return new WaitForSeconds(delay);
        PoolManager.instance.PoolDestroyObj(impactInstance);
        PoolManager.instance.PoolDestroyObj(gameObject);

        //Debug.Log("Delay");
    }
        

}

public enum owner
{
    Chick,
    Player
}
