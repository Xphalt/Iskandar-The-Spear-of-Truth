using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public static class SaveManager
{
    public static void SavePlayerStatsf1(CharacterStats playerStats)
    {
        BinaryFormatter bf = new BinaryFormatter();

        string filePath = Application.persistentDataPath + "/Player_statsf1.txt";
        FileStream fs = new FileStream(filePath, FileMode.Create);

        SaveData saveData = new SaveData(playerStats);

        bf.Serialize(fs, saveData);
        fs.Close();
    }


    public static SaveData LoadPlayerStatsf1()
    {
        string filePath = Application.persistentDataPath + "/Player_statsf1.txt";
        if (File.Exists(filePath))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream fs = new FileStream(filePath, FileMode.Open);

            SaveData saveData = bf.Deserialize(fs) as SaveData;
            fs.Close();

            return saveData;
        }
        else
        {
            return null;
        }
    }

    //"PlayerStats_Jerzy" is a placeholder location

    public static void SaveInventoryStatsf1(CharacterStats playerStats)
    {
        BinaryFormatter bf = new BinaryFormatter();

        string filePath = Application.persistentDataPath + "/inventory_statsf1.txt";
        FileStream fs = new FileStream(filePath, FileMode.Create);

        SaveData saveData = new SaveData(playerStats);

        bf.Serialize(fs, saveData);
        fs.Close();
    }


    public static SaveData LoadInventoryStatsf1()
    {
        string filePath = Application.persistentDataPath + "/inventory_statsf1.txt";
        if (File.Exists(filePath))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream fs = new FileStream(filePath, FileMode.Open);

            SaveData saveData = bf.Deserialize(fs) as SaveData;
            fs.Close();

            return saveData;
        }
        else
        {
            return null;
        }
    }
}