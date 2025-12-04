using UnityEngine;
using System.Collections.Generic;
using Mediapipe; // Basis Namespace
using Mediapipe.Unity;

public class HandDataProvider : MonoBehaviour
{
    public static HandDataProvider Instance;

    public bool isHandDetected = false;
    public Vector3 wristPosition = Vector3.zero;
    public bool isFist = false;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void OnHandDataReceived(List<NormalizedLandmarkList> multiHandLandmarks)
    {
        if (multiHandLandmarks == null || multiHandLandmarks.Count == 0)
        {
            isHandDetected = false;
            return;
        }

        NormalizedLandmarkList hand = multiHandLandmarks[0];

        if (hand.Landmark == null || hand.Landmark.Count < 21)
        {
            isHandDetected = false;
            return;
        }

        isHandDetected = true;

        var rawWrist = hand.Landmark[0];
        wristPosition = new Vector3(rawWrist.X, 1f - rawWrist.Y, 0);
        isFist = CheckIfFist(hand.Landmark);
    }

    private bool CheckIfFist(IList<NormalizedLandmark> landmarks)
    {
        Vector2 wrist = new Vector2(landmarks[0].X, landmarks[0].Y);
        Vector2 tip = new Vector2(landmarks[12].X, landmarks[12].Y);
        Vector2 knuckle = new Vector2(landmarks[9].X, landmarks[9].Y);

        return Vector2.Distance(wrist, tip) < (Vector2.Distance(wrist, knuckle) * 1.5f);
    }
}