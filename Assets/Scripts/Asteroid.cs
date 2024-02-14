using UnityEngine;

public class Asteroid : MonoBehaviour
{
    private Vector2 target;
    private bool reached;
    [SerializeField] private float speed;

    void Start()
    {
        target = GameManager.instance.player.transform.position;    // Set target position to player's position
        PoolManager.instance.IncreaseSpeed += SpeedIncrease;        // Subscribe to event for speed increase
    }

    void Update()
    {
        if (GameManager.instance.gameState == GameState.Running)
        {
            if (target != null)
            {
                float dist = Vector2.Distance(transform.position, target);  // Calculate distance to target
                if (dist > .1f && reached == false)
                {       // Move towards target if not reached the player yet
                    transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);
                    reached = true;
                }
                else
                {   // Move downward and check boundary
                    transform.Translate(Vector2.down * speed * Time.deltaTime);
                    BoundaryCheck();
                }
            }
        }
    }

    void SpeedIncrease()
    {
        speed += 1;
    }

    void BoundaryCheck()
    {
        if (transform.position.y < -4.5f)       // Check if the piece's y position is below the lower boundary
        {
            PoolManager.instance.PoolDestroyObj(this.gameObject);
        }
    }
}
