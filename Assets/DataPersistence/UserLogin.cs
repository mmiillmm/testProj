using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class UserLogin : MonoBehaviour
{
    public string loginPageURL = "http://localhost/project-main/logreg.html";
    public string getUserURL = "http://localhost/project-main/backend/get_user.php";
    public static string playerID;

    private string phpSessionID;

    public void OpenLoginPage()
    {
        Application.OpenURL(loginPageURL);
        Debug.Log("login page nytiva");
    }

    public void TryFetchUsernameFromSession()
    {
        StartCoroutine(GetUsernameFromSession());
    }

    private IEnumerator GetUsernameFromSession()
    {
        UnityWebRequest request = UnityWebRequest.Get(getUserURL);
        if (!string.IsNullOrEmpty(phpSessionID))
        {
            request.SetRequestHeader("Cookie", "PHPSESSID=" + phpSessionID);
        }

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string jsonResponse = request.downloadHandler.text;
            Debug.Log("szerver valasz: " + jsonResponse);

            try
            {
                UserResponse response = JsonUtility.FromJson<UserResponse>(jsonResponse);
                if (!string.IsNullOrEmpty(response.username))
                {
                    playerID = response.username;
                    Debug.Log("aktiv user: " + playerID);
                }
                else
                {
                    Debug.LogWarning("felhnev nincs meg.");
                }
            }
            catch
            {
                Debug.LogError("json parse nem sikerult: " + jsonResponse);
            }
        }
        else
        {
            Debug.LogError("session data nem sikerult " + request.error);
        }
    }

    [System.Serializable]
    public class UserResponse
    {
        public string username;
    }

    public void SetSessionID(string id)
    {
        phpSessionID = id;
        Debug.Log("id failsafe: " + id);
    }
}
