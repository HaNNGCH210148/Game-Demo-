using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public FirstPersonController player;

    private bool isPaused = false;
    void Start()
    {
        pauseMenuUI.SetActive(false); // ẨN KHI BẮT ĐẦU GAME
    }
    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused) ResumeGame();
            else PauseGame();
        }
    }

    public void PauseGame()
    {
        isPaused = true;

        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        FindObjectOfType<SetTime>()?.audioSource.Pause();

        FirstPersonController.isPauseMenuOpen = true;

        if (player != null)
        {
            player.playerCanMove = false;
            player.cameraCanMove = false;
        }

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void ResumeGame()
    {
        isPaused = false;

        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        FindObjectOfType<SetTime>()?.audioSource.UnPause();

        FirstPersonController.isPauseMenuOpen = false;

        if (player != null)
        {
            player.playerCanMove = true;
            player.cameraCanMove = true;
        }

        if (!ClueNotiText.isReading)   // chỉ khóa chuột khi không đọc clue
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    public void ExitGame()
    {
        Time.timeScale = 1f;
        StartCoroutine(BackToMenuSoundDelay());
    }

    IEnumerator BackToMenuSoundDelay()
    {
        AudioSource audio = GetComponent<AudioSource>();
        if (audio != null)
            audio.Play();

        yield return new WaitForSeconds(0.25f);

        SceneManager.LoadScene(1);
    }
}
