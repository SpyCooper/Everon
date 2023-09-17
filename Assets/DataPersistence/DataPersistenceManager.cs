using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DataPersistenceManager : MonoBehaviour
{
    // allows the file name and encryption options to be set
    [Header("File Storage Config")]
    [SerializeField] private string fileName;
    [SerializeField] private bool useEncryption;

    private GameData gameData;
    private List<IDataPersistence> dataPersistenceObjects;
    private FileDataHandler dataHandler;

    // sets up the file as a singleton
    public static DataPersistenceManager Instance {get; private set;}

    // -------------------------------------------------------------------------------------------

    private void Awake()
    {
        // checks to make sure that there's not more than 1 DataPersistenceManager
        if(Instance != null)
        {
            Debug.LogError("Found more than 1 Data Persistence Manager!");
        }
        // sets the instance as this
        Instance  = this;
    }

    private void Start()
    {
        // creates a new FileDataHandler using the default player (user/Appdata/LocalLow/DefaultCompany/Everon), the file name, and if it's using encryption or not
        dataHandler = new FileDataHandler(Application.persistentDataPath, fileName, useEncryption);

        // finds all objects that use IDataPersistence
        dataPersistenceObjects = FindAllDataPersistenceObjects();

        // loads the game
        LoadGame();
    }

    // sets the gameData as a fresh game GameData
    public void NewGame()
    {
        gameData = new GameData();
    }
    
    // loads the game based off the saved file
    public void LoadGame()
    {
        // Load any save data from a file using the data handler
        gameData = dataHandler.Load();

        // if no data was loaded, creates a new game
        if(gameData == null)
        {
            Debug.Log("No data was found. Initializing data to default values.");
            NewGame();
        }

        // sets the currentGameData that the PlayerManager points to as the loaded game data
        PlayerManager.Instance.currentGameData = gameData;
        // creates a new CharacterData based off the GameData
        PlayerManager.Instance.currentCharacterData = new CharacterData(gameData);

        // push data to all other scripts that need it
        foreach(IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            Debug.Log(dataPersistenceObj);
            dataPersistenceObj.LoadData(gameData);
        }
    }
    
    // saves the GameData
    public void SaveGame()
    {
        // pass to other scripts so they can update it
        foreach(IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            dataPersistenceObj.SaveData(ref gameData);
        }

        // save that data to a file using the data handler
        dataHandler.Save(gameData);
    }

    // when the game is closed, GameData should be saved
    private void OnApplicationQuit()
    {
        SaveGame();
    }

    // returns a list of all the scripts that extend IDataPersistence
    // NOTE: the scripts have to extend monobehaviour and IDataPersistence to be found, i.e. CharacterData is not found since it does not extend Monobehaviour
    private List<IDataPersistence> FindAllDataPersistenceObjects()
    {
        IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistence>();

        return new List<IDataPersistence>(dataPersistenceObjects);
    }
}
