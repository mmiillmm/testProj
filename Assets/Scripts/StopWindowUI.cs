using UnityEngine;
using UnityEngine.SceneManagement;

public class StopWindowUI : MonoBehaviour
{
    public GameObject stopWindow;
    public TrainController train;

    public void OnContinue()
    {
        stopWindow.SetActive(false);
        train.StartTrain();
    }

    public void OnStop()
    {
        stopWindow.SetActive(false);

        LoadNewScene("RogueLiteTest");
    }

    public void ShowWindow()
    {
        stopWindow.SetActive(true);
    }


    void LoadNewScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
