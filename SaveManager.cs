using System;
using System.IO;
using UnityEngine;

public static class SaveManager
{
    private static readonly string fileName = "savegame.json";
    public static string SavePath => Path.Combine(Application.persistentDataPath, fileName);

    public static bool SaveGame(SaveData data)
    {
        try
        {
            string json = JsonUtility.ToJson(data, true);
            File.WriteAllText(SavePath, json);
            Debug.Log($"SaveManager: Saved to {SavePath}");
            return true;
        }
        catch (Exception ex)
        {
            Debug.LogError($"SaveManager: Save failed: {ex}");
            return false;
        }
    }

    public static SaveData LoadGame()
    {
        try
        {
            if (!File.Exists(SavePath))
            {
                Debug.LogWarning("SaveManager: No save file found.");
                return null;
            }

            string json = File.ReadAllText(SavePath);
            var data = JsonUtility.FromJson<SaveData>(json);
            Debug.Log($"SaveManager: Loaded from {SavePath}");
            return data;
        }
        catch (Exception ex)
        {
            Debug.LogError($"SaveManager: Load failed: {ex}");
            return null;
        }
    }

    public static bool DeleteSave()
    {
        try
        {
            if (File.Exists(SavePath))
            {
                File.Delete(SavePath);
                Debug.Log("SaveManager: Save deleted.");
            }
            return true;
        }
        catch (Exception ex)
        {
            Debug.LogError($"SaveManager: Delete failed: {ex}");
            return false;
        }
    }
}