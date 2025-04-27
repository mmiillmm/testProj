using UnityEngine;

public class Waypoint : MonoBehaviour
{
    public GameObject stopWindow;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Train"))
        {
            stopWindow.SetActive(true);
            TrainController controller = other.GetComponentInParent<TrainController>();
            if (controller != null)
                controller.StopTrain();
        }
    }
}
