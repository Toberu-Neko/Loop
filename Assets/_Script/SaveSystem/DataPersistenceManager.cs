using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;
using System;
using Eflatun.SceneReference;

public class DataPersistenceManager : MonoBehaviour
{
    [Header("Debugging")]
    [SerializeField] private bool initializeDataIfNull = false;
    [field: SerializeField] public bool DisableDataPersistance { get; private set; } = false;
    [SerializeField] private bool overwriteSelectedProfile = false;
    [SerializeField] private string selectedProfileIdDebug = "";

    [Header("Config")]
    [SerializeField] private string fileName;
    [SerializeField] private bool useEncryption = false;

    [SerializeField] private SceneReference baseScene;
    [SerializeField] private SceneReference mainMenuScene;

    public GameData GameData { get; private set;}
    public List<IDataPersistance> DataPersistanceObjects { get; private set; }
    private FileDataHandler dataHandler;

    private string selectedProfileId = "";

    private float timer;

    public event Action OnSave;
    public event Action OnLoad;

    private bool firstTimeLoad;

    public static DataPersistenceManager Instance { get; private set; }

    private int loadObjCount;

    private void Awake()
    {
        if(Instance != null)
        {
            // Debug.Log("Found more than one data persistence manager in the scene, delete the new one.");
            Destroy(gameObject);
            return;
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        timer = 0f;
        dataHandler = new FileDataHandler(Application.persistentDataPath, fileName, useEncryption);
        selectedProfileId = dataHandler.GetMostRecentlyUpdatedProfileId();
        firstTimeLoad = false;

        if (DisableDataPersistance)
        {
            Debug.LogError("Data persistance is disabled, this should only be used for debugging. And something will go wrong.");
            GameData = new GameData();
        }

        if (overwriteSelectedProfile)
        {
            selectedProfileId = selectedProfileIdDebug;
            Debug.LogError("Overwriting selected profile id: " + selectedProfileId + ", this should only be used for debugging.");
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Update()
    {
        timer += Time.unscaledDeltaTime;
    }

    public void CheckIfShouldSaveOnLoad()
    {
        if (FindAllDataPersistenceObjects().Count > loadObjCount)
        {
            SaveGame();
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // TODO: Load when enter boss room, solved with manually calling load game on bossbase script
        if (scene.name == baseScene.Name)
        {
            LoadGame();
            if (firstTimeLoad)
            {
                SaveGame();
                LoadGame();
                firstTimeLoad = false;
            }
        }
    }

    private List<IDataPersistance> FindAllDataPersistenceObjects()
    {
        /*
        Scene scene = SceneManager.GetSceneByName("MultiSceneBase");
        GameObject[] objs = scene.GetRootGameObjects();
        int baseObjCount = 0;

        foreach (GameObject obj in objs)
        {
            IDataPersistance[] items = obj.GetComponentsInChildren<IDataPersistance>();
            baseObjCount += items.Length;

            if(items.Length > 0)
            {
                Debug.Log("Found " + items.Length + " data persistance objects in " + obj.name);
            }
        }

        Debug.Log("Base object count: " + baseObjCount);
        */

        IEnumerable<IDataPersistance> dataPersistanceObjects = FindObjectsOfType<MonoBehaviour>(true).OfType<IDataPersistance>();


        return new List<IDataPersistance>(dataPersistanceObjects);
    }

    public void NewGame()
    {
        GameData = new();
        dataHandler.Save(GameData, selectedProfileId);
        Debug.Log("Creating new game");
        firstTimeLoad = true;
    }


    public void LoadGame()
    {
        DataPersistanceObjects = FindAllDataPersistenceObjects();
        Debug.Log("Load " + DataPersistanceObjects.Count + " objects.");
        loadObjCount = DataPersistanceObjects.Count;
        if (DisableDataPersistance)
        {
            return;
        }

        GameData = dataHandler.Load(selectedProfileId);

        if (GameData == null)
        {
            if (initializeDataIfNull)
            {
                NewGame();
            }
            else
            {
                Debug.LogWarning("No game data found, need to start game first before loading game");
                return;
            }
        }
        timer = 0f;

        if (firstTimeLoad)
        {
            GameData.gotoSavePoint = true;
        }

        foreach (IDataPersistance dataPersistanceObject in DataPersistanceObjects)
        {
            dataPersistanceObject.LoadData(GameData);
        }

        OnLoad?.Invoke();
    }

    public void SaveGame(bool gotoSavepoint = true)
    {
        DataPersistanceObjects = FindAllDataPersistenceObjects();
        Debug.Log("Saved, " + DataPersistanceObjects.Count + " Objects.");
        if (DisableDataPersistance)
        {
            return;
        }

        if (GameData == null)
        {
            Debug.LogWarning("No game data found, create a new one first.");
            return;
        }

        GameData.gotoSavePoint = gotoSavepoint;

        foreach (IDataPersistance dataPersistanceObject in DataPersistanceObjects)
        {
            dataPersistanceObject.SaveData(GameData);
        }

        GameData.lastUpdated = DateTime.Now.ToBinary();
        GameData.timePlayed += timer;
        timer = 0f;


        dataHandler.Save(GameData, selectedProfileId);

        OnSave?.Invoke();
    }

    public void ChangeSelectedProfileId(string profileId)
    {
        selectedProfileId = profileId;
    }

    public void ReloadBaseScene()
    {
        ObjectPoolManager.ReturnAllObjectsToPool();
        SceneManager.LoadScene(baseScene.Name);
    }

    public void LoadMainMenuScene()
    {
        ObjectPoolManager.ReturnAllObjectsToPool();
        SceneManager.LoadScene(mainMenuScene.Name);
    }

    public bool HasGameData()
    {
        return dataHandler.LoadAllProfiles().Count > 0;
    }

    public Dictionary<string, GameData> GetAllProfilesGameData()
    {
        return dataHandler.LoadAllProfiles();
    }
}
