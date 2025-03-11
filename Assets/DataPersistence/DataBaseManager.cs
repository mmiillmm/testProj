using UnityEngine;
using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Text;

public class DatabaseManager : MonoBehaviour
{
    private string connectionString = "server=localhost;database=game_saves_db;user=root;password=";

    public static DatabaseManager instance { get; private set; }

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    public void UploadSaveFile(string playerID, byte[] encryptedData)
    {
        try
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = "INSERT INTO save_files (player_id, data) VALUES (@id, @data) " +
                               "ON DUPLICATE KEY UPDATE data=@data";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", playerID);
                    cmd.Parameters.AddWithValue("@data", encryptedData);
                    cmd.ExecuteNonQuery();
                }
                Debug.Log("save feltoltve");
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("save nem toltott fel" + ex.Message);
        }
    }

    public IEnumerator DownloadSaveFile(string playerID, Action<byte[]> callback)
    {
        byte[] encryptedData = null;
        try
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT data FROM save_files WHERE player_id = @id";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", playerID);
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            encryptedData = (byte[])reader["data"];
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("save nem toltott le" + ex.Message);
        }

        yield return null;
        callback?.Invoke(encryptedData);
    }
}
