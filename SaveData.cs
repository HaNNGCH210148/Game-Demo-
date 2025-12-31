using System;
using UnityEngine;

[Serializable]
public class SaveData
{
    public int sceneBuildIndex;
    public int currentClue;

    // player position example
    public float playerPosX;
    public float playerPosY;
    public float playerPosZ;

    // add more fields as needed (inventory, settings, etc.)
    public static SaveData FromSceneAndTransform(int sceneIndex, int currentClue, Transform playerTransform)
    {
        var d = new SaveData
        {
            sceneBuildIndex = sceneIndex,
            currentClue = currentClue
        };

        if (playerTransform != null)
        {
            d.playerPosX = playerTransform.position.x;
            d.playerPosY = playerTransform.position.y;
            d.playerPosZ = playerTransform.position.z;
        }

        return d;
    }

    public Vector3 GetPlayerPosition()
    {
        return new Vector3(playerPosX, playerPosY, playerPosZ);
    }
}