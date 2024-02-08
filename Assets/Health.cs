using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private float health = 5;
    private GameManager gameManager;

    void Start()
    {
        gameManager = GameManager.instance;
    }

    void Update()
    {
        if (health <= 0)
        {
            gameManager.Die(transform);
           
        }
    }

    public void TakeDamage(int hitValue)
    {
        Debug.Log("asdasd"+health);
        health -= hitValue;
    }


}
