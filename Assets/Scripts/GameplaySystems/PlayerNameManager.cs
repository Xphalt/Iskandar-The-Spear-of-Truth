using UnityEngine;
using TMPro;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class PlayerNameManager : MonoBehaviour
{
    public int currentSaveFile = 0;
    public TMP_InputField _InputField;

    private void Start()
    {
        _InputField.characterLimit = 8;
    }

    public void SetSaveNum(int num)
    {
        currentSaveFile = num;
    }

    public void SavePlayerName()
    {
        string name = _InputField.text;
        Debug.Log(currentSaveFile + " " + name);
        BinaryFormatter bf = new BinaryFormatter();

        string filePath = Application.persistentDataPath + "/Player_name" + currentSaveFile + ".txt";
        FileStream fs = new FileStream(filePath, FileMode.Create);

        bf.Serialize(fs, name);
        fs.Close();
    }
}