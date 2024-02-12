using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameState gameState = GameState.Running;
    public GameObject playerBullet, chickBullet, playerExplosion, smokeExplosion , chickExplosion, player;
    public GameObject magnet, shield;
    public GameObject power;
    public bool magnetOn, shieldOn;
    public Transform playerFirePos;
    public int piecesCollected;

    private void Awake()
    {
        if(instance != null)
        {
            Destroy(this.gameObject);
            return;
        }
        instance = this;
    }


    public void Die(Transform obj)
    {
        if (obj.CompareTag("Player"))
        {
            PoolManager.instance.poolInstantiateObj(playerExplosion, obj.transform.position, obj.transform.rotation);
            Destroy(obj.gameObject);
            EndGame();
        }else if (obj.CompareTag("Asteroid"))
        {
            PoolManager.instance.poolInstantiateObj(smokeExplosion,obj.transform.position,obj.transform.rotation, ObjType.VFX);
            PoolManager.instance.poolInstantiateObj(shield, obj.transform.position,obj.transform.rotation, ObjType.VFX);
            PoolManager.instance.poolDestroyObj(obj.gameObject);
        }
        else if(obj.CompareTag("Chick"))
        {
            PoolManager.instance.poolInstantiateObj(chickExplosion, obj.transform.position, obj.transform.rotation, ObjType.VFX);
            Vector2 chickPos = obj.position;
            PoolManager.instance.poolDestroyObj(obj.gameObject);
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
        gameState = GameState.Paused;
        StartCoroutine(wait(1f));
    }

    IEnumerator wait(float delay)
    {
        yield return new WaitForSeconds(delay);
        UiManager.instance.EndGame();
    }
    
}
public enum GameState
{
    Paused,
    GameOver,
    Running
}
