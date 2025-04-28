using UnityEngine;
using MySql.Data.MySqlClient;
using System;
using System.Collections;

public class DatabaseManager : MonoBehaviour
{
    private string connectionString = "server=localhost;database=gamedb;user=root;password=";

    public static DatabaseManager instance;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    public void UploadSaveFile(string playerID, string plainJson)
    {
        try
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = "INSERT INTO save_files (player_id, json_data) VALUES (@id, @data) " +
                               "ON DUPLICATE KEY UPDATE json_data=@data";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", playerID);
                    cmd.Parameters.AddWithValue("@data", plainJson);
                    cmd.ExecuteNonQuery();
                }

                Debug.Log("save feltoltve.");
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("nem sikerult jsont feltolteni: " + ex.Message);
        }
    }

    public IEnumerator DownloadSaveFile(string playerID, Action<string> callback)
    {
        string jsonData = null;
        try
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT json_data FROM save_files WHERE player_id = @id";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", playerID);
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            jsonData = reader["json_data"].ToString();
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("jsont nem sikerult letolteni " + ex.Message);
        }

        yield return null;
        callback?.Invoke(jsonData);
    }
}
