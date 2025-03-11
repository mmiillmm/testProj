using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Collections;

public class DataPersistenceManager : MonoBehaviour
{
    [Header("File Storage Conf")]
    [SerializeField] private string fileName;
    [SerializeField] private bool useEncryption;
    private string playerID = "PlayerTest";

    private GameData gameData;
    private List<IDataPersistence> dataPersistenceObjects;
    private FileDataHandler dataHandler;

    public static DataPersistenceManager instance { get; private set; }

    private void Awake()
    {
        if (instance == null) instance = this;
        else
        {
            Debug.LogError("csak egy dbm lehet");
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        this.dataHandler = new FileDataHandler(Application.persistentDataPath, fileName, useEncryption);
        this.dataPersistenceObjects = FindAllDataPersistenceObjects();
        LoadGame();
    }

    public void NewGame()
    {
        this.gameData = new GameData();
    }

    public void LoadGame()
    {
        DatabaseManager dbManager = FindFirstObjectByType<DatabaseManager>();
        if (dbManager == null)
        {
            Debug.LogError("dbm nincs meg");
            return;
        }

        StartCoroutine(dbManager.DownloadSaveFile(playerID, (byte[] encryptedData) =>
        {
            if (encryptedData == null)
            {
                Debug.LogWarning("nincs data, uj save keszites");
                NewGame();
                return;
            }

            string decryptedJson = XORDecrypt(encryptedData);
            gameData = JsonUtility.FromJson<GameData>(decryptedJson);

            foreach (IDataPersistence obj in dataPersistenceObjects)
            {
                obj.LoadData(gameData);
            }
        }));
    }

    public void SaveGame()
    {
        if (gameData == null)
        {
            Debug.LogError("no idea de valami szar");
            return;
        }

        foreach (IDataPersistence obj in dataPersistenceObjects)
        {
            obj.SaveData(ref gameData);
        }

        string json = JsonUtility.ToJson(gameData);
        byte[] encryptedData = XOREncrypt(json);

        DatabaseManager dbManager = FindFirstObjectByType<DatabaseManager>();
        if (dbManager != null)
        {
            dbManager.UploadSaveFile(playerID, encryptedData);
        }
        else
        {
            Debug.LogError("nem talal dbmet");
        }
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

    private List<IDataPersistence> FindAllDataPersistenceObjects()
    {
        IEnumerable<IDataPersistence> objs = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None).OfType<IDataPersistence>();
        return new List<IDataPersistence>(objs);
    }

    private byte[] XOREncrypt(string data)
    {
        byte[] bytes = System.Text.Encoding.UTF8.GetBytes(data);
        byte key = 0x5A;
        for (int i = 0; i < bytes.Length; i++)
        {
            bytes[i] ^= key;
        }
        return bytes;
    }

    private string XORDecrypt(byte[] data)
    {
        byte key = 0x5A;
        for (int i = 0; i < data.Length; i++)
        {
            data[i] ^= key;
        }
        return System.Text.Encoding.UTF8.GetString(data);
    }
}
