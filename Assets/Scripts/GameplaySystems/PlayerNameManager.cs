using UnityEngine;
using TMPro;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Linq;

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


    internal string invalidCharacters = "!£$%^&*()_-+=@#~`¬|/?.>,< ";

    private bool validName;
    public void ValidNameCheck(string name)
    {
        validName = true;
        for (int i = 0; i < invalidCharacters.Length; i++)
        {
            if (name.Contains(invalidCharacters[i]))
            {
                validName = false;
            }
        }

        if (name.Length <= 0)
        {
            validName = false;
        }

        if (!validName) _InputField.text = "";
    }


    public void SavePlayerName()
    {
        ValidNameCheck(_InputField.text);
        if (validName)
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
}