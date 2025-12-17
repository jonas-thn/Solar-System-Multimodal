using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Windows.Speech; 

public class VoiceRecognition : MonoBehaviour
{
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
        //keywords.Add("Mond", OnMond);
        //keywords.Add("System", OnSystem);

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
    }

    private void OnMerkur()
    {
        print("Merkur erkannt");
        mainCam.Target.TrackingTarget = merkurTarget;
    }

    private void OnVenus()
    {
        print("Venus erkannt");
        mainCam.Target.TrackingTarget = venusTarget;
    }

    private void OnErde()
    {
        print("Erde erkannt");
        mainCam.Target.TrackingTarget = erdeTarget;
    }

    private void OnMars()
    {
        print("Mars erkannt");
        mainCam.Target.TrackingTarget = marsTarget;
    }

    private void OnJupiter()
    {
        print("Jupiter erkannt");
        mainCam.Target.TrackingTarget = jupiterTarget;
    }

    private void OnSaturn()
    {
        print("Saturn erkannt");
        mainCam.Target.TrackingTarget = saturnTarget;
    }

    private void OnUranus()
    {
        print("Uranus erkannt");
        mainCam.Target.TrackingTarget = uranusTarget;
    }

    private void OnNeptun()
    {
        print("Neptun erkannt");
        mainCam.Target.TrackingTarget = neptunTarget;
    }

    private void OnMond()
    {
        print("Mond erkannt");
    }

    private void OnSystem()
    {
        print("System erkannt");
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