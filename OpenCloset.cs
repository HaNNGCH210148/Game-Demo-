using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenCloset : MonoBehaviour
{
    public Animator closetAnimator;
    public GameObject interactText;

    public AudioSource audioSource;   // audio source gắn vào tủ
    public AudioClip openSound;       // âm mở tủ
    public AudioClip closeSound;

    bool inRange = false;
    bool isOpened = false; // trạng thái thực tế của tủ

    void Start()
    {
        interactText.SetActive(false);
    }

    void Update()
    {
        if (inRange && Input.GetKeyDown(KeyCode.E))
        {
            if (!isOpened)
            {
                // MỞ TỦ
                closetAnimator.ResetTrigger("Close");
                closetAnimator.SetTrigger("Open");
                isOpened = true;

                if (audioSource != null && closeSound != null)
                    audioSource.PlayOneShot(closeSound);
            }
            else
            {
                // ĐÓNG TỦ
                closetAnimator.ResetTrigger("Open");
                closetAnimator.SetTrigger("Close");
                isOpened = false;

                if (audioSource != null && closeSound != null)
                    audioSource.PlayOneShot(closeSound);
            }

            UpdateUI();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inRange = true;
            interactText.SetActive(true);
            UpdateUI();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inRange = false;
            interactText.SetActive(false);
        }
    }

    void UpdateUI()
    {
        interactText.GetComponent<TMPro.TextMeshProUGUI>().text =
            isOpened ? "Press E to Close" : "Press E to Open";
    }
}
