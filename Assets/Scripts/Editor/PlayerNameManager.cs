using UnityEngine;
using TMPro;
using UnityEditor;
using System.IO;

public class PlayerNameManager : MonoBehaviour
{
    private void Start()
    {
        GetComponent<TMP_InputField>().characterLimit = 8;
        // AssetDatabase.Refresh();
    }

    public void CreatePlayerData(string name)
    {
        DirectoryInfo levelDirectoryPath = new DirectoryInfo("Assets/ScriptableObjects/Player Name");
        FileInfo[] fileInfo = levelDirectoryPath.GetFiles("*.*", SearchOption.AllDirectories);

        //If something is in the folder for the player name, like a previous save, delete it
        foreach (FileInfo file in fileInfo)
        {
            file.Delete();
        }

        UserData userData = UserData.CreateInstance(name);

        string localPath = "Assets/ScriptableObjects/Player Name/" + name + ".asset";
        localPath = AssetDatabase.GenerateUniqueAssetPath(localPath);

        AssetDatabase.CreateAsset(userData, localPath);
        AssetDatabase.SaveAssets();

        Debug.Log("Created Player Asset for: " + name);
        AssetDatabase.Refresh();
    }
}