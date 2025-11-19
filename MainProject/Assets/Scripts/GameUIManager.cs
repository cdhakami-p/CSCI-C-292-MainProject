using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering;

public class GameUIManager : MonoBehaviour
{

    [SerializeField] private TMP_Text timerText;
    [SerializeField] private TMP_Text scoreText;

    [SerializeField] private Button pauseButton;
    [SerializeField] private GameObject pausePanel;

    [SerializeField] private TMP_Text countdownText;

    [SerializeField] private string mainMenuSceneName = "MainMenu";

    [SerializeField] private float matchDuration = 180f;

    [SerializeField] private Transform ballSpawn;

    [SerializeField] private Transform playerBottomSpawn;
    [SerializeField] private Transform playerTopSpawn;

    [SerializeField] private Transform[] bottomPlayerSpawns;
    [SerializeField] private Transform[] topPlayerSpawns;

    [SerializeField] private Rigidbody2D ball;

    [SerializeField] private Rigidbody2D playerBottom;
    [SerializeField] private Rigidbody2D playerTop;

    [SerializeField] private GoalLine topGoalLine;
    [SerializeField] private GoalLine bottomGoalLine;

    [SerializeField] private AudioClip countdownSFX;
    [SerializeField] private AudioClip goalSFX;

    private List<Rigidbody2D> bottomPlayers = new List<Rigidbody2D>();
    private List<Rigidbody2D> topPlayers = new List<Rigidbody2D>();

    private bool isPaused = false;
    private bool gameActive = false;
    private float timeRemaining;

    private int bottomScore = 0;
    private int topScore = 0;

    private bool gameOver = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        timeRemaining = matchDuration;
        UpdateTimerUI();

        if (pausePanel != null)
        {
            pausePanel.SetActive(false);
        }

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.StopMusic();
        }

        Time.timeScale = 0f;
        StartCoroutine(StartCountdown());

        if (pauseButton != null)
        {
            pauseButton.onClick.AddListener(TogglePause);
        }

        if (scoreText != null)
        {
            scoreText.text = "0 - 0";
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (gameActive && !isPaused)
        {
            timeRemaining -= Time.unscaledDeltaTime;

            if (timeRemaining <= 0f && !gameOver)
            {
                timeRemaining = 0f;
                UpdateTimerUI();

                StartCoroutine(GameOver());
                gameOver = true;

                return;
            }

            UpdateTimerUI();
        }
    }

    private IEnumerator StartCountdown()
    {
        yield return new WaitForSecondsRealtime(0.5f);

        if (countdownText != null)
        {
            countdownText.gameObject.SetActive(true);
        }

        if (AudioManager.Instance != null && countdownSFX != null)
        {
            AudioManager.Instance.PlaySFX(countdownSFX);
        }

        int count = 3;
        while (count > 0)
        {
            if (countdownText != null)
            {
                countdownText.text = count.ToString();
            }

            yield return new WaitForSecondsRealtime(1f);
            count--;
        }

        if (countdownText != null)
        {
            countdownText.text = "Go!";
        }

        yield return new WaitForSecondsRealtime(0.5f);

        if (countdownText != null)
        {
            countdownText.gameObject.SetActive(false);
        }

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayMusic();
        }

        Time.timeScale = 1f;
        gameActive = true;
    }

    private void UpdateTimerUI()
    {
        if (timerText == null) return;

        int totalSeconds = Mathf.FloorToInt(timeRemaining);
        int minutes = totalSeconds / 60;
        int seconds = totalSeconds % 60;

        timerText.text = string.Format("{0}:{1:00}", minutes, seconds);
    }

    public void TogglePause()
    {
        if (gameOver) return;

        if (!gameActive) return;
        isPaused = !isPaused;

        if (isPaused)
        {
            Time.timeScale = 0f;
            if (pausePanel != null)
            {
                pausePanel.SetActive(true);
            }
        } else
        {
            Time.timeScale = 1f;
            if (pausePanel != null)
            {
                pausePanel.SetActive(false);
            }
        }
    }

    public void OnResume()
    {
        if (!gameActive) return;

        isPaused = false;
        Time.timeScale = 1f;
        if (pausePanel != null)
        {
            pausePanel.SetActive(false);
        }
    }

    public void OnQuit()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(mainMenuSceneName);
    }

    public void GoalScored(bool scoredOnTop)
    {
        if (gameOver) return;

        if(scoredOnTop)
        {
            bottomScore++;
        } else
        {
            topScore++;
        }
        
        if (scoreText != null)
        {
            scoreText.text = string.Format("{0} - {1}", bottomScore, topScore);
        }

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.StopMusic();
            if (goalSFX != null)
            {
                AudioManager.Instance.PlaySFX(goalSFX);
            }
        }

        StartCoroutine(Reset());
    }

    private IEnumerator Reset()
    {
        gameActive = false;
        isPaused = true;

        if (countdownText != null)
        {
            countdownText.gameObject.SetActive(true);
            countdownText.text = "Goal!";
        }

        yield return new WaitForSecondsRealtime(2f);

        Time.timeScale = 0f;
        countdownText.gameObject.SetActive(false);

        ResetPlayerAbilities();
        ResetBall();
        ResetPlayers();

        if (topGoalLine != null)
        {
            topGoalLine.SetCanScore(true);
        }

        if (bottomGoalLine != null)
        {
            bottomGoalLine.SetCanScore(true);
        }

        yield return new WaitForSecondsRealtime(0.5f);
        yield return StartCoroutine(StartCountdown());

        isPaused = false;
    }

    private void ResetBall()
    {
        if (ball != null && ballSpawn != null)
        {
            ball.linearVelocity = Vector2.zero;
            ball.angularVelocity = 0f;
            ball.transform.position = ballSpawn.position;
        }
    }

    private void ResetPlayers()
    {
        for (int i = 0; i < bottomPlayers.Count; i++)
        {
            var rb = bottomPlayers[i];
            if (rb == null) continue;

            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;

            if (bottomPlayerSpawns != null && i < bottomPlayerSpawns.Length && bottomPlayerSpawns[i] != null)
            {
                rb.transform.position = bottomPlayerSpawns[i].position;
                rb.transform.rotation = bottomPlayerSpawns[i].rotation;
            }
            else if (playerBottomSpawn != null && i == 0)
            {
                rb.transform.position = playerBottomSpawn.position;
                rb.transform.rotation = Quaternion.identity;
            }
        }

        for (int i = 0; i < topPlayers.Count; i++)
        {
            var rb = topPlayers[i];
            if (rb == null) continue;

            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;

            if (topPlayerSpawns != null && i < topPlayerSpawns.Length && topPlayerSpawns[i] != null)
            {
                rb.transform.position = topPlayerSpawns[i].position;
                rb.transform.rotation = Quaternion.Euler(0f, 0f, 180f);
            }
            else if (playerTopSpawn != null && i == 0)
            {
                rb.transform.position = playerTopSpawn.position;
                rb.transform.rotation = Quaternion.Euler(0f, 0f, 180f);
            }
        }
    }

    private void ResetPlayerAbilities()
    {
        foreach (var rb in bottomPlayers)
        {
            if (rb == null) continue;

            var bottomController = rb.GetComponent<PlayerController>();
            if (bottomController != null)
                bottomController.ResetBoostCooldown();

            var bottomAbility = rb.GetComponent<AbilityAC>();
            if (bottomAbility != null)
                bottomAbility.ResetAbilityCooldown();
        }

        foreach (var rb in topPlayers)
        {
            if (rb == null) continue;

            var topController = rb.GetComponent<PlayerController>();
            if (topController != null)
                topController.ResetBoostCooldown();

            var topAbility = rb.GetComponent<AbilityAC>();
            if (topAbility != null)
                topAbility.ResetAbilityCooldown();
        }
    }

    public void RegisterBall(Rigidbody2D rb)
    {
        ball = rb;
    }

    public void RegisterPlayers(bool isTopPlayer, Rigidbody2D rb)
    {
        if (isTopPlayer)
        {
            topPlayers.Add(rb);
        } else
        {
            bottomPlayers.Add(rb);
        }
    }

    private IEnumerator GameOver()
    {
        gameActive = false;
        isPaused = true;

        if (countdownText != null)
        {
            countdownText.gameObject.SetActive(true);
            countdownText.text = "Game Over!";
        }

        yield return new WaitForSecondsRealtime(3f);

        Time.timeScale = 1f;

        SceneManager.LoadScene(mainMenuSceneName);
    }
}
