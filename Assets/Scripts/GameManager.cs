using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameState gameState = GameState.Running;
    public GameObject playerBullet, chickBullet, playerExplosion, smokeExplosion , chickExplosion, player;
    public Transform playerFirePos,chickFirePos;

    private void Awake()
    {
        if(instance != null)
        {
            Destroy(this.gameObject);
            return;
        }
        instance = this;
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void Die(Transform obj)
    {
        if (obj.CompareTag("Player"))
        {
            Instantiate(playerExplosion, obj.transform.position, obj.transform.rotation);
            Destroy(obj.gameObject);
            EndGame();
        }else if (obj.CompareTag("Asteroid"))
        {
            Instantiate(smokeExplosion, obj.transform.position, obj.transform.rotation);
            Destroy(obj.gameObject);
        }
        else
        {
            Instantiate(chickExplosion, obj.transform.position, obj.transform.rotation);
            Destroy(obj.gameObject);
        }
    }

    public void EndGame()
    {
        gameState = GameState.GameOver;
    }
}
public enum GameState
{
    Paused,
    GameOver,
    Running
}
