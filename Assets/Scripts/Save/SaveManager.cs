using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public static class SaveManager
{
    // Morgan S Script

    //FILE 1
    public static void SavePlayerStats(PlayerStats playerStats, int num)
    {
        BinaryFormatter bf = new BinaryFormatter();

        string filePath = Application.persistentDataPath + "/Player_statsf" + num + ".txt";
        FileStream fs = new FileStream(filePath, FileMode.Create);

        SaveData saveData = new SaveData(playerStats);

        bf.Serialize(fs, saveData);
        fs.Close();
    }

    public static void SavePlayerInventory(InventoryObject_Sal playerInventory, int num)
    {
        BinaryFormatter bf = new BinaryFormatter();

        string filePath = Application.persistentDataPath + "/Player_Inventoryf" + num + ".txt";
        FileStream fs = new FileStream(filePath, FileMode.Create);

        SaveData saveData = new SaveData(playerInventory);

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

    public static SaveData LoadPlayerInventory(int num)
    {
        string filePath = Application.persistentDataPath + "/Player_Inventoryf" + num + ".txt";
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

}/*
    //FILE 2
    public static void SavePlayerStatsf2(PlayerStats playerStatsf2)
    {
        BinaryFormatter bf = new BinaryFormatter();

        string filePathF2 = Application.persistentDataPath + "/Player_statsf2.txt";
        FileStream fs = new FileStream(filePathF2, FileMode.Create);

        SaveDataF2 saveDataf2 = new SaveDataF2(playerStatsf2);

        bf.Serialize(fs, saveDataf2);
        fs.Close();
    }

    public static void SavePlayerInventoryf2(InventoryObject_Sal playerInventoryf2)
    {
        BinaryFormatter bf = new BinaryFormatter();

        string filePathF2 = Application.persistentDataPath + "/Player_Inventoryf2.txt";
        FileStream fs = new FileStream(filePathF2, FileMode.Create);

        SaveDataF2 saveDataf2 = new SaveDataF2(playerInventoryf2);

        bf.Serialize(fs, saveDataf2);
        fs.Close();
    }


    public static SaveDataF2 LoadPlayerStatsf2()
    {
        string filePathF2 = Application.persistentDataPath + "/Player_statsf2.txt";
        if (File.Exists(filePathF2))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream fs = new FileStream(filePathF2, FileMode.Open);

            SaveDataF2 saveDataf2 = bf.Deserialize(fs) as SaveDataF2;
            fs.Close();

            return saveDataf2;
        }
        else
        {
            return null;
        }
    }

    public static SaveDataF2 LoadPlayerInventoryf2()
    {
        string filePathF2 = Application.persistentDataPath + "/Player_Inventoryf2.txt";
        if (File.Exists(filePathF2))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream fs = new FileStream(filePathF2, FileMode.Open);

            SaveDataF2 saveDataf2 = bf.Deserialize(fs) as SaveDataF2;
            fs.Close();

            return saveDataf2;
        }
        else
        {
            return null;
        }
    }

    //FILE 3
    public static void SavePlayerStatsf3(PlayerStats playerStatsf3)
    {
        BinaryFormatter bf = new BinaryFormatter();

        string filePathF3 = Application.persistentDataPath + "/Player_statsf3.txt";
        FileStream fs = new FileStream(filePathF3, FileMode.Create);

        SaveDataF3 saveDataf3 = new SaveDataF3(playerStatsf3);

        bf.Serialize(fs, saveDataf3);
        fs.Close();
    }

    public static void SavePlayerInventoryf3(InventoryObject_Sal playerInventoryf3)
    {
        BinaryFormatter bf = new BinaryFormatter();

        string filePathF3 = Application.persistentDataPath + "/Player_Inventoryf3.txt";
        FileStream fs = new FileStream(filePathF3, FileMode.Create);

        SaveDataF3 saveDataf3 = new SaveDataF3(playerInventoryf3);

        bf.Serialize(fs, saveDataf3);
        fs.Close();
    }


    public static SaveDataF3 LoadPlayerStatsf3()
    {
        string filePathF3 = Application.persistentDataPath + "/Player_statsf3.txt";
        if (File.Exists(filePathF3))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream fs = new FileStream(filePathF3, FileMode.Open);

            SaveDataF3 saveDataf3 = bf.Deserialize(fs) as SaveDataF3;
            fs.Close();

            return saveDataf3;
        }
        else
        {
            return null;
        }
    }

    public static SaveDataF3 LoadPlayerInventoryf3()
    {
        string filePathF3 = Application.persistentDataPath + "/Player_Inventoryf3.txt";
        if (File.Exists(filePathF3))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream fs = new FileStream(filePathF3, FileMode.Open);

            SaveDataF3 saveDataf3 = bf.Deserialize(fs) as SaveDataF3;
            fs.Close();

            return saveDataf3;
        }
        else
        {
            return null;
        }
    }
} */