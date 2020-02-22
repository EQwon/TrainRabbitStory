using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

public static class SaveSystem
{
    public static void SaveData(Data data)
    {
        string path = Application.persistentDataPath + "/data.txt";
        FileStream stream = new FileStream(path, FileMode.OpenOrCreate);

        try
        {
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, data);
            Debug.Log("Save Successfully at " + path);
        }
        catch(SerializationException e)
        {
            Debug.LogError("Sth happens during Saving : " + e.Message);
        }
        finally
        {
            stream.Close();
        }
    }

    public static Data LoadData()
    {
        string path = Application.persistentDataPath + "/data.txt";
        Debug.Log(path);
        if (!File.Exists(path))
        {
            Debug.Log("Save file not found in " + path);
            Debug.Log("Create And Saving New File...");
            SaveData(new Data());
        }

        FileStream stream = new FileStream(path, FileMode.Open);
        Data data = new Data();

        try
        {
            BinaryFormatter formatter = new BinaryFormatter();
            data = formatter.Deserialize(stream) as Data;
            Debug.Log("Load Successfully");
        }
        catch (SerializationException e)
        {
            Debug.LogError("Sth happens during Loading : " + e.Message);
        }
        finally
        {
            stream.Close();
        }

        return data;
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