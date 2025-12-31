using System.Collections;
using UnityEngine;
using TMPro;

public class ClueNotiText : MonoBehaviour
{
    public TextMeshProUGUI interactText;     // text press E
    public GameObject Dialog;                // panel text
    public TextMeshProUGUI dialogText;       // text trong dialog
    public GameObject PlayerScript;          // player module
    public ClueManager clueManager;
    public AudioSource audioSource;
    public AudioClip readClueSound;

    [TextArea(3, 10)]
    public string clueContent;               // nội dung của clue

    private bool isPlayerNear = false;
    public static bool isReading = false;

    void Start()
    {
        //  thông báo nếu chưa kéo text vào
        if (interactText == null)
            interactText = GameObject.Find("NotiText").GetComponent<TextMeshProUGUI>();

        interactText.gameObject.SetActive(false);

        Dialog.SetActive(false);  // ẩn clue
    }

    void Update()
    {
        if (isPlayerNear && Input.GetKeyDown(KeyCode.E) && !isReading)
        {
            ReadClue();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            interactText.text = "Press E to Read";
            interactText.gameObject.SetActive(true);
            isPlayerNear = true;
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            interactText.gameObject.SetActive(false);
            isPlayerNear = false;
        }
    }

    void ReadClue()
    {
        if (audioSource != null && readClueSound != null)
            audioSource.PlayOneShot(readClueSound);
        isReading = true;

        // Ẩn thông báo
        interactText.gameObject.SetActive(false);

        // Khóa di chuyển
        PlayerScript.GetComponent<FirstPersonController>().enabled = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Hiện dialog và set nội dung
        Dialog.SetActive(true);
        dialogText.text = clueContent;
       


        // Ẩn tờ giấy trong game
        //gameObject.SetActive(false);
        var rend = GetComponent<Renderer>();
        if (rend != null) rend.enabled = false;
        foreach (var r in GetComponentsInChildren<Renderer>()) r.enabled = false;

        var col = GetComponent<Collider>();
        if (col != null) col.enabled = false;
        foreach (var c in GetComponentsInChildren<Collider>()) c.enabled = false;
    }

    public void ExitClue()
    {
        isReading = false;
        Dialog.SetActive(false);
        PlayerScript.GetComponent<FirstPersonController>().enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;


        if (clueManager != null)
            clueManager.NextClue();
    }
}
