using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class SessionLogin : MonoBehaviour
{
    [SerializeField] private string getUserURL = "http://localhost/project/backend/active_user.txt";

    public void GetActiveUser()
    {
        StartCoroutine(FetchActiveUser());
    }

    private IEnumerator FetchActiveUser()
    {
        UnityWebRequest www = UnityWebRequest.Get(getUserURL);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            string username = www.downloadHandler.text.Trim();
            Debug.Log("bejel user: " + username);

            if (!string.IsNullOrEmpty(username))
            {
                UserLogin.playerID = username;
            }
            else
            {
                Debug.LogWarning("nincs logolva senki.");
            }
        }
        else
        {
            Debug.LogError("aktiv user nincs meg" + www.error);
        }
    }
}
