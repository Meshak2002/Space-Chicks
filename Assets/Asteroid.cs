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
        if (Vector2.Distance(transform.position, target) > .1f && reached==false)
        {
            transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);
            reached = true;
        }
        else
        {
            transform.Translate(Vector2.down *speed * Time.deltaTime); 
        }
    }
}
