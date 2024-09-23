using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public Dictionary<string, object> data = new Dictionary<string, object>();
}

public class DataManager : MonoBehaviour
{
    private static string fileName = "FNaF2SD.bin";

    // Function to save values
    public static void SaveValue<T>(string key, T value, string rootDir)
    {
        string filePath = Path.Combine(rootDir, fileName);
        SaveData saveData = new SaveData();

        if (File.Exists(filePath))
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream file = File.Open(filePath, FileMode.Open, FileAccess.Read);
            saveData = (SaveData)binaryFormatter.Deserialize(file);
            file.Close();
        }
        else
        {
            Debug.LogError("Nothing to Write to!");
        }

        saveData.data[key] = value;

        FileStream saveFile = File.Create(filePath);
        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(saveFile, saveData);
        saveFile.Close();
    }

    // Function to get values
    public static T GetValue<T>(string key, string rootDir)
    {
        string filePath = Path.Combine(rootDir, fileName);
        if (File.Exists(filePath))
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream file = File.Open(filePath, FileMode.Open, FileAccess.Read);
            SaveData saveData = (SaveData)binaryFormatter.Deserialize(file);
            file.Close();

            object value;
            if (saveData.data.TryGetValue(key, out value))
            {
                return (T)value;
            }
        }
        //Debug.LogError("Couldnt Retrieve it");
        return default(T);
    }

    // Function to get values
    public static bool ValueExists(string key, string rootDir)
    {
        string filePath = Path.Combine(rootDir, fileName);
        if (File.Exists(filePath))
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream file = File.Open(filePath, FileMode.Open, FileAccess.Read);
            SaveData saveData = (SaveData)binaryFormatter.Deserialize(file);
            file.Close();

            object value;
            if (saveData.data.TryGetValue(key, out value))
            {
                return true;
            }
        }
        return false;
    }

    // Function to delete all saved data
    public static void DeleteAllData(string rootDir)
    {
        string filePath = Path.Combine(rootDir, fileName);
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
            Debug.Log("All data has been deleted.");
        }
        else
        {
            Debug.LogWarning("No data file found to delete.");
        }
    }
}
