using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class FileDataHandler
{
    private string dataDirectoryPath = "";
    private string dataFileName = "";

    private bool useEncryption = false;
    private readonly string encryptionPassword = "IronHeartCat";

    public FileDataHandler(string dataDirectoryPath, string dataFileName, bool useEncryption)
    {
        this.dataDirectoryPath = dataDirectoryPath;
        this.dataFileName = dataFileName;
        this.useEncryption = useEncryption;
    }

    public GameData Load(string profileId)
    {
        if(profileId == null)
        {
            Debug.Log("Tried to load game data with null profile id");
            return null;
        }

        string fullPath = Path.Combine(dataDirectoryPath, profileId, dataFileName);
        GameData loadedData = null;

        if (File.Exists(fullPath))
        {
            try
            {
                string dataToLoad = "";
                using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }

                if(useEncryption)
                {
                    dataToLoad = EncryptDecrypt(dataToLoad);
                }

                loadedData = JsonUtility.FromJson<GameData>(dataToLoad);
            }
            catch (Exception e)
            {
                Debug.LogError("Error loading game data: " + fullPath + "\n" + e.Message);
            }
        }
        return loadedData;
    }

    public void Save(GameData gameData, string profileId)
    {
        if (profileId == null)
        {
            Debug.Log("Tried to save game data with null profile id");
            return;
        }

        string fullPath = Path.Combine(dataDirectoryPath, profileId, dataFileName);
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            string dataToStore = JsonUtility.ToJson(gameData, true);

            if (useEncryption)
            {
                dataToStore = EncryptDecrypt(dataToStore);
            }

            using(FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                using(StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(dataToStore);
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Error saving game data: " + fullPath + "\n" + e.Message);
        }
    }

    public OptionData LoadOptionData()
    {
        string fullPath = Path.Combine(dataDirectoryPath, "Option.opt");
        OptionData loadedData = null;

        if (File.Exists(fullPath))
        {
            try
            {
                string dataToLoad = "";
                using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }

                if (useEncryption)
                {
                    dataToLoad = EncryptDecrypt(dataToLoad);
                }

                loadedData = JsonUtility.FromJson<OptionData>(dataToLoad);
            }
            catch (Exception e)
            {
                Debug.LogError("Error loading game data: " + fullPath + "\n" + e.Message);
            }
        }
        return loadedData;
    }

    public void SaveOptionData(OptionData optionData)
    {
        string fullPath = Path.Combine(dataDirectoryPath, "Option.opt");
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            string dataToStore = JsonUtility.ToJson(optionData, true);

            if (useEncryption)
            {
                dataToStore = EncryptDecrypt(dataToStore);
            }

            using (FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(dataToStore);
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Error saving game data: " + fullPath + "\n" + e.Message);
        }
    }

    public Dictionary<string, GameData> LoadAllProfiles()
    {
        Dictionary<string, GameData> profileDictionary = new();

        IEnumerable<DirectoryInfo> dirInfos = new DirectoryInfo(dataDirectoryPath).EnumerateDirectories();
        foreach (DirectoryInfo dirInfo in dirInfos)
        {
            string profileId = dirInfo.Name;

            string fullPath = Path.Combine(dataDirectoryPath, profileId, dataFileName);

            if (!File.Exists(fullPath))
            {
                Debug.LogWarning("Skipping directory when loading all profiles because it does not contain data: " + profileId);
                continue;
            }

            GameData profileData = Load(profileId);
            if (profileData != null)
            {
                profileDictionary.Add(profileId, profileData);
            }
            else
            {
                Debug.LogError("Tried to load profile but failed: " + profileId + "\n" + "Skipping profile");
                continue;
            }
        }
        return profileDictionary;
    }

    public string GetMostRecentlyUpdatedProfileId()
    {
        string mostRecentlyUpdatedProfileId = null;

        Dictionary<string, GameData> profilesGameData = LoadAllProfiles();

        foreach (KeyValuePair<string, GameData> profileGameData in profilesGameData)
        {
            string profileId = profileGameData.Key;
            GameData gameData = profileGameData.Value;

            if(gameData == null)
            {
                continue;
            }

            if(mostRecentlyUpdatedProfileId == null)
            {
                mostRecentlyUpdatedProfileId = profileId;
            }
            else
            {
                DateTime mostRecentDateTime = DateTime.FromBinary(profilesGameData[mostRecentlyUpdatedProfileId].lastUpdated);
                DateTime newDateTime = DateTime.FromBinary(gameData.lastUpdated);

                if(newDateTime > mostRecentDateTime)
                {
                    mostRecentlyUpdatedProfileId = profileId;
                }
            }
        }
        return mostRecentlyUpdatedProfileId;
    }

    private string EncryptDecrypt(string data)
    {
        string modifiedData = "";
        for (int i = 0; i < data.Length; i++)
        {
            modifiedData += (char)(data[i] ^ encryptionPassword[i % encryptionPassword.Length]);
        }
        return modifiedData;
    }
}
