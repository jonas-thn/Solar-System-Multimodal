using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Rendering;

public class GameController : MonoBehaviour
{

    [SerializeField] private CinemachineCamera mainCamera;

    [SerializeField] private float cameraZoomSpeed = 5.0f;
    [SerializeField] private float cameraRotationSpeed = 5.0f;

    [SerializeField] private Vector2 zoomBounds; //11, 150

    CinemachineOrbitalFollow followCam;

    private void Awake()
    {
        followCam = mainCamera.GetComponent<CinemachineOrbitalFollow>();
    }

    public void UpdateHandData(Vector3 screenPos, string gesture)
    {
        float x = (screenPos.x - 0.5f) * 15f;
        float y = (screenPos.y - 0.5f) * 10f;

           
        followCam.HorizontalAxis.Value += (x * cameraRotationSpeed * Time.deltaTime);

        switch (gesture)
        {
            case "Fist":
                followCam.Radius -= cameraZoomSpeed * Time.deltaTime;
                followCam.Radius = Mathf.Clamp(followCam.Radius, zoomBounds.x, zoomBounds.y);
                break;

            case "Point":
                followCam.Radius += cameraZoomSpeed * Time.deltaTime;
                followCam.Radius = Mathf.Clamp(followCam.Radius, zoomBounds.x, zoomBounds.y);
                break;

            case "Open":
                break;

            default:
                break;
        }
    }

    public void ChangeCameraProperties(float lowerBound)
    {
        zoomBounds = new Vector2(lowerBound, zoomBounds.y);

        if(followCam.Radius < lowerBound)
        {
            followCam.Radius = lowerBound;
        }
    }
}