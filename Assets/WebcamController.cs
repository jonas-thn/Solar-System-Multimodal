using UnityEngine;
using UnityEngine.UI; // Wichtig für RawImage

public class WebcamController : MonoBehaviour
{
    public RawImage displayImage;

    private WebCamTexture webcamTexture;

    void Start()
    {
        WebCamDevice[] devices = WebCamTexture.devices;

        if (devices.Length == 0)
        {
            Debug.LogError("Keine Webcam gefunden!");
            return;
        }

        Debug.Log("Benutze Kamera: " + devices[0].name);

        webcamTexture = new WebCamTexture(devices[0].name, 1280, 720, 30);

        displayImage.texture = webcamTexture;

        webcamTexture.Play();
    }

    void OnDestroy()
    {
        if (webcamTexture != null && webcamTexture.isPlaying)
        {
            webcamTexture.Stop();
        }
    }
}