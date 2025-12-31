using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveController : MonoBehaviour
{
    [Tooltip("Nhấn phím này để lưu nhanh (Inspector)")]
    public KeyCode saveKey = KeyCode.F5;

    [Tooltip("Tự động lưu khi thoát ứng dụng")]
    public bool autoSaveOnQuit = true;

    // Gọi thủ công từ UI hoặc từ code
    public void SaveNow()
    {
        var player = GameObject.FindWithTag("Player");
        if (player == null)
        {
            Debug.LogWarning("SaveController: không tìm thấy Player (tag 'Player'). Lưu sẽ bỏ qua vị trí player.");
        }

        var cm = FindObjectOfType<ClueManager>();
        int currentClue = 0; // default to 0 (index-based as in ClueManager)

        if (cm != null)
        {
            // Try to read private field 'currentClue' by reflection
            var cmType = cm.GetType();
            var currentField = cmType.GetField("currentClue", BindingFlags.NonPublic | BindingFlags.Instance);
            if (currentField != null)
            {
                object val = currentField.GetValue(cm);
                if (val is int i) currentClue = i;
            }
            else
            {
                // Fallback: infer from public clues list (first active index or count if none active)
                try
                {
                    if (cm.clues != null && cm.clues.Count > 0)
                    {
                        int activeIndex = -1;
                        for (int idx = 0; idx < cm.clues.Count; idx++)
                        {
                            var go = cm.clues[idx];
                            if (go != null && go.activeSelf)
                            {
                                activeIndex = idx;
                                break;
                            }
                        }

                        if (activeIndex >= 0)
                            currentClue = activeIndex;
                        else
                            currentClue = cm.clues.Count; // none active means all clues completed
                    }
                }
                catch (System.Exception ex)
                {
                    Debug.LogWarning($"SaveController: Fallback infer currentClue failed: {ex}");
                    currentClue = 0;
                }
            }
        }

        int sceneIndex = SceneManager.GetActiveScene().buildIndex;

        Transform playerTransform = player != null ? player.transform : null;
        var data = SaveData.FromSceneAndTransform(sceneIndex, currentClue, playerTransform);
        bool ok = SaveManager.SaveGame(data);
        if (ok) Debug.Log($"SaveController: Game saved. scene={sceneIndex} currentClue={currentClue}");
    }

    void Update()
    {
        if (Input.GetKeyDown(saveKey))
        {
            SaveNow();
        }
    }

    void OnApplicationQuit()
    {
        if (autoSaveOnQuit)
            SaveNow();
    }
}
