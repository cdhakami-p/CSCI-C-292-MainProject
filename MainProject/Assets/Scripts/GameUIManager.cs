using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;
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

    [SerializeField] private Rigidbody2D ball;
    [SerializeField] private Rigidbody2D playerBottom;
    [SerializeField] private Rigidbody2D playerTop;

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

        ResetBall();
        ResetPlayers();
        ResetPlayerAbilities();

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
        if (playerBottom != null && playerBottomSpawn != null)
        {
            playerBottom.linearVelocity = Vector2.zero;
            playerBottom.angularVelocity = 0f;
            playerBottom.transform.position = playerBottomSpawn.position;
            playerBottom.transform.rotation = Quaternion.identity;
        }
        if (playerTop != null && playerTopSpawn != null)
        {
            playerTop.linearVelocity = Vector2.zero;
            playerTop.angularVelocity = 0f;
            playerTop.transform.position = playerTopSpawn.position;
            playerTop.transform.rotation = Quaternion.Euler(0f, 0f, 180f);
        }
    }

    private void ResetPlayerAbilities()
    {
        if (playerBottom != null)
        {
            var bottomController = playerBottom.GetComponent<PlayerController>();
            if (bottomController != null)
                bottomController.ResetBoostCooldown();

            var bottomAbility = playerBottom.GetComponent<AbilityAC>();
            if (bottomAbility != null)
                bottomAbility.ResetAbilityCooldown();
        }

        if (playerTop != null)
        {
            var topController = playerTop.GetComponent<PlayerController>();
            if (topController != null)
                topController.ResetBoostCooldown();

            var topAbility = playerTop.GetComponent<AbilityAC>();
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
            playerTop = rb;
        } else
        {
            playerBottom = rb;
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
