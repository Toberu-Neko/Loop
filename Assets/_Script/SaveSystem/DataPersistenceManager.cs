using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;
using System;
using Eflatun.SceneReference;

/// <summary>
/// This manager is responsible for saving and loading game data.
/// Also responsible for saving and loading option data.
/// Dependencies: ObjectPoolManager.cs, LoadSceneManager.cs
/// </summary>
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
    public OptionData OptionData { get; private set; }
    public Vector3 playerPosition;

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

    #region OptionData
    public void LoadOptionData()
    {
        OptionData = dataHandler.LoadOptionData();

        bool doSaveAfterLoad = false;
        if(OptionData == null)
        {
            if (Application.systemLanguage == SystemLanguage.Chinese || Application.systemLanguage == SystemLanguage.ChineseSimplified || Application.systemLanguage == SystemLanguage.ChineseTraditional)
            {
                OptionData = new OptionData(0);
            }
            else if (Application.systemLanguage == SystemLanguage.Japanese)
            {
                OptionData = new OptionData(1);
            }
            else
            {
                OptionData = new OptionData(2);
            }
            Debug.Log("No Option data, lan index = " + OptionData.languageIndex);
            doSaveAfterLoad = true;
        }

        List<IOptionData> options = FindAllOptionDataObjects();

        foreach (IOptionData option in options)
        {
            option.LoadOptionData(OptionData);
        }

        if(doSaveAfterLoad)
            SaveOptionData();
    }

    public void SaveOptionData()
    {
        List<IOptionData> options = FindAllOptionDataObjects();

        foreach (IOptionData option in options)
        {
            option.SaveOptionData(OptionData);
        }

        dataHandler.SaveOptionData(OptionData);
    }

    private List<IOptionData> FindAllOptionDataObjects()
    {
        IEnumerable<IOptionData> objs = FindObjectsOfType<MonoBehaviour>(true).OfType<IOptionData>();

        return new List<IOptionData>(objs);
    }
    #endregion

    public void CheckIfShouldSaveOnLoad()
    {
        if (FindAllDataPersistenceObjects().Count > loadObjCount)
        {
            SaveGame();
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == baseScene.Name)
        {
            LoadOptionData();
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
        IEnumerable<IDataPersistance> dataPersistanceObjects = FindObjectsOfType<MonoBehaviour>(true).OfType<IDataPersistance>();

        return new List<IDataPersistance>(dataPersistanceObjects);
    }

    /// <summary>
    /// Create a new game data.
    /// </summary>
    public void NewGame()
    {
        GameData = new();
        dataHandler.Save(GameData, selectedProfileId);
        Debug.Log("Creating new game");
        firstTimeLoad = true;
    }

    /// <summary>
    /// This method is used to load the game data.
    /// Cant load the objects in the level scene.
    /// Using Start() method in the level scene to load the objects instead.
    /// </summary>
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

    /// <summary>
    /// This method is used to save the game data.
    /// Will automatically find all data persistance objects in the scene, including the inactive ones and objects in the level scene.
    /// </summary>
    /// <param name="gotoSavepoint"></param>
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
        LoadSceneManager.Instance.LoadSceneSingle(baseScene.Name);
    }

    public void LoadMainMenuScene()
    {
        ObjectPoolManager.ReturnAllObjectsToPool();
        LoadSceneManager.Instance.LoadSceneSingle(mainMenuScene.Name);
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
