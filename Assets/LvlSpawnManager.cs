using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LvlSpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject piece, chick, asteroid;
    [SerializeField] private float pieceSpawnRate, chickSpawnRate, asterSpawnRate;
    private bool piecePause, chickPause, asterPause;

    private Transform[] spawnPts;
    private Transform targetSpawnPt;
    public static LvlSpawnManager instance;

    private void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
            return;
        }    
        instance = this;
        spawnPts = GetComponentsInChildren<Transform>();
    }

    void Start()
    {
        targetSpawnPt = spawnPts[Random.Range(0, spawnPts.Length)];
        if (piece != null)
            StartCoroutine(GeneratePieces());
        if (chick != null)
            StartCoroutine(GenerateChicks());
        if(asteroid != null)
            StartCoroutine(GenerateAsteroid());
    }


    IEnumerator GeneratePieces()
    {
        while (piecePause == false)
        {
            yield return new WaitForSeconds(pieceSpawnRate);
            Instantiate(piece, targetSpawnPt.position, Quaternion.identity);
            targetSpawnPt = spawnPts[Random.Range(0, spawnPts.Length)];
        }
    }

    IEnumerator GenerateChicks()
    {
        while (chickPause == false)
        {
            yield return new WaitForSeconds(chickSpawnRate);
            Instantiate(chick, targetSpawnPt.position, Quaternion.identity);
            targetSpawnPt = spawnPts[Random.Range(0, spawnPts.Length)];
        }
    }

    IEnumerator GenerateAsteroid()
    {
        while (asterPause == false)
        {
            yield return new WaitForSeconds(asterSpawnRate);
            Instantiate(asteroid, targetSpawnPt.position, Quaternion.identity);
            targetSpawnPt = spawnPts[Random.Range(0, spawnPts.Length)];
        }
    }

    public IEnumerator Reward(Vector2 pos)
    {
        if (piece != null)
        {
            for (int i = 0; i < 6; i++)
            {
                Instantiate(piece, pos, Quaternion.identity);
                yield return new WaitForSeconds(.1f);
                if (i % 2 == 0)
                    pos.x += .4f;
                else
                    pos.x -= .6f;
            }
        }
    }

    public void Endgame()
    {
        StopAllCoroutines();
    }
}
