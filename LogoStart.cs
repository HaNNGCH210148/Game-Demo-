using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LogoStart : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(StartLogo());
    }

    IEnumerator StartLogo()
    {
        yield return new WaitForSeconds(2.05f);
        SceneManager.LoadScene(1);
    }
}
