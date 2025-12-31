using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("BedRoom");
    }

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
}
