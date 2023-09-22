using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public PlayerInfo player;
    private string savePath;

    private void Start()
    {
        savePath = Application.persistentDataPath + "/saveData/";
        Debug.Log("Saving to path: " + savePath);
    }

    [ContextMenu("Save Player")]
    public void SavePlayer(PlayerInfo play)
    {
        savePath = Application.persistentDataPath + "/saveData/";
        // save the data
        this.player = play;
        JSONLoaderSaver.SavePlayerAsJSON(savePath, "armour.json", this.player);
    }

    [ContextMenu("Load Armour")]
    public void LoadArmour()
    {
        savePath = Application.persistentDataPath + "/saveData/";
        // load the data
        this.player = JSONLoaderSaver.LoadPlayerFromJSON(savePath, "armour.json");
    }
}