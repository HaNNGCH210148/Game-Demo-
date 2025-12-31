using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Footstep : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip walkClip;
    public float walkPitch = 1f;      // tốc độ âm đi bộ
    public float runPitch = 1.5f;     // tốc độ âm chạy (nhanh hơn)

    void Update()
    {
        bool isPressingMoveKey =
            Input.GetKey(KeyCode.W) ||
            Input.GetKey(KeyCode.A) ||
            Input.GetKey(KeyCode.S) ||
            Input.GetKey(KeyCode.D);

        bool isRunning = Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.LeftShift);

        if (isPressingMoveKey)
        {
            if (isRunning)
            {
                audioSource.pitch = runPitch;
            }
            else
            {
                audioSource.pitch = walkPitch;
            }

            if (!audioSource.isPlaying)
            {
                audioSource.clip = walkClip;
                audioSource.loop = true;
                audioSource.Play();
            }
        }
        else
        {
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
            }
        }
    }
}
