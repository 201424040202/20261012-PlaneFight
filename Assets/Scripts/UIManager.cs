using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    private static UIManager _instance;
    public static UIManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindAnyObjectByType<UIManager>();
            }
            return _instance;
        }
    }

    public TextMeshProUGUI scoreTMP;
    public TextMeshProUGUI bombCountTMP;
    public TextMeshProUGUI lifeCountTMP;

    public Button pauseButton;
    public Button resumeButton;

    public GameObject gameOverPanel;
    public TextMeshProUGUI bestScoreTMP;
    public TextMeshProUGUI currentScoreTMP;

    public Button restartButton;
    public Button quitButton;

    public void Start()
    {
        pauseButton.onClick.RemoveListener(this.OnPauseButtonClick);
        resumeButton.onClick.RemoveListener(this.OnResumeButtonClick);

        pauseButton.onClick.AddListener(this.OnPauseButtonClick);
        resumeButton.onClick.AddListener(this.OnResumeButtonClick);

        restartButton.onClick.RemoveListener(this.OnRestartButtonClick);
        quitButton.onClick.RemoveListener(this.OnQuitButtonClick);

        restartButton.onClick.AddListener(this.OnRestartButtonClick);
        quitButton.onClick.AddListener(this.OnQuitButtonClick);
    }

    public void UpdateScoreUI(int score)
    {
        this.scoreTMP.text = score + "";
    }

    public void UpdateBombCountUI(int count)
    {
        this.bombCountTMP.text = count + "";
    }

    public void UpdateLifeCountUI(int count)
    {
        this.lifeCountTMP.text = count + "";
    }

    void OnPauseButtonClick()
    {
        pauseButton.gameObject.SetActive(false);
        resumeButton.gameObject.SetActive(true);
        GameManager.Instance.PauseGame();
        AudioManager.Instance.PlayButtonClip();
    }
    void OnResumeButtonClick()
    {
        pauseButton.gameObject.SetActive(true);
        resumeButton.gameObject.SetActive(false);
        GameManager.Instance.ResumeGame();
        AudioManager.Instance.PlayButtonClip();
    }

    // 显示游戏结束界面
    public void ShowGameOverPanel(int bestScore, int currentScore)
    {
        // 显示面板
        gameOverPanel.SetActive(true);
        this.bestScoreTMP.text = bestScore + "";
        this.currentScoreTMP.text = currentScore + "";

    }

    void OnRestartButtonClick()
    {
        GameManager.Instance.RestartGame();
        AudioManager.Instance.PlayButtonClip();
    }

    void OnQuitButtonClick()
    {
        GameManager.Instance.QuitGame();
    }

}
