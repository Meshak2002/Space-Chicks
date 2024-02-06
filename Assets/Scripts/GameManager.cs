using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameState gameState = GameState.Running;
    public GameObject playerBullet, chickBullet, explosion;
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

}
public enum GameState
{
    Paused,
    GameOver,
    Running
}
