using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnet : MonoBehaviour
{
    // Start is called before the first frame update
    private bool once;
    private float timer;
    [SerializeField]private float duration,initDuration,speed;
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
        if (collision.CompareTag("Player") && once==false)
        {
            once = true;
            duration += Time.time;
            GameManager.instance.magnetOn = true;
            GameManager.instance.power.SetActive(true);
            spriteRenderer.enabled = false;
            cirCol.enabled = false;
        }
    }

    void LossPower()
    {
        GameManager.instance.magnetOn = false;
        once = false;
        GameManager.instance.power.SetActive(false);
        PoolManager.instance.poolDestroyObj(this.gameObject);
    }

}
