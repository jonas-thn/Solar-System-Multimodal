using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Windows.Speech; 

//Script zur Erkennung und Steuerung von Himmelskörpern mit Stimme
public class VoiceRecognition : MonoBehaviour
{
    //Abhängigkeiten
    [Header("Main")]
    [SerializeField] private GameController gameController;
    [SerializeField] private CinemachineCamera mainCam;

    //Kamera Ziele
    [Header("Targets")]
    [SerializeField] private Transform sonneTarget;
    [SerializeField] private Transform merkurTarget;
    [SerializeField] private Transform venusTarget;
    [SerializeField] private Transform erdeTarget;
    [SerializeField] private Transform marsTarget;
    [SerializeField] private Transform jupiterTarget;
    [SerializeField] private Transform saturnTarget;
    [SerializeField] private Transform uranusTarget;
    [SerializeField] private Transform neptunTarget;
    [SerializeField] private Transform plutoTarget;

    //UI Infos Anzeige
    [Header("UI")]
    [SerializeField] private GameObject sonneUI;
    [SerializeField] private GameObject mercuryUI;
    [SerializeField] private GameObject venusUI;
    [SerializeField] private GameObject earthUI;
    [SerializeField] private GameObject marsUI;
    [SerializeField] private GameObject jupiterUI;
    [SerializeField] private GameObject saturnUI;
    [SerializeField] private GameObject uranusUI;
    [SerializeField] private GameObject naptunUI;
    [SerializeField] private GameObject plutoUI;

    //Windows Voice Erkennung
    public static VoiceRecognition Instance;
    private KeywordRecognizer keywordRecognizer;

    //Eingabe zu Funktions-Delegate Map
    private Dictionary<string, Action> keywords = new Dictionary<string, Action>();

    private List<GameObject> uiObjects = new List<GameObject>();

    //Singleton
    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    //List Setup
    void Start()
    {
        uiObjects.Add(sonneUI);
        uiObjects.Add(mercuryUI);
        uiObjects.Add(venusUI);
        uiObjects.Add(earthUI);
        uiObjects.Add(marsUI);
        uiObjects.Add(jupiterUI);
        uiObjects.Add(saturnUI);
        uiObjects.Add(uranusUI);
        uiObjects.Add(naptunUI);

        keywords.Add("Zeig mir die Sonne", OnSonne);
        keywords.Add("Sonne", OnSonne);
        keywords.Add("Zeig mir den Merkur", OnMerkur);
        keywords.Add("Merkur", OnMerkur);
        keywords.Add("Venus", OnVenus);
        keywords.Add("Zeig mir die Venus", OnVenus);
        keywords.Add("Erde", OnErde);
        keywords.Add("Zeig mir die Erde", OnErde);
        keywords.Add("Mars", OnMars);
        keywords.Add("Zeig mir den Mars", OnMars);
        keywords.Add("Jupiter", OnJupiter);
        keywords.Add("Zeig mir den Jupiter", OnJupiter);
        keywords.Add("Saturn", OnSaturn);
        keywords.Add("Zeig mir den Saturn", OnSaturn);
        keywords.Add("Uranus", OnUranus);
        keywords.Add("Zeig mir den Uranus", OnUranus);
        keywords.Add("Neptun", OnNeptun);
        keywords.Add("Zeig mir den Neptun", OnNeptun);
        keywords.Add("Pluto", OnPluto);
        keywords.Add("Zeig mir den Pluto", OnPluto);

        keywords.Add("Schließen", OnQuit);
        keywords.Add("Beenden", OnQuit);


        //Keyword Recognizer mit Werten füttern
        keywordRecognizer = new KeywordRecognizer(keywords.Keys.ToArray());

        //callback starten
        keywordRecognizer.OnPhraseRecognized += KeywordRecognizer_OnPhraseRecognized;

        keywordRecognizer.Start();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            OnPluto();
        }
    }

    private void OnQuit()
    {
        print("quit");
        Application.Quit();
    }

    //callack
    private void KeywordRecognizer_OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        if (keywords.ContainsKey(args.text))
        {
            //delegat auslösen
            keywords[args.text].Invoke();
        }
    }

    //Funktionen der Himmelskörper richten Kamera aus und aktivieren UI
    private void OnSonne()
    {
        print("Sonne erkannt");
        mainCam.Target.TrackingTarget = sonneTarget;
        gameController.ChangeCameraProperties(20.0f);

        foreach (var uiObject in uiObjects)
        {
            uiObject.SetActive(false);
        }

        sonneUI.SetActive(true);
    }

    private void OnMerkur()
    {
        print("Merkur erkannt");
        mainCam.Target.TrackingTarget = merkurTarget;
        gameController.ChangeCameraProperties(6.0f);

        foreach (var uiObject in uiObjects)
        {
            uiObject.SetActive(false);
        }

        mercuryUI.SetActive(true);
    }

    private void OnVenus()
    {
        print("Venus erkannt");
        mainCam.Target.TrackingTarget = venusTarget;
        gameController.ChangeCameraProperties(7.0f);

        foreach (var uiObject in uiObjects)
        {
            uiObject.SetActive(false);
        }

        venusUI.SetActive(true);
    }

    private void OnErde()
    {
        print("Erde erkannt");
        mainCam.Target.TrackingTarget = erdeTarget;
        gameController.ChangeCameraProperties(9.0f);

        foreach (var uiObject in uiObjects)
        {
            uiObject.SetActive(false);
        }

        earthUI.SetActive(true);
    }

    private void OnMars()
    {
        print("Mars erkannt");
        mainCam.Target.TrackingTarget = marsTarget;
        gameController.ChangeCameraProperties(8.0f);

        foreach (var uiObject in uiObjects)
        {
            uiObject.SetActive(false);
        }

        marsUI.SetActive(true);
    }

    private void OnJupiter()
    {
        print("Jupiter erkannt");
        mainCam.Target.TrackingTarget = jupiterTarget;
        gameController.ChangeCameraProperties(10.0f);

        foreach (var uiObject in uiObjects)
        {
            uiObject.SetActive(false);
        }

        jupiterUI.SetActive(true);
    }

    private void OnSaturn()
    {
        print("Saturn erkannt");
        mainCam.Target.TrackingTarget = saturnTarget;
        gameController.ChangeCameraProperties(10.0f);

        foreach (var uiObject in uiObjects)
        {
            uiObject.SetActive(false);
        }

        saturnUI.SetActive(true);
    }

    private void OnUranus()
    {
        print("Uranus erkannt");
        mainCam.Target.TrackingTarget = uranusTarget;
        gameController.ChangeCameraProperties(9.0f);

        foreach (var uiObject in uiObjects)
        {
            uiObject.SetActive(false);
        }

        uranusUI.SetActive(true);
    }

    private void OnNeptun()
    {
        print("Neptun erkannt");
        mainCam.Target.TrackingTarget = neptunTarget;
        gameController.ChangeCameraProperties(9.0f);

        foreach (var uiObject in uiObjects)
        {
            uiObject.SetActive(false);
        }

        naptunUI.SetActive(true);
    }

    private void OnPluto()
    {
        print("Pluto erkannt");
        mainCam.Target.TrackingTarget = plutoTarget;
        gameController.ChangeCameraProperties(5.0f);

        foreach (var uiObject in uiObjects)
        {
            uiObject.SetActive(false);
        }

        plutoUI.SetActive(true);
    }

    //Keywoard Recognizer löschen
    private void OnDestroy()
    {
        if (keywordRecognizer != null && keywordRecognizer.IsRunning)
        {
            keywordRecognizer.Stop();
            keywordRecognizer.Dispose();
        }
    }
}