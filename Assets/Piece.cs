using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private GameObject bone,piece;
    private BoxCollider2D boxCol;
    private float initSpeed;

    private void Awake()
    {
        boxCol = GetComponent<BoxCollider2D>();
        initSpeed = speed;
    }

    private void OnEnable()
    {
         speed = initSpeed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            EatPiece();
        }
    }

    void Update()
    {
        if (GameManager.instance.gameState == GameState.Running)
        {
            transform.Translate(Vector3.down * speed * Time.deltaTime);
            BoundaryCheck();
        }
         
    }

    void EatPiece()
    {
        GameManager.instance.piecesCollected++;
        piece.SetActive(false);
        bone.SetActive(true);
        boxCol.enabled = false;
        speed *= 2f;
    }

    void RestorePiece()
    {
        piece.SetActive(true);
        bone.SetActive(false);
        boxCol.enabled = true;
    }

    void BoundaryCheck()
    {
        if (transform.position.y < -4.5f)
        {
            RestorePiece();
            PoolManager.instance.poolDestroyObj(this.gameObject);
        }
    }
}
