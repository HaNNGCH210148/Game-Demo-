using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StartStory : MonoBehaviour
{
    public GameObject StartStoryPanel;  
    public GameObject PlayerScript;     
    public GameObject StartImg;         
    public float showTime = 8f;         
    public float delayTime = 3f;

    private bool isShowing = false;

    void Start()
    {
        // ẩn player control
        if (PlayerScript != null)
            PlayerScript.GetComponent<FirstPersonController>().enabled = false;

        // Ẩn toàn bộ UI trước
        StartImg.SetActive(false);
        StartStoryPanel.SetActive(false);

        //  intro
        StartCoroutine(DelayedStartStory());
    }
    IEnumerator DelayedStartStory()
    {
        // Chờ đúng delayTime giây
        yield return new WaitForSeconds(delayTime);

        // Sau khi chờ xong → bật intro
        StartCoroutine(ShowIntroStory());
    }

    IEnumerator ShowIntroStory()
    {
        isShowing = true;

        StartImg.SetActive(true);
        StartStoryPanel.SetActive(true);

        
        yield return new WaitForSeconds(showTime);

        if (isShowing)
        {
            CloseIntro();
        }
    }

    public void CloseIntro()
    {
        isShowing = false;

        // Ẩn toàn bộ UI
        StartImg.SetActive(false);
        StartStoryPanel.SetActive(false);

        // Kích hoạt lại Player Control
        if (PlayerScript != null)
            PlayerScript.GetComponent<FirstPersonController>().enabled = true;
    }
}
