using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private GameManager gameManager;
    [SerializeField] private  float speed;
    [SerializeField] private float fireRate=1;
    private bool fireDelay;
    private Vector2 inputMovement;


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
            StartCoroutine(Fire());
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.gameObject.CompareTag("Chick") || collision.gameObject.CompareTag("Asteroid"))
        {
            gameManager.Die(transform);
        }
    }

    Vector2 BoundaryCheck(Vector2 input)
    {
        if (transform.position.x > 2.2f)
            input.x = input.x > 0 ? 0 : input.x;

        if (transform.position.x < -2.2f)
            input.x = input.x > 0 ? input.x : 0;

        if (transform.position.y > 3)
            input.y = input.y > 0 ? 0 : input.y;

        if (transform.position.y < -4.5f)
            input.y = input.y > 0 ? input.y : 0;

        return input;
    }

    IEnumerator Fire()
    {      
        fireDelay = true;
        Instantiate(gameManager.playerBullet, gameManager.playerFirePos.position, gameManager.playerFirePos.rotation);
        yield return new WaitForSeconds(fireRate);
        fireDelay = false;     
    }


    
}
