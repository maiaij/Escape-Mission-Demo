using UnityEngine;
using System.IO;

public class JSONLoaderSaver
{
    public static void SavePlayerAsJSON(
        string savePath,
        string filename,
        PlayerInfo player
        )
    {
        if (!Directory.Exists(savePath))
        {
            Directory.CreateDirectory(savePath);
        }
        string json = JsonUtility.ToJson(player);
        File.WriteAllText(savePath + filename, json);
    }
    
    public static PlayerInfo LoadPlayerFromJSON(
        string savePath,
        string filename)
    {
        if (File.Exists(savePath + filename))
        {
            string json = File.ReadAllText(savePath + filename);
            PlayerInfo player = JsonUtility.FromJson<PlayerInfo>(json);
            return player;
        }
        Debug.Log("Unable to load from file: " + savePath + filename);
        return null;
    }
}