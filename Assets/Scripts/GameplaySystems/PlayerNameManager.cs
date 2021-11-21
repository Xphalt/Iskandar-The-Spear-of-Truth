using UnityEngine;
using TMPro;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class PlayerNameManager : MonoBehaviour
{
    internal int currentSaveFile = 0; 

    private void Start()
    {
        GetComponent<TMP_InputField>().characterLimit = 8;
    }

    public void SetSaveNum(int num)
    {
        currentSaveFile = num;
    }

    public void SavePlayerName(string name)
    {
        Debug.Log(currentSaveFile + " " + name);
        BinaryFormatter bf = new BinaryFormatter();

        string filePath = Application.persistentDataPath + "/Player_name" + currentSaveFile + ".txt";
        FileStream fs = new FileStream(filePath, FileMode.Create);

        bf.Serialize(fs, name);
        fs.Close();
    }
}