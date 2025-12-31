using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TakeObjectRound2 : MonoBehaviour
{
    [Header("UI")]
    public GameObject interactText;

    [Header("World Object (on table)")]
    public GameObject realObject;

    [Header("Object on Hand")]
    public GameObject fakeObject;

    [Header("Custom Prompt")]
    public string objectName = "Item"; 

    [Header("Audio")]
    public AudioSource audioSource;  
    public AudioClip pickupSound;     
    public AudioClip putbackSound;    

    bool inRange = false;
    bool isTaken = false;

    void Start()
    {
        interactText.SetActive(false);
        fakeObject.SetActive(false);
    }

    void Update()
    {
        if (inRange && Input.GetKeyDown(KeyCode.E))
        {
            ToggleObject();
        }
    }

    void ToggleObject()
    {
        if (!isTaken)
        {
            fakeObject.SetActive(true);
            realObject.SetActive(false);
            isTaken = true;

            if (audioSource != null && pickupSound != null)
                audioSource.PlayOneShot(pickupSound);
        }
        else
        {
            fakeObject.SetActive(false);
            realObject.SetActive(true);
            isTaken = false;

            if (audioSource != null && putbackSound != null)
                audioSource.PlayOneShot(putbackSound);
        }

        UpdateUI();
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
        interactText.GetComponent<TextMeshProUGUI>().text =
            isTaken ? $"Press E to Put Back {objectName}"
                    : $"Press E to Take {objectName}";
    }
}
