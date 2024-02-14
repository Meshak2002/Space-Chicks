using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameState gameState = GameState.Running;

    public GameObject player;
    public GameObject power, poolManager, bgScroller;
    public Transform playerFirePos;
    public int piecesCollected;

    [Header("Bullet")]
    public GameObject playerBullet;
    public GameObject chickBullet;

    [Header("Explosion")]
    public GameObject playerExplosion;
    public GameObject smokeExplosion;
    public GameObject chickExplosion;

    [Header("Special Pickups")]
    public GameObject magnet;
    public GameObject shield;
    public GameObject doubleGun;

    [HideInInspector] public bool magnetOn, shieldOn, doubleGunOn;

    private void Awake()
    {
        if(instance != null)          // Ensures only one instance of GameManager exists
        {   
            Destroy(this.gameObject);
            return;
        }
        instance = this;
    }

    public bool IsPowerOn()           // Checks if any power-up is active
    {
        if (!magnetOn && !shieldOn)
        {
            if (!doubleGunOn)
            {
                return false;
            }
        }
        return true;
    }


    public void Die(Transform obj)          // Handles object destruction upon death
    {
        if (obj.CompareTag("Player"))
        {
            // Instantiates player explosion and ends the game
            PoolManager.instance.PoolInstantiateObj(playerExplosion, obj.transform.position, obj.transform.rotation, ObjType.VFX);
            Destroy(obj.gameObject);
            EndGame();
        }
        else if (obj.CompareTag("Asteroid"))
        {
            // Handles asteroid destruction and potential power-up drop
            PoolManager.instance.PoolInstantiateObj(smokeExplosion,obj.transform.position,obj.transform.rotation, ObjType.VFX);
            if(!IsPowerOn())
                PoolManager.instance.PoolInstantiateObj(doubleGun, obj.transform.position,obj.transform.rotation, ObjType.VFX);
            PoolManager.instance.PoolDestroyObj(obj.gameObject);
        }
        else if(obj.CompareTag("Chick"))
        {
            // Handles chick destruction and potential reward
            PoolManager.instance.PoolInstantiateObj(chickExplosion, obj.transform.position, obj.transform.rotation, ObjType.VFX);
            Vector2 chickPos = obj.position;
            PoolManager.instance.PoolDestroyObj(obj.gameObject);
            if (PoolManager.instance!=null)
            {
                StartCoroutine(PoolManager.instance.Reward(chickPos));
            }
        }
        else
        {
            return;
        }
    }

    public void EndGame()        
    {
        gameState = GameState.Paused;       // Pauses game state
        StartCoroutine(wait(1f));
    }

    IEnumerator wait(float delay)         // Waits for a specified duration before ending
    {
        yield return new WaitForSeconds(delay);
        UiManager.instance.EndGame();
    }
    
}

// Enumeration for different game states
public enum GameState
{
    Paused,
    GameOver,
    Running
}
