using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed=20;
    [SerializeField] private Vector2 direction = Vector2.right;

    private void OnEnable()
    {
        
    }

    void Start()
    {

    }

    void Update()
    {
        transform.Translate(direction*speed*Time.deltaTime);    //   Fire/Move Forwards

        DestroyOnBoundary();
    }

    void DestroyOnBoundary()
    {
        if (transform.position.y > 6 || transform.position.y < -6)
        {
            Destroy(gameObject);
        }
    }

}
