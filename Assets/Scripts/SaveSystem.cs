using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveSystem
{
    private static readonly string path = Application.persistentDataPath + "/etadata.dat";
    private static readonly BinaryFormatter formatter = new();

    public static void SavePlayerData(PlayerData playerData)
    {
        FileStream stream = new(path, FileMode.OpenOrCreate);
        PlayerData data = new(playerData);
        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static PlayerData LoadPlayerData()
    {
        if (File.Exists(path))
        {
            FileStream stream = new(path, FileMode.Open);
            PlayerData data = formatter.Deserialize(stream) as PlayerData;
            stream.Close();
            return data;
        }
        else
        {
            FileStream stream = new(path, FileMode.Create);
            PlayerData data = new();
            formatter.Serialize(stream, data);
            stream.Close();
            return data;
        }
    }
}
