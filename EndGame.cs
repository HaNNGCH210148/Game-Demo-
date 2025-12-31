using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using System.Collections;

public class ShowEndUI : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public GameObject endUI;   

    private void Start()
    {
        // Ẩn UI lúc đầu
        if (endUI != null)
            endUI.SetActive(false);

        //  sự kiện khi video kết thúc
        videoPlayer.loopPointReached += OnVideoEnd;
    }

    private void OnVideoEnd(VideoPlayer vp)
    {
        
        endUI.SetActive(true);
    }

    // Gọi từ button Quit Game
    public void QuitGame()
    {


        StartCoroutine(QuitSoundDelay());
    }

    IEnumerator QuitSoundDelay()
    {
        // phát âm nút
        AudioSource audio = GetComponent<AudioSource>();
        if (audio != null)
            audio.Play();

        // đợi âm phát
        yield return new WaitForSeconds(0.25f);

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    // Gọi từ button MainMenu
    public void BackToMainMenu()
    {
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
