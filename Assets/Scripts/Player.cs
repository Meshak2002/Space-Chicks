using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private GameManager gameManager;
    [SerializeField] private  float speed;
    [SerializeField] private float fireRate=1;
    [SerializeField] private bool fireDelay;
    private Vector2 inputMovement;


    private void Awake()
    {
        
    }

    private void Start()
    {
        gameManager = GameManager.instance;
        SwipeControl.OnSwipeInput += Move;
    }

    void Update()
    {
        if (gameManager != null)
        {
            if (gameManager.gameState == GameState.Running)
            {
                
            }
        }
    }

    void Move(Vector2 input )
    {
        inputMovement = input;
        inputMovement = BoundaryCheck(inputMovement);
        transform.Translate(inputMovement * speed * Time.deltaTime);
        if (fireDelay == false)
        {
            if (GameManager.instance.doubleGunOn)
            {
                StartCoroutine(Fire(gameManager.playerFirePos.GetChild(0)));
                StartCoroutine(Fire(gameManager.playerFirePos.GetChild(1)));
            }
            else
            {
                StartCoroutine(Fire(gameManager.playerFirePos));
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (gameManager.shieldOn)
        {
            gameManager.Die(collision.transform);
            return;
        }
        
        if (collision.gameObject.CompareTag("Chick") || collision.gameObject.CompareTag("Asteroid"))
        {
            gameManager.Die(transform);
        }
    }

    Vector2 BoundaryCheck(Vector2 input)
    {
        if (this.transform.position.x > 2.2f)
            input.x = input.x > 0 ? 0 : input.x;

        if (this.transform.position.x < -2.2f)
            input.x = input.x > 0 ? input.x : 0;

        if (this.transform.position.y > 3)
            input.y = input.y > 0 ? 0 : input.y;

        if (this.transform.position.y < -4.5f)
            input.y = input.y > 0 ? input.y : 0;

        return input;
    }

    IEnumerator Fire(Transform target)
    {      
        fireDelay = true;
        if(PoolManager.instance != null)
            PoolManager.instance.poolInstantiateObj(gameManager.playerBullet, target.position, target.rotation, ObjType.Bullet);
        yield return new WaitForSeconds(fireRate);
        fireDelay = false;     
    }

    private void OnDestroy()
    {
        SwipeControl.OnSwipeInput -= Move;
    }


}
