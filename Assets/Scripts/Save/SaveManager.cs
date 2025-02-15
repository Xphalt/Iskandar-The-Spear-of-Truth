using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public static class SaveManager
{
    // Morgan S Script

    //FILE 1
    public static void SavePlayerStats(SaveData saveData)
    {
        BinaryFormatter bf = new BinaryFormatter();
        
        string filePath = Application.persistentDataPath + "/Player_statsf" + saveData.LastFileSaved + ".txt";
        FileStream fs = new FileStream(filePath, FileMode.Create);

        bf.Serialize(fs, saveData);
        fs.Close();
    }

    public static void SavePlayerInventory(InventoryObject_Sal playerInventory, int num, int sceneIndex = -1)
    {
        BinaryFormatter bf = new BinaryFormatter();

        string filePath = Application.persistentDataPath + "/Player_Inventoryf" + num + playerInventory.type + 
            ((sceneIndex >= 0) ? sceneIndex.ToString() : "") + ".txt";      
        
        FileStream fs = new FileStream(filePath, FileMode.Create);

        SaveData saveData = new SaveData(playerInventory);

        bf.Serialize(fs, saveData);
        fs.Close();
    }

    public static void SaveQuests(QuestLogManager quests, int num)
    {
        BinaryFormatter bf = new BinaryFormatter();

        string filePath = Application.persistentDataPath + "/Player_Questsf" + num + ".txt";
        FileStream fs = new FileStream(filePath, FileMode.Create);

        SaveData saveData = new SaveData(quests);

        bf.Serialize(fs, saveData);
        fs.Close();
    }

    public static SaveData LoadPlayerStats(int num)
    {
        string filePath = Application.persistentDataPath + "/Player_statsf" + num + ".txt";
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

    public static SaveData LoadPlayerInventory(int num, string inventoryType, int sceneIndex = -1)
    {
        string filePath = Application.persistentDataPath + "/Player_Inventoryf" + num + inventoryType + 
            ((sceneIndex >= 0) ? sceneIndex.ToString() : "") + ".txt";
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

    public static string LoadPlayerName(int num)
    {
        string filePath = Application.persistentDataPath + "/Player_name" + num + ".txt";
        if (File.Exists(filePath))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream fs = new FileStream(filePath, FileMode.Open);

            string saveData = bf.Deserialize(fs) as string;
            fs.Close();

            return saveData;
        }
        else
        {
            return null;
        }
    }

    public static SaveData LoadQuests(int num)
    {
        string filePath = Application.persistentDataPath + "/Player_Questsf" + num + ".txt";
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