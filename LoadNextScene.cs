using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadNextScene : MonoBehaviour
{
    public string[] sceneOrder;   // kéo vào inspector theo thứ tự

    public void NextScene()
    {
        string current = SceneManager.GetActiveScene().name;

        int index = System.Array.IndexOf(sceneOrder, current);

        if (index == -1)
        {
            Debug.LogError("Scene không có trong sceneOrder list!");
            return;
        }

        int nextIndex = index + 1;

        if (nextIndex >= sceneOrder.Length)
        {
            StartCoroutine(LoadSceneWithFade("MainMenu"));
            return;
        }

        StartCoroutine(LoadSceneWithFade(sceneOrder[nextIndex]));
    }
    IEnumerator LoadSceneWithFade(string sceneName)
    {
        yield return new WaitForSeconds(0.6f);
        SceneManager.LoadScene(sceneName);
    }
}
