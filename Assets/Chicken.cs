using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chicken : MonoBehaviour
{
    private GameManager gameManager;
    [SerializeField] private float speed;
    [SerializeField] private float fireRate = 1;
    private bool fireDelay;
    [SerializeField] private Vector2 targetDir;
    private int i;

    void Start()
    {
        gameManager = GameManager.instance;
        i = UnityEngine.Random.Range(0, 1);
        if(i == 0)
        {
            targetDir = Vector2.right;
        }
        else
        {
            targetDir = Vector2.left;
        }
        targetDir += Vector2.down;
    }

    void Update()
    {
        targetDir = BoundaryCheck(targetDir);
        transform.Translate(targetDir*speed*Time.deltaTime);
        StartCoroutine(Fire());
    }

    Vector2 BoundaryCheck(Vector2 dir)
    {
        if (transform.position.x > 2.2f)
            dir = new Vector2(-1, -1);

        if (transform.position.x < -2.2f)
            dir = new Vector2(1, -1);

        if (transform.position.y < -4.5f)
            Destroy(gameObject);

        return dir;
    }

    IEnumerator Fire()
    {
        if (fireDelay == false)
        {
            fireDelay = true;
            yield return new WaitForSeconds(fireRate);
            Instantiate(gameManager.chickBullet, gameManager.chickFirePos.position, gameManager.chickFirePos.rotation);
            fireDelay = false;
        }
    }
}
