using UnityEngine;

public class DoubleGun : SpecialPower
{
    [SerializeField] private float duration, speed;
    private float initDuration;

    private void Awake()
    {
        initDuration = duration;
        cirCol = GetComponent<CircleCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        // Reset doubleGun state when enabled
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

        if (once)           // If the doubleGun power is activated, count down its duration
        {
            timer = duration - Time.time;
            if (timer <= 0) LossPower(ref GameManager.instance.doubleGunOn);
        }
        else               // If not, move it downward
        {
            BoundaryCheck();
            transform.Translate(Vector3.down * speed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player") && once == false)        // Check if collided with player 
        {
            ActivatePower(ref GameManager.instance.doubleGunOn,ref duration);
        }
    }

    void SpeedIncrease()
    {
        speed += 1;
    }



}
