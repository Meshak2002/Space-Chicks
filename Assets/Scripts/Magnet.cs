using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnet : MonoBehaviour
{
    private bool once;
    private float timer, initDuration;
    [SerializeField] private float duration, speed;
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
        // Reset magnet state when enabled
        spriteRenderer.enabled = true;
        cirCol.enabled = true;
        duration = initDuration;
    }

    private void Start()
    {
        PoolManager.instance.IncreaseSpeed += SpeedIncrease;    // Subscribe to event for speed increase in each level
    }

    void Update()
    {
        if (GameManager.instance.gameState != GameState.Running)
            return;

        if (once)           // If the magnet power is activated, count down its duration
        {
            timer = duration - Time.time;
            if (timer <= 0) LossPower();
        }
        else                // If not, move it downward
        {
            transform.Translate(Vector3.down * speed * Time.deltaTime);
        }

        if (GameManager.instance.IsPowerOn())           // Check if other power is active; if so, destroy the shield
        {
            PoolManager.instance.PoolDestroyObj(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && once==false)      // Check if collided with player 
        {
            ActivatePower();
        }
    }

    void ActivatePower()
    {
        once = true;
        AudioManager.instance.PickSFxPlay();
        duration += Time.time;
        GameManager.instance.magnetOn = true;
        GameManager.instance.power.SetActive(true);
        spriteRenderer.enabled = false;
        cirCol.enabled = false;
    }

    void SpeedIncrease()
    {
        speed += 1;
    }

    void LossPower()            //Called this after the duration of power of shield is finished
    {
        GameManager.instance.magnetOn = false;
        once = false;
        GameManager.instance.power.SetActive(false);
        PoolManager.instance.PoolDestroyObj(this.gameObject);
    }

}
