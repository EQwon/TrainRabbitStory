﻿using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    public static void SaveData(Data data)
    {
        BinaryFormatter formatter = new BinaryFormatter();

        string path = Application.persistentDataPath + "data.txt";
        FileStream stream = new FileStream(path, FileMode.Create);

        Data newData = new Data(data);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static Data LoadData()
    {
        string path = Application.persistentDataPath + "data.txt";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            Data data = formatter.Deserialize(stream) as Data;
            stream.Close();

            data.hp = 1000;
            return data;
        }
        else
        {
            Debug.Log("Save file not found in " + path);
            Data data = new Data();
            return data;
        }
    }

    public static void DeleteData()
    {
        string path = Application.persistentDataPath + "data.txt";
        if (File.Exists(path))
        {
            Debug.Log("Delete save file in " + path);
            File.Delete(path);
        }
    }
}