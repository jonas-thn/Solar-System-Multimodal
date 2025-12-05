using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Windows.Speech; // Wichtig: Funktioniert nur auf Windows Standalone/Editor

public class VoiceRecognition : MonoBehaviour
{
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
        keywords.Add("Sonne", OnSonne);
        keywords.Add("Merkur", OnMerkur);
        keywords.Add("Venus", OnVenus);
        keywords.Add("Erde", OnErde);
        keywords.Add("Mars", OnMars);
        keywords.Add("Jupiter", OnJupiter);
        keywords.Add("Saturn", OnSaturn);
        keywords.Add("Uranus", OnUranus);
        keywords.Add("Neptun", OnNeptun);
        keywords.Add("Mond", OnMond);
        keywords.Add("System", OnSystem);

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
    }

    private void OnMerkur()
    {
        print("Merkur erkannt");
    }

    private void OnVenus()
    {
        print("Venus erkannt");
    }

    private void OnErde()
    {
        print("Erde erkannt");
    }

    private void OnMars()
    {
        print("Mars erkannt");
    }

    private void OnJupiter()
    {
        print("Jupiter erkannt");
    }

    private void OnSaturn()
    {
        print("Saturn erkannt");
    }

    private void OnUranus()
    {
        print("Uranus erkannt");
    }

    private void OnNeptun()
    {
        print("Neptun erkannt");
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