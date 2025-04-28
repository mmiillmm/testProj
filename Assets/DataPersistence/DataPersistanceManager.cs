using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.IO;
using UnityEngine.SceneManagement;
using System.Collections;

public class DataPersistenceManager : MonoBehaviour
{
    [SerializeField] private string playerID = "default_player";
    private GameData gameData;
    private List<IDataPersistence> dataPersistenceObjects;
    public static DataPersistenceManager instance;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    public void LoadNameAndSave()
    {
        StartCoroutine(LoadThenSaveRoutine());
    }

    private IEnumerator LoadThenSaveRoutine()
    {
        string path = @"H:\xampp\htdocs\project-main\backend\active_user.txt";

        if (File.Exists(path))
        {
            playerID = File.ReadAllText(path).Trim();
            Debug.Log("playerid betoltve" + playerID);
        }
        else
        {
            Debug.LogWarning("active_user.txt nincs meg " + path + " — defa idt hasznal.");
        }

        yield return StartCoroutine(LoadGameRoutine());
        SaveGame();
    }

    private IEnumerator LoadGameRoutine()
    {
        bool isDone = false;

        StartCoroutine(DatabaseManager.instance.DownloadSaveFile(playerID, (json) =>
        {
            if (!string.IsNullOrEmpty(json))
            {
                gameData = JsonUtility.FromJson<GameData>(json);
                Debug.Log("jatek betoltve");
            }
            else
            {
                NewGame();
                Debug.Log("nincs save, uj jatek kezdve");
            }

            dataPersistenceObjects = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None)
                .OfType<IDataPersistence>().ToList();

            foreach (IDataPersistence obj in dataPersistenceObjects)
            {
                obj.LoadData(gameData);
            }

            isDone = true;
        }));

        yield return new WaitUntil(() => isDone);
    }

    public void NewGame()
    {
        gameData = new GameData();
    }

    public void SaveGame()
    {
        if (gameData == null)
        {
            Debug.LogWarning("nincs mentheto adat");
            return;
        }

        foreach (IDataPersistence obj in dataPersistenceObjects)
        {
            obj.SaveData(ref gameData);
        }

        string json = JsonUtility.ToJson(gameData);
        DatabaseManager.instance.UploadSaveFile(playerID, json);
        Debug.Log("jatek mentve neki " + playerID);
    }
    private void OnApplicationQuit()
    {
        SaveGame();
    }

}
