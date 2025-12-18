using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Windows.Speech; 

public class VoiceRecognition : MonoBehaviour
{
    [SerializeField] private GameController gameController;

    [SerializeField] private Transform sonneTarget;
    [SerializeField] private Transform merkurTarget;
    [SerializeField] private Transform venusTarget;
    [SerializeField] private Transform erdeTarget;
    [SerializeField] private Transform marsTarget;
    [SerializeField] private Transform jupiterTarget;
    [SerializeField] private Transform saturnTarget;
    [SerializeField] private Transform uranusTarget;
    [SerializeField] private Transform neptunTarget;

    [SerializeField] private CinemachineCamera mainCam;

    public static VoiceRecognition Instance;

    private KeywordRecognizer keywordRecognizer;

    private Dictionary<string, Action> keywords = new Dictionary<string, Action>();

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        keywords.Add("Zeig mir die Sonne", OnSonne);
        keywords.Add("Sonne", OnSonne);
        keywords.Add("Zeig mir den Merkur", OnMerkur);
        keywords.Add("Merkur", OnMerkur);
        keywords.Add("Venus", OnVenus);
        keywords.Add("Erde", OnErde);
        keywords.Add("Mars", OnMars);
        keywords.Add("Jupiter", OnJupiter);
        keywords.Add("Saturn", OnSaturn);
        keywords.Add("Uranus", OnUranus);
        keywords.Add("Neptun", OnNeptun);

        keywordRecognizer = new KeywordRecognizer(keywords.Keys.ToArray());

        keywordRecognizer.OnPhraseRecognized += KeywordRecognizer_OnPhraseRecognized;

        keywordRecognizer.Start();
    }

    private void KeywordRecognizer_OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        if (keywords.ContainsKey(args.text))
        {
            keywords[args.text].Invoke();
        }
    }

    private void OnSonne()
    {
        print("Sonne erkannt");
        mainCam.Target.TrackingTarget = sonneTarget;
        gameController.ChangeCameraProperties(20.0f);
    }

    private void OnMerkur()
    {
        print("Merkur erkannt");
        mainCam.Target.TrackingTarget = merkurTarget;
        gameController.ChangeCameraProperties(6.0f);
    }

    private void OnVenus()
    {
        print("Venus erkannt");
        mainCam.Target.TrackingTarget = venusTarget;
        gameController.ChangeCameraProperties(7.0f);
    }

    private void OnErde()
    {
        print("Erde erkannt");
        mainCam.Target.TrackingTarget = erdeTarget;
        gameController.ChangeCameraProperties(9.0f);
    }

    private void OnMars()
    {
        print("Mars erkannt");
        mainCam.Target.TrackingTarget = marsTarget;
        gameController.ChangeCameraProperties(8.0f);
    }

    private void OnJupiter()
    {
        print("Jupiter erkannt");
        mainCam.Target.TrackingTarget = jupiterTarget;
        gameController.ChangeCameraProperties(10.0f);
    }

    private void OnSaturn()
    {
        print("Saturn erkannt");
        mainCam.Target.TrackingTarget = saturnTarget;
        gameController.ChangeCameraProperties(10.0f);
    }

    private void OnUranus()
    {
        print("Uranus erkannt");
        mainCam.Target.TrackingTarget = uranusTarget;
        gameController.ChangeCameraProperties(9.0f);
    }

    private void OnNeptun()
    {
        print("Neptun erkannt");
        mainCam.Target.TrackingTarget = neptunTarget;
        gameController.ChangeCameraProperties(9.0f);
    }

    private void OnDestroy()
    {
        if (keywordRecognizer != null && keywordRecognizer.IsRunning)
        {
            keywordRecognizer.Stop();
            keywordRecognizer.Dispose();
        }
    }
}