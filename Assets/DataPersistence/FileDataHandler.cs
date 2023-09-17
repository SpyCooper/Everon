using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class FileDataHandler
{
    private string dataDirPath = "";
    private string dataFileName = "";
    private bool useEncryption = false;
    // I enjoy alliteration
    private readonly string encryptionCodeWord = "JohnnyJumpedOverJammingJamboree";

    // constructor
    public FileDataHandler(string dataDirPath, string dataFileName, bool useEncryption)
    {
        // loads in the save data name, path, and if encryption is to be used
        this.dataDirPath = dataDirPath;
        this.dataFileName = dataFileName;
        this.useEncryption = useEncryption;
    }

    // returns the GameData that has been loaded
    public GameData Load()
    {
        // uses Path.Combine to account for different OS's path seperators
        string fullPath = Path.Combine(dataDirPath, dataFileName);
        GameData loadedData = null;
        if(File.Exists(fullPath))
        {
            try
            {
                // Load serialize data from the file
                string dataToLoad = "";
                using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }

                // if encryption is being used, it needs to be decrypted
                if(useEncryption)
                {
                    dataToLoad = EncryptDecrypt(dataToLoad);
                }

                // deserialize the data from Json into C#
                loadedData = JsonUtility.FromJson<GameData>(dataToLoad);
            }
            catch (Exception e)
            {
                Debug.LogError("Error occured when trying to load game data to file: " + fullPath + "\n" + e);
            }
        }
        return loadedData;
    }

    // saves the GameData to the file
    public void Save(GameData data)
    {
        // uses Path.Combine to account for different OS's path seperators
        string fullPath = Path.Combine(dataDirPath, dataFileName);
        try
        {
            // create directory if not already created
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            // serialize the C# data into JSON
            string dataToStore = JsonUtility.ToJson(data, true);

            // if encryption is being used, it needs to be encrypted
            if(useEncryption)
            {
                dataToStore = EncryptDecrypt(dataToStore);
            }

            // write the data to the file
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
            Debug.LogError("Error occured when trying to save game data to file: " + fullPath + "\n" + e);
        }
    }

    // excrypts the data using XOR encryption
    private string EncryptDecrypt(string data)
    {
        string modifiedData = "";
        for(int i = 0; i < data.Length; i++)
        {
            modifiedData += (char) (data[i] ^ encryptionCodeWord[i % encryptionCodeWord.Length]);
        }
        return modifiedData;
    }
}
