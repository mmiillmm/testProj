using UnityEngine;

public class KillLogger : MonoBehaviour, IDataPersistence
{
    private int totalKills = 0;

    private void Start()
    {
        Debug.Log("logger meggy");
    }

    public void LogKill()
    {
        totalKills++;
        Debug.Log($"logolva: {totalKills}");
    }

    public void LoadData(GameData data)
    {
        totalKills = data.killCount;
        Debug.Log($"loadolva: {totalKills}");
    }

    public void SaveData(ref GameData data)
    {
        data.killCount = totalKills;
        Debug.Log($"ment�sre: {totalKills}");
    }

    public int GetKillCount()
    {
        return totalKills;
    }
}
