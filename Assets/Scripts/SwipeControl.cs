using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeControl : MonoBehaviour
{
    bool isDragging;
    Vector2 startInPos;
    Vector2 endInPos;
    Vector2 offset;
    Vector2 direction;
    private GameManager gameManager;
    public delegate void SwipeInput(Vector2 input);
    public static event SwipeInput OnSwipeInput;

    private void OnEnable()
    {
        gameManager = GameManager.instance;
    }

    private void Start()
    {

    }

    void Update()
    {
        if (gameManager.gameState==GameState.Running)
        {
            CaptureInput();
        }
    }

    void CaptureInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isDragging = true;
            startInPos = Input.mousePosition;

        }
        else if (Input.GetMouseButton(0))
        {
            if(isDragging)
            {
                endInPos = Input.mousePosition;
                offset = endInPos - startInPos;
                direction = Vector2.ClampMagnitude(offset, 1);
                OnSwipeInput?.Invoke(direction);
                startInPos = endInPos;
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isDragging=false;
            direction= Vector2.zero;
            OnSwipeInput?.Invoke(direction);
        }
    }
}
