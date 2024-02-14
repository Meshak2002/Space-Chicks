using UnityEngine;

public class SwipeControl : MonoBehaviour
{
    private bool isDragging;
    private Vector2 startInPos;
    private Vector2 endInPos;
    private Vector2 offset;
    private Vector2 direction;
    private GameManager gameManager;

    public delegate void SwipeInput(Vector2 input);
    public static event SwipeInput OnSwipeInput;

    private void OnEnable()
    {
        gameManager = GameManager.instance;
    }

    void Update()
    {
        // Check if the game is running
        if (gameManager.gameState == GameState.Running)
        {
            CaptureInput(); // Capture swipe input
        }
    }

    void CaptureInput()
    {
        // Check for mouse button down
        if (Input.GetMouseButtonDown(0))
        {
            isDragging = true;
            startInPos = Input.mousePosition;
        }
        // Check for mouse button held down
        else if (Input.GetMouseButton(0))
        {
            if (isDragging)
            {
                endInPos = Input.mousePosition;
                offset = endInPos - startInPos;
                direction = Vector2.ClampMagnitude(offset, 5);
                OnSwipeInput?.Invoke(direction);
                startInPos = endInPos;
            }
        }
        // Check for mouse button release
        else if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
            direction = Vector2.zero;
            OnSwipeInput?.Invoke(direction);
        }
    }
}
