using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SetTime : MonoBehaviour
{

    [Header("Timer Settings")]
    public float roundTime = 60f;
    private float currentTime;

    [Header("Story Delay")]
    public float storyDelay = 8f; // thời gian chờ trước khi bắt đầu countdown
    private bool timerStarted = false;

    [Header("UI References")]
    public TextMeshProUGUI txtTimer;
    public GameObject gameOverUI;

    [Header("Fade Animation")]
    public Animator fadeAnimator;        // Animator chứa nextlevelfadein/out
    public float fadeDuration = 1f;      // thời gian animation

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip fadeSound;          // âm thanh fade

    [Header("Set Time Sound")]
    public AudioClip countdownSound;
    private bool[] played = new bool[6];

    [Header("Warning Effect (Last 5s)")]
    public Color normalColor = Color.white;   // màu bình thường
    public Color warningColor = Color.red;    // màu khi cảnh báo
    public float blinkInterval = 0.9f;        // tốc độ nháy 
    private bool isBlinking = false;

    private bool isGameOver = false;

    void Start()
    {
        currentTime = roundTime;
        gameOverUI.SetActive(false);
        if (fadeAnimator != null)
            fadeAnimator.updateMode = AnimatorUpdateMode.UnscaledTime;
        StartCoroutine(StartTimerAfterStory());
    }

    IEnumerator StartTimerAfterStory()
    {
        yield return new WaitForSeconds(storyDelay);
        timerStarted = true;
    }


    void Update()
    {
        if (!timerStarted || isGameOver) return;

        currentTime -= Time.deltaTime;

        int minutes = Mathf.FloorToInt(currentTime / 60f);
        int seconds = Mathf.FloorToInt(currentTime % 60f);

        txtTimer.text = $"{minutes:00}:{seconds:00}";

        if (!isBlinking && currentTime <= 6f && currentTime > 0f)
        {
            StartCoroutine(BlinkTimer());
        }

        if (currentTime <= 0)
        {
            GameOver();
        }
        PlayCountdownSound();

        if (currentTime <= 0)
        {
            GameOver();
        }
    }

    IEnumerator BlinkTimer()
    {
        isBlinking = true;

        bool isRed = false;

        while (!isGameOver && currentTime > 0f && currentTime <= 5f)
        {
            // đổi màu giữa normal ↔ warning
            txtTimer.color = isRed ? normalColor : warningColor;
            isRed = !isRed;

            yield return new WaitForSeconds(blinkInterval);
        }

        //  trả về màu bình thường
        txtTimer.color = normalColor;
        isBlinking = false;
    }


    void PlayCountdownSound()
    {
        int timeInt = Mathf.CeilToInt(currentTime);

        if (timeInt <= 5 && timeInt >= 1)
        {
            if (!played[timeInt]) // chưa phát trong giây này
            {
                if (audioSource != null && countdownSound != null)
                    audioSource.PlayOneShot(countdownSound);

                played[timeInt] = true;
            }
        }
    }

    void GameOver()
    {
        

        isGameOver = true;

            audioSource?.Stop();

        if (gameOverUI != null && !gameOverUI.activeSelf)
            gameOverUI.SetActive(true);

        if (fadeAnimator != null)
            fadeAnimator.Play("GameOverOut");

        if (audioSource != null && fadeSound != null)
            audioSource.PlayOneShot(fadeSound);

        StartCoroutine(ShowGameOverPanel());
    }

    IEnumerator ShowGameOverPanel()
    {
        yield return new WaitForSecondsRealtime(fadeDuration);

        Time.timeScale = 0f;

        // disable player control
        var fpc = FindObjectOfType<FirstPersonController>();
        if (fpc != null)
        {
            fpc.playerCanMove = false;
            fpc.cameraCanMove = false;
        }

        gameOverUI.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ExitToMainMenu()
    {
        Time.timeScale = 1f;
        StartCoroutine(FadeAndLoadScene("MainMenu"));
    }

    public void RestartRound()
    {
        Time.timeScale = 1f;
        StartCoroutine(FadeAndLoadScene(SceneManager.GetActiveScene().name));
    }

    IEnumerator FadeAndLoadScene(string sceneName)
    {
        Time.timeScale = 1f;
       
        if (fadeAnimator != null)
            fadeAnimator.Play("GameOverIn");

        if (audioSource != null && fadeSound != null)
            audioSource.PlayOneShot(fadeSound);

        yield return new WaitForSeconds(fadeDuration);

        SceneManager.LoadScene(sceneName);
    }

}
