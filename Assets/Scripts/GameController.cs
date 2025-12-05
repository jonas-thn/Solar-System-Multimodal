using UnityEngine;
using UnityEngine.Rendering;

public class GameController : MonoBehaviour
{

    [SerializeField] private Transform mainCamera;
    [SerializeField] private float cameraMoveSpeed = 5f;
    [SerializeField] private Vector2 xBounds;
    [SerializeField] private Vector2 yBounds;
    [SerializeField] private Vector2 zBounds;

    public void UpdateHandData(Vector3 screenPos, string gesture)
    {
        float x = (screenPos.x - 0.5f) * 15f;
        float y = (screenPos.y - 0.5f) * 10f;

        UpdateCamera(x, y);

        switch (gesture)
        {
            case "Fist":
                break;

            case "Point":
                break;

            case "Open":
                break;

            default:
                break;
        }
    }

    private void UpdateCamera(float x, float y)
    {
        float cameraXPos = Mathf.Clamp(mainCamera.position.x + x * Time.deltaTime * cameraMoveSpeed, xBounds.x, xBounds.y);
        float cameraYPos = Mathf.Clamp(mainCamera.position.y + y * Time.deltaTime * cameraMoveSpeed, yBounds.x, yBounds.y);
        mainCamera.position = new Vector3(cameraXPos, cameraYPos, mainCamera.position.z);
    }
}