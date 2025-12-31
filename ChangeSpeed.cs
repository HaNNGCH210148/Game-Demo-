using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSpeed : MonoBehaviour
{
    public float newWalkSpeed = 3f;     // tốc độ đi bình thường
    public float newSprintSpeed = 7f;   // tốc độ khi nhấn Shift
    private MonoBehaviour fpcScript;    // script FirstPersonController
    private System.Type fpcType;

    void Start()
    {
        fpcScript = (MonoBehaviour)GetComponent("FirstPersonController");
        if (fpcScript != null)
        {
            fpcType = fpcScript.GetType();

            // đè tốc độ đi
            var walkField = fpcType.GetField("walkSpeed");
            if (walkField != null)
                walkField.SetValue(fpcScript, newWalkSpeed);

            // đè tốc độ chạy
            var sprintField = fpcType.GetField("sprintSpeed");
            if (sprintField != null)
                sprintField.SetValue(fpcScript, newSprintSpeed);

            Debug.Log("✅ Speed changed successfully!");
        }
        else
        {
            Debug.LogError("❌ Không tìm thấy script FirstPersonController trên Player!");
        }
    }
}
