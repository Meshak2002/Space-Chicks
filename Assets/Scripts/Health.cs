using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private float health = 5;
    private float initHealth;
    private GameManager gameManager;

    private void Awake()
    {

    }

    void Start()
    {
        gameManager = GameManager.instance;
        initHealth = 5;
    }

    void Update()
    {
        if (GameManager.instance.gameState == GameState.Running)
        {
            if (health <= 0)
            {
                gameManager.Die(transform);

            }
        }
    }

    public void TakeDamage(int hitValue)
    {
        health -= hitValue;
    }

    public void RestoreHealth()
    {
        health = initHealth;
    }

    private void OnDisable()
    {
        RestoreHealth();
    }


}
