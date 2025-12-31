using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class ClueManager : MonoBehaviour
{
    [Header("Clues (sắp xếp thứ tự từ 1 → n)")]
    public List<GameObject> clues = new List<GameObject>();

    [Header("Next Level UI")]
    public GameObject nextLevelPanel;
    public Animator nextLevelAnimator;
    public Button nextButton;
    public Button exitButton;


    [Header("Audio")]
    public AudioSource clueAudioSource;
    public AudioSource panelAudioSource;
    public AudioClip clueSound;
    public AudioClip nextLevelSound;

    private int currentClue = 0;   // Clue bắt đầu từ index 0
    private bool panelShown = false;

    void Awake()
    {
        if (nextLevelPanel == null)
            Debug.LogError("NextLevelPanel chưa kéo vào!", this);

        if (nextLevelAnimator == null && nextLevelPanel != null)
            nextLevelAnimator = nextLevelPanel.GetComponent<Animator>();
    }

    void Start()
    {
        // Tắt toàn bộ + bật clue đầu tiên
        for (int i = 0; i < clues.Count; i++)
            clues[i].SetActive(i == 0);

        if (nextLevelPanel != null)
            nextLevelPanel.SetActive(false);

        if (nextButton != null)
            nextButton.onClick.AddListener(LoadNextScene);

        if (exitButton != null)
            exitButton.onClick.AddListener(() => StartCoroutine(LoadSceneWithFade("MainMenu")));

        var listener = FindObjectOfType<AudioListener>();
        if (listener == null)
            Debug.LogWarning("Không tìm thấy AudioListener trong scene. Gắn AudioListener lên Main Camera hoặc Player để nghe âm thanh.");
    }

    public void NextClue()
    {
        PlaySound(clueAudioSource, clueSound, "ClueSound");

        // Tắt clue cũ
        if (currentClue < clues.Count)
            clues[currentClue].SetActive(false);

        currentClue++;

        // còn clue → bật tiếp
        if (currentClue < clues.Count)
        {
            clues[currentClue].SetActive(true);
        }
        else
        {
            if (!panelShown)
                ShowNextLevelPanel();
        }

        // Save game
        var player = GameObject.FindWithTag("Player");
        var data = SaveData.FromSceneAndTransform(
            SceneManager.GetActiveScene().buildIndex,
            currentClue,
            player ? player.transform : null
        );
        SaveManager.SaveGame(data);
    }

    void ShowNextLevelPanel()
    {
        panelShown = true;
        nextLevelPanel.SetActive(true);

        // lock player
        var player = GameObject.FindWithTag("Player");
        var fpc = player?.GetComponent<FirstPersonController>();
        if (fpc) fpc.enabled = false;

        bool isLastScene = SceneManager.GetActiveScene().buildIndex ==
                           SceneManager.sceneCountInBuildSettings - 1;

        if (isLastScene)
        {
            // ẩn nút
            nextButton?.gameObject.SetActive(false);
            exitButton?.gameObject.SetActive(false);

            StartCoroutine(LastSceneFadeRoutine());
        }
        else
        {
            nextLevelAnimator?.Play("NextLevelFadeIn", 0, 0f);

            PlaySound(panelAudioSource, nextLevelSound, "NextLevelSound");

            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    IEnumerator LastSceneFadeRoutine()
    {
        if (!nextLevelAnimator) yield break;

        float fadeIn = 4f;
        float fadeOut = 0.6f;

        foreach (var clip in nextLevelAnimator.runtimeAnimatorController.animationClips)
        {
            if (clip.name == "NextLevelFadeIn") fadeIn = clip.length;
            if (clip.name == "NextLevelFadeOut") fadeOut = clip.length;
        }

        nextLevelAnimator.Play("NextLevelFadeIn");
        yield return new WaitForSeconds(fadeIn + 0.05f);

        nextLevelAnimator.Play("NextLevelFadeOut");
        yield return new WaitForSeconds(fadeOut + 0.05f);

        nextLevelPanel.SetActive(false);

        var player = GameObject.FindWithTag("Player");
        var fpc = player?.GetComponent<FirstPersonController>();
        if (fpc) fpc.enabled = true;
    }

    public void LoadNextScene()
    {
        int nextIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextIndex >= SceneManager.sceneCountInBuildSettings)
        {
            Debug.Log("Không còn scene tiếp theo.");
            return;
        }

        string nextSceneName = System.IO.Path.GetFileNameWithoutExtension(
            SceneUtility.GetScenePathByBuildIndex(nextIndex)
        );

        StartCoroutine(LoadSceneWithFade(nextSceneName));
    }

    IEnumerator LoadSceneWithFade(string scene)
    {
        if (nextLevelAnimator)
        {
            nextLevelAnimator.Play("NextLevelFadeOut");

            float wait = 0.6f;
            foreach (var clip in nextLevelAnimator.runtimeAnimatorController.animationClips)
                if (clip.name == "NextLevelFadeOut") wait = clip.length;

            yield return new WaitForSeconds(wait + 0.05f);
        }

        SceneManager.LoadScene(scene);
        panelShown = false;
    }

    public void SetClueIndexFromSave(int index)
    {
        currentClue = index;
    }

    private void PlaySound(AudioSource src, AudioClip clip, string name)
    {
        if (clip == null)
        {
            Debug.LogWarning($"ClueManager: {name} clip is null - nothing to play.");
            return;
        }

        if (src != null)
        {
            try
            {
                src.PlayOneShot(clip);
                Debug.Log($"ClueManager: Playing {name} via assigned AudioSource.");
            }
            catch (System.Exception ex)
            {
                Debug.LogWarning($"ClueManager: PlayOneShot failed for {name}: {ex}");
                AudioSource.PlayClipAtPoint(clip, GetListenerPosition());
            }
        }
        else
        {
            Debug.Log($"ClueManager: No AudioSource for {name}, using PlayClipAtPoint fallback.");
            AudioSource.PlayClipAtPoint(clip, GetListenerPosition());
        }
    }

    private Vector3 GetListenerPosition()
    {
        var listener = FindObjectOfType<AudioListener>();
        if (listener != null) return listener.transform.position;
        if (Camera.main != null) return Camera.main.transform.position;
        var player = GameObject.FindWithTag("Player");
        if (player != null) return player.transform.position;
        return Vector3.zero;
    }
}
