using UnityEngine;

public class WebOpen : MonoBehaviour
{
    public void OpenLoginPage()
    {
        Application.OpenURL("http://localhost/project/logreg.html");
    }
}
