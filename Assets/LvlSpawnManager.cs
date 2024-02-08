using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LvlSpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject piece, chick, asteroid;
    [SerializeField] private float pieceSpawnRate, chickSpawnRate, asterSpawnRate;

    public int piecesCollected;
    private bool delay;
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
    }

    void Start()
    {
        spawnPts = GetComponentsInChildren<Transform>();
        targetSpawnPt = spawnPts[Random.Range(0, spawnPts.Length)];
    }

    void Update()
    {
        if (delay == false)
        {
            StartCoroutine(GeneratePieces());
            delay = true;
        }
    }

    IEnumerator GeneratePieces()
    {
        yield return new WaitForSeconds(pieceSpawnRate);
        Instantiate(piece, targetSpawnPt.position,Quaternion.identity);
        targetSpawnPt = spawnPts[Random.Range(0, spawnPts.Length)];
        delay = false;
    }

    IEnumerator GenerateChicks()
    {

    }

    IEnumerator GenerateAsteroid()
    {

    }
}
