using UnityEngine;
using UnityEngine.UI; 

//Test Script um Webcam zu prüfen
public class WebcamController : MonoBehaviour
{
    //UI Webcam Display
    public RawImage displayImage;

    private WebCamTexture webcamTexture;

    void Start()
    {
        //Webcam finden
        WebCamDevice[] devices = WebCamTexture.devices;

        if (devices.Length == 0)
        {
            Debug.LogError("Keine Webcam gefunden!");
            return;
        }

        Debug.Log("Benutze Kamera: " + devices[0].name);

        //Webcam Objekt
        webcamTexture = new WebCamTexture(devices[0].name, 1280, 720, 30);

        displayImage.texture = webcamTexture;

        //Webcam anzeigen
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