using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private float speed;
    [SerializeField] private GameObject bone,piece;
    private BoxCollider2D boxCol;

    private void Awake()
    {
        boxCol = GetComponent<BoxCollider2D>();    
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
        transform.Translate(Vector3.down * speed * Time.deltaTime);
        BoundaryCheck(transform.position);
    }

    void EatPiece()
    {
        GameManager.instance.piecesCollected++;
        piece.SetActive(false);
        bone.SetActive(true);
        boxCol.enabled = false;
        speed *= 2f;
    }

    void BoundaryCheck(Vector2 position)
    {
        if (transform.position.y < -4.5f)
            Destroy(gameObject);
    }
}
