using UnityEngine;

public class VisibilityCulling : MonoBehaviour
{
    private Renderer objectRenderer;
    private Camera mainCamera;

    private void Start()
    {
        objectRenderer = GetComponent<Renderer>();
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (IsVisible())
        {
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    private bool IsVisible()
    {
        if (!objectRenderer)
            return false;

        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(mainCamera);
        return GeometryUtility.TestPlanesAABB(planes, objectRenderer.bounds);
    }
}
