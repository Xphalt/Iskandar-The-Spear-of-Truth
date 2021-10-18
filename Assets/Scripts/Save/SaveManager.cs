using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public static class SaveManager
{

    //FILE 1
    public static void SavePlayerStatsf1(PlayerStats playerStatsf1)
    {
        BinaryFormatter bf = new BinaryFormatter();

        string filePathF1 = Application.persistentDataPath + "/Player_statsf1.txt";
        FileStream fs = new FileStream(filePathF1, FileMode.Create);

        SaveDataF1 saveDataf1 = new SaveDataF1(playerStatsf1);

        bf.Serialize(fs, saveDataf1);
        fs.Close();
    }


    public static SaveDataF1 LoadPlayerStatsf1()
    {
        string filePathF1 = Application.persistentDataPath + "/Player_statsf1.txt";
        if (File.Exists(filePathF1))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream fs = new FileStream(filePathF1, FileMode.Open);

            SaveDataF1 saveDataf1 = bf.Deserialize(fs) as SaveDataF1;
            fs.Close();

            return saveDataf1;
        }
        else
        {
            return null;
        }
    }

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


}