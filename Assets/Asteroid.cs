using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    private Vector2 target;
    private bool reached;
    [SerializeField] private float speed;
    void Start()
    {
        target = GameManager.instance.player.transform.position;
    }

    void Update()
    {
        if (GameManager.instance.gameState == GameState.Running)
        {
            if (target != null)
            {
                float dist = Vector2.Distance(transform.position, target);
                if (dist > .1f && reached == false)
                {
                    transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);
                    reached = true;
                }
                else
                {
                    transform.Translate(Vector2.down * speed * Time.deltaTime);
                    BoundaryCheck();
                }
            }
        }
    }

    void BoundaryCheck()
    {
        if (transform.position.y < -4.5f)
        {
            PoolManager.instance.poolDestroyObj(this.gameObject);
        }
    }
}
