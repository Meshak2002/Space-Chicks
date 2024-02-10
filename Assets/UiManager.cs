using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class UiManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText, finalScoreText;
    [SerializeField] private GameObject gameStartUI, gameOverUI;
    private bool isRetry;
    public static UiManager instance;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
            return;
        }
        instance = this;

        if (PlayerPrefs.GetInt("isRetry") == 1)
        {
            OnPlay();
            PlayerPrefs.SetInt("isRetry", 0);
        }
    }

    void Update()
    {
        if (scoreText != null)
        {
            scoreText.text = GameManager.instance.piecesCollected.ToString();
        }
    }

    public void OnPlay()
    {
        gameStartUI.SetActive(false);
        gameOverUI.SetActive(false);
        GameManager.instance.player.SetActive(true);
        GameManager.instance.gameState = GameState.Running;
    }

    public void Retry()
    {
        SceneManager.LoadScene(0);
        isRetry = true;
        PlayerPrefs.SetInt("isRetry", 1);
    }

    public void EndGame()
    {
        gameOverUI.SetActive(true);
        finalScoreText.text = scoreText.text;
    }
}
