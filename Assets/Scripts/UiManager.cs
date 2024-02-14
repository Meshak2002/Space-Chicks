using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class UiManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText, finalScoreText, HiScoreText;
    [SerializeField] private GameObject gameStartUI, gameOverUI;

    public static UiManager instance;

    void Awake()
    {
        // Ensuring only one instance of UiManager exists
        if (instance != null)
        {
            Destroy(this.gameObject);
            return;
        }
        instance = this;

        // Checking if the game is being retried
        if (PlayerPrefs.GetInt("isRetry") == 1)
        {
            OnPlay();
            PlayerPrefs.SetInt("isRetry", 0);
        }
    }

    void Update()
    {
        if (scoreText != null)
        {                                   //// Display the number of pieces collected in real-time
            scoreText.text = GameManager.instance.piecesCollected.ToString();
        }
    }

    public void OnPlay()
    {
        gameStartUI.SetActive(false);
        gameOverUI.SetActive(false);
        GameManager.instance.poolManager.SetActive(true);
        GameManager.instance.bgScroller.SetActive(true);
        GameManager.instance.gameState = GameState.Running;
    }

    public void OnRetry()                   // Called when the retry button is pressed
    {               
        SceneManager.LoadScene(0);
        PlayerPrefs.SetInt("isRetry", 1);
    }

    public void EndGame()                   // Called when the game ends
    {
        gameOverUI.SetActive(true);
        finalScoreText.text = scoreText.text;
                                            
        if (GameManager.instance.piecesCollected > PlayerPrefs.GetInt("HiScore"))   // Check if the player's score is a new high score
        {
            HiScoreText.text = scoreText.text;
            PlayerPrefs.SetInt("HiScore", GameManager.instance.piecesCollected);
        }
        else
        {
            HiScoreText.text = PlayerPrefs.GetInt("HiScore").ToString();
        }
    }
}
