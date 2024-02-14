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
        PoolManager.instance.IncreaseSpeed += SpeedIncrease;        // Subscribe to event for speed increase
    }

    void Update()
    {
        if (GameManager.instance.gameState == GameState.Running)
        {
            if (rImg != null)
            {                        // Update UV offset to create scrolling effect
                rImg.uvRect = new Rect(rImg.uvRect.position + movVelocity * Time.deltaTime, rImg.uvRect.size);
            }
        }
    }

    void SpeedIncrease()
    {
        movVelocity.y += .1f;
    }
}
