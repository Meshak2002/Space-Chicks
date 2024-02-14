using System.Collections;
using UnityEngine;

public class Chicken : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float fireRate = 1;
    [SerializeField] private Vector2 targetDir;
    [SerializeField] private Transform firePT;
    private bool fireDelay;
    private int i;

    private void OnEnable()
    {
        fireDelay = false;
    }

    void Start()
    {
        i = Random.Range(0, 1);
        targetDir = (i == 0) ? Vector2.right : Vector2.left;    // Randomly choose initial movement direction
        targetDir += Vector2.down;

        PoolManager.instance.IncreaseSpeed += SpeedIncrease;     // Subscribe to event for speed increase
    }

    void Update()
    {
        if (GameManager.instance.gameState == GameState.Running && gameObject.activeSelf)
        {
            // Move the object and fire bullets
            targetDir = BoundaryCheck(targetDir);
            transform.Translate(targetDir * speed * Time.deltaTime);
            StartCoroutine(Fire());
        }
        
    }

    void SpeedIncrease()
    {
        speed += .7f;
    }

    Vector2 BoundaryCheck(Vector2 dir)
    {
        // Adjust direction if the object goes out of horizontal boundaries
        if (transform.position.x > 2.2f)
            dir = new Vector2(-1, -1);

        if (transform.position.x < -2.2f)
            dir = new Vector2(1, -1);

        if (transform.position.y < -4.5f)       // Check if the piece's y position is below the lower boundary
        {
            PoolManager.instance.PoolDestroyObj(this.gameObject);
        }

        return dir;
    }

    IEnumerator Fire()
    {
        if (fireDelay == false)
        {
            fireDelay = true;
            yield return new WaitForSeconds(fireRate);
            PoolManager.instance.PoolInstantiateObj(GameManager.instance.chickBullet, firePT.position, firePT.rotation, ObjType.Bullet);
            AudioManager.instance.EggSFxPlay();
            fireDelay = false;
        }
    }

}
