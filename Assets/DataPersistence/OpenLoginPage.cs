using UnityEngine;

public class OpenLoginPage : MonoBehaviour
{
    public string loginUrl = "http://localhost/project-main/logreg.html";

    public void OpenLogin()
    {
        Application.OpenURL(loginUrl);
        Debug.Log("Opening login page: " + loginUrl);
    }
}
