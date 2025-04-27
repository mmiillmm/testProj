using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.IO;
using System.Text;
using UnityEngine.SceneManagement;

public class DataPersistenceManager : MonoBehaviour
{
    [SerializeField] private string playerID = "player_001";
    private GameData gameData;

    private List<IDataPersistence> dataPersistenceObjects;
    public static DataPersistenceManager instance;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        LoadGame();
    }

    public void NewGame()
    {
        gameData = new GameData();
    }

    public void LoadGame()
    {
        StartCoroutine(DatabaseManager.instance.DownloadSaveFile(playerID, (json) =>
        {
            if (!string.IsNullOrEmpty(json))
            {
                gameData = JsonUtility.FromJson<GameData>(json);
                Debug.Log("jatek toltve dbbol");
            }
            else
            {
                NewGame();
                Debug.Log("nincs save, uj game");
            }

            dataPersistenceObjects = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None)
                .OfType<IDataPersistence>().ToList();

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
            Debug.LogWarning("nincs mentesre adat");
            return;
        }

        foreach (IDataPersistence obj in dataPersistenceObjects)
        {
            obj.SaveData(ref gameData);
        }

        string json = JsonUtility.ToJson(gameData);
        DatabaseManager.instance.UploadSaveFile(playerID, json);
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }
}
