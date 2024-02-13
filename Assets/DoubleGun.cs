using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleGun : MonoBehaviour
{
    private bool once;
    private float timer;
    [SerializeField] private float duration, initDuration, speed;
    private CircleCollider2D cirCol;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        cirCol = GetComponent<CircleCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        initDuration = duration;
    }

    private void OnEnable()
    {
        spriteRenderer.enabled = true;
        cirCol.enabled = true;
        duration = initDuration;
    }
    private void Start()
    {
        PoolManager.instance.IncreaseSpeed += SpeedIncrease;
    }

    void Update()
    {
        if (GameManager.instance.gameState != GameState.Running)
            return;

        if (once)
        {
            timer = duration - Time.time;
            if (timer <= 0) LossPower();
        }
        else
        {
            transform.Translate(Vector3.down * speed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && once == false)
        {
            once = true;
            AudioManager.instance.PickSFxPlay();
            duration += Time.time;
            GameManager.instance.doubleGunOn = true;
            GameManager.instance.power.SetActive(true);
            spriteRenderer.enabled = false;
            cirCol.enabled = false;
        }
    }

    void SpeedIncrease()
    {
        speed += 1;
    }

    void LossPower()
    {
        GameManager.instance.doubleGunOn = false;
        once = false;
        GameManager.instance.power.SetActive(false);
        PoolManager.instance.poolDestroyObj(this.gameObject);
    }

}