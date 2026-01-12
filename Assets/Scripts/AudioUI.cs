using UnityEngine;
using UnityEngine.UI;

//Script um Lautstärke anzeige zu kontrollieren
public class AudioUI : MonoBehaviour
{
    //UI Slider
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private float sensitivity = 5f;

    private AudioSource audioSource;
    private string deviceName;
    private int sampleWindow = 128;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        //Mikrofon finden
        if (Microphone.devices.Length > 0)
        {
            deviceName = Microphone.devices[0];
            audioSource.clip = Microphone.Start(deviceName, true, 1, 44100);
        }
        else
        {
            Debug.LogError("Kein Mikrofon gefunden!");
        }
    }

    void Update()
    {
        //Slider aktualisieren
        float level = GetLevelMax();
        volumeSlider.value = Mathf.Lerp(volumeSlider.value, level * sensitivity, Time.deltaTime * 15f);
    }

    //abtasten
    float GetLevelMax()
    {
        float levelMax = 0;
        float[] waveData = new float[sampleWindow];
        int micPosition = Microphone.GetPosition(deviceName) - (sampleWindow + 1);

        if (micPosition < 0) return 0;

        audioSource.clip.GetData(waveData, micPosition);

        for (int i = 0; i < sampleWindow; i++)
        {
            float wavePeak = waveData[i] * waveData[i];
            if (levelMax < wavePeak)
            {
                levelMax = wavePeak;
            }
        }
        return Mathf.Sqrt(levelMax);
    }
}