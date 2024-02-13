using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BG_Scroller : MonoBehaviour
{
    private RawImage rImg;
    [SerializeField] private Vector2 movVelocity;

    private void OnEnable()
    {
        rImg = GetComponent<RawImage>();    
    }

    void Start()
    {
        PoolManager.instance.IncreaseSpeed += SpeedIncrease;
    }

    void Update()
    {
        if (GameManager.instance.gameState == GameState.Running)
        {
            if (rImg != null)
            {
                rImg.uvRect = new Rect(rImg.uvRect.position + movVelocity * Time.deltaTime, rImg.uvRect.size);
            }
        }
    }

    void SpeedIncrease()
    {
        movVelocity.y += .1f;
    }
}
