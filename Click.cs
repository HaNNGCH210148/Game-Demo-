using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Click : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip clickSound;

    public void PlayClick()
    {
        audioSource.pitch = 1.5f;
        if (audioSource != null && clickSound != null)
            audioSource.PlayOneShot(clickSound);
    }
}
