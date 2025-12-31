using UnityEngine;
using UnityEngine.SceneManagement;

[AddComponentMenu("SaveSystem/Save Loader")]
public class SaveLoader : MonoBehaviour
{
    public TextAsset jsonFile;

    [Tooltip("Tự động load scene nếu scene lưu khác scene hiện tại.")]
    public bool loadSceneIfDifferent = false;

    [Tooltip("Áp dữ liệu ngay khi kéo file JSON vào (Editor only).")]
    public bool autoApplyOnAssign = true;

    public void ApplyFromTextAsset()
    {
        if (jsonFile == null)
        {
            Debug.LogWarning("SaveLoader: chưa có file JSON.");
            return;
        }

        SaveData data = null;

        try
        {
            data = JsonUtility.FromJson<SaveData>(jsonFile.text);
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"SaveLoader: lỗi parse JSON: {ex}");
            return;
        }

        if (data == null)
        {
            Debug.LogWarning("SaveLoader: JSON không hợp lệ.");
            return;
        }

        Apply(data);
    }

    public void Apply(SaveData data)
    {
        if (data == null) return;

        // --- 1. Load scene nếu khác ---
        int currentScene = SceneManager.GetActiveScene().buildIndex;

        if (data.sceneBuildIndex != 0 &&
            data.sceneBuildIndex != currentScene &&
            loadSceneIfDifferent)
        {
            Debug.Log($"SaveLoader: Load scene {data.sceneBuildIndex} từ save.");
            SceneManager.LoadScene(data.sceneBuildIndex);
            return;
        }

        // --- 2. Áp trạng thái clue ---
        ApplyClueState(data);

        // --- 3. Áp vị trí player ---
        ApplyPlayerTransform(data);

        Debug.Log("SaveLoader: Applied save data.");
    }

    private void ApplyClueState(SaveData data)
    {
        var cm = FindObjectOfType<ClueManager>();
        if (cm == null) return;

        // reset tất cả clue trước
        for (int i = 0; i < cm.clues.Count; i++)
            if (cm.clues[i] != null)
                cm.clues[i].SetActive(false);

        // bật đúng clue theo currentClue
        int index = data.currentClue;

        if (index >= 0 && index < cm.clues.Count)
            cm.clues[index].SetActive(true);

        // set value cho currentClue (public method tốt hơn reflection)
        cm.SetClueIndexFromSave(index);
    }

    private void ApplyPlayerTransform(SaveData data)
    {
        var player = GameObject.FindWithTag("Player");
        if (player == null) return;

        Vector3 pos = data.GetPlayerPosition();

        // Không dùng Translate → dùng trực tiếp position
        player.transform.position = pos;

        var rb = player.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.position = pos;
        }

        var cc = player.GetComponent<CharacterController>();
        if (cc != null)
        {
            cc.enabled = false;
            player.transform.position = pos;
            cc.enabled = true;
        }
    }

#if UNITY_EDITOR
    void OnValidate()
    {
        if (!autoApplyOnAssign) return;
        if (jsonFile == null) return;

        if (!UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode)
        {
            ApplyFromTextAsset();
        }
    }
#endif
}
