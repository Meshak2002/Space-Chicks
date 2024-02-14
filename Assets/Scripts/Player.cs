using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private  float speed;
    [SerializeField] private float fireRate=1;
    [SerializeField] private bool fireDelay;
    private Vector2 inputMovement;

    private void Start()
    {
        SwipeControl.OnSwipeInput += Move;              // Subscribe to SwipeControl swipe input event
    }


    void Move(Vector2 input)         // Method to handle player movement based on swipe input
    {
        inputMovement = input;  

        inputMovement = BoundaryCheck(inputMovement);    // Apply boundary check to the input movement vector

        transform.Translate(inputMovement);    
        if (fireDelay == false)
        {
            if (GameManager.instance.doubleGunOn)    // Check if double gun power is activated if so double fire, else single Fire.
            {
                StartCoroutine(Fire(GameManager.instance.playerFirePos.GetChild(0)));
                StartCoroutine(Fire(GameManager.instance.playerFirePos.GetChild(1)));
            }
            else
            {
                StartCoroutine(Fire(GameManager.instance.playerFirePos));
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (GameManager.instance.shieldOn)      // If shield is active, the collided object will be died
        {
            GameManager.instance.Die(collision.transform);
            return;
        }
        
        if (collision.gameObject.CompareTag("Chick") || collision.gameObject.CompareTag("Asteroid"))
        {                                               // if the collided object is a Chick or Asteroid , then the player will die
            GameManager.instance.Die(transform);
        }
    }

    Vector2 BoundaryCheck(Vector2 input)
    {
        Vector2 futurePos = (Vector2)transform.position+input*speed*Time.deltaTime;  //Calculate future Position based on the input
        
        //Rephrase the values if it crosses the boundary
        futurePos.x = Mathf.Clamp(futurePos.x,-2.2f, 2.2f);                        
        futurePos.y = Mathf.Clamp(futurePos.y,-4.5f, 3f);

        return futurePos-(Vector2)transform.position;   //Return the inputMovement
    }

    IEnumerator Fire(Transform target)       // Coroutine to handle firing
    {      
        fireDelay = true;
        if(PoolManager.instance != null)
            PoolManager.instance.PoolInstantiateObj(GameManager.instance.playerBullet, target.position, target.rotation, ObjType.Bullet);
        yield return new WaitForSeconds(fireRate);
        fireDelay = false;     
    }

    private void OnDestroy()
    {
        SwipeControl.OnSwipeInput -= Move;      // Unsubscribe from SwipeControl swipe input event
    }


}
