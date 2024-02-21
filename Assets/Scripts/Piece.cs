using UnityEngine;

public class Piece : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private GameObject bone,piece;
    private BoxCollider2D boxCol;
    private float initSpeed;
    private bool isEaten;

    private void Awake()
    {
        boxCol = GetComponent<BoxCollider2D>();
        initSpeed = speed;
    }

    private void OnEnable()
    {
        // Reset speed to initial speed when object is enabled
        speed = initSpeed;
    }

    private void Start()
    {
        PoolManager.instance.IncreaseSpeed += SpeedIncrease;     // Subscribe to event for speed increase
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))             // Check if collided with player 
        {
            EatPiece();
        }
    }

    void Update()
    {
        if (GameManager.instance.gameState != GameState.Running)
            return;

        BoundaryCheck();    // Check if the piece is within the bounds

        if (GameManager.instance.magnetOn && isEaten==false)  // Move the piece towards the player if magnet is active and piece has not been eaten
        {
            transform.position = Vector2.MoveTowards(transform.position, GameManager.instance.player.transform.position , speed * Time.deltaTime);
        }
        else
        {
            transform.Translate(Vector3.down * speed * Time.deltaTime);
        }
    }

    void SpeedIncrease()
    {
        initSpeed += 1.5f;
    }

    void EatPiece()
    {
        GameManager.instance.piecesCollected++;
        AudioManager.instance.EatSFxPlay();
        piece.SetActive(false);
        bone.SetActive(true);
        boxCol.enabled = false;
        speed *= 2f;
        isEaten = true;
    }

    void RestorePiece()
    {
        piece.SetActive(true);
        bone.SetActive(false);
        isEaten = false;
        boxCol.enabled = true;
    }

    void BoundaryCheck()
    {
        if (transform.position.y < -4.5f)    // Check if the piece's y position is below the lower boundary
        {
            RestorePiece();
            PoolManager.instance.PoolDestroyObj(this.gameObject);
        }
    }
}
