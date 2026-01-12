// Copyright (c) 2023 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using Mediapipe.Tasks.Vision.HandLandmarker;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
using Mediapipe;
using Mediapipe.Unity;
using Mediapipe.Unity.Sample.HandLandmarkDetection;
using Mediapipe.Unity.Sample;

//Script um Hand-Gestiken mit Mediapipe zu erkennen
public class HandRunner : VisionTaskApiRunner<HandLandmarker>
{
    //UI Annotation
    [SerializeField] private HandLandmarkerResultAnnotationController _handLandmarkerResultAnnotationController;

    private Mediapipe.Unity.Experimental.TextureFramePool _textureFramePool;

    //Config laden
    public readonly HandLandmarkDetectionConfig config = new HandLandmarkDetectionConfig();

    //Unity Event Setup
    [System.Serializable]
    public class HandDataEvent : UnityEvent<Vector3, string> { }

    //Hand Daten über Event weitergeben
    [Header("GAME EVENTS")]
    public HandDataEvent onHandDataReceived;

    //wichtige Daten
    private Vector3 _latestPosition;
    private string _latestGesture = "Unknown";
    private bool _hasNewData = false;
    private object _dataLock = new object();

    //auf neue Daten testen
    private void Update()
    {
        if (_hasNewData)
        {
            Vector3 posToSend;
            string gestureToSend;

            lock (_dataLock)
            {
                posToSend = _latestPosition;
                gestureToSend = _latestGesture;
                _hasNewData = false;
            }
            //Callback auslösen
            onHandDataReceived?.Invoke(posToSend, gestureToSend);
        }
    }

    public override void Stop()
    {
        base.Stop();
        _textureFramePool?.Dispose();
        _textureFramePool = null;
    }

    //Run Preset (von Mediapipe Assets)
    protected override IEnumerator Run()
    {
        Debug.Log($"Delegate = {config.Delegate}");
        Debug.Log($"Image Read Mode = {config.ImageReadMode}");
        Debug.Log($"Running Mode = {config.RunningMode}");
        Debug.Log($"NumHands = {config.NumHands}");
        Debug.Log($"MinHandDetectionConfidence = {config.MinHandDetectionConfidence}");
        Debug.Log($"MinHandPresenceConfidence = {config.MinHandPresenceConfidence}");
        Debug.Log($"MinTrackingConfidence = {config.MinTrackingConfidence}");

        yield return AssetLoader.PrepareAssetAsync(config.ModelPath);

        var options = config.GetHandLandmarkerOptions(config.RunningMode == Mediapipe.Tasks.Vision.Core.RunningMode.LIVE_STREAM ? OnHandLandmarkDetectionOutput : null);
        taskApi = HandLandmarker.CreateFromOptions(options, GpuManager.GpuResources);
        var imageSource = ImageSourceProvider.ImageSource;

        yield return imageSource.Play();

        if (!imageSource.isPrepared)
        {
            Debug.LogError("Failed to start ImageSource, exiting...");
            yield break;
        }

        _textureFramePool = new Mediapipe.Unity.Experimental.TextureFramePool(imageSource.textureWidth, imageSource.textureHeight, TextureFormat.RGBA32, 10);

        screen.Initialize(imageSource);

        SetupAnnotationController(_handLandmarkerResultAnnotationController, imageSource);

        var transformationOptions = imageSource.GetTransformationOptions();
        var flipHorizontally = transformationOptions.flipHorizontally;
        var flipVertically = transformationOptions.flipVertically;
        var imageProcessingOptions = new Mediapipe.Tasks.Vision.Core.ImageProcessingOptions(rotationDegrees: (int)transformationOptions.rotationAngle);

        AsyncGPUReadbackRequest req = default;
        var waitUntilReqDone = new WaitUntil(() => req.done);
        var waitForEndOfFrame = new WaitForEndOfFrame();
        var result = HandLandmarkerResult.Alloc(options.numHands);

        var canUseGpuImage = SystemInfo.graphicsDeviceType == GraphicsDeviceType.OpenGLES3 && GpuManager.GpuResources != null;
        using var glContext = canUseGpuImage ? GpuManager.GetGlContext() : null;

        while (true)
        {
            if (isPaused)
            {
                yield return new WaitWhile(() => isPaused);
            }

            if (!_textureFramePool.TryGetTextureFrame(out var textureFrame))
            {
                yield return new WaitForEndOfFrame();
                continue;
            }

            Mediapipe.Image image;
            switch (config.ImageReadMode)
            {
                case ImageReadMode.GPU:
                    if (!canUseGpuImage)
                    {
                        throw new System.Exception("ImageReadMode.GPU is not supported");
                    }
                    textureFrame.ReadTextureOnGPU(imageSource.GetCurrentTexture(), flipHorizontally, flipVertically);
                    image = textureFrame.BuildGPUImage(glContext);
                    yield return waitForEndOfFrame;
                    break;
                case ImageReadMode.CPU:
                    yield return waitForEndOfFrame;
                    textureFrame.ReadTextureOnCPU(imageSource.GetCurrentTexture(), flipHorizontally, flipVertically);
                    image = textureFrame.BuildCPUImage();
                    textureFrame.Release();
                    break;
                case ImageReadMode.CPUAsync:
                default:
                    req = textureFrame.ReadTextureAsync(imageSource.GetCurrentTexture(), flipHorizontally, flipVertically);
                    yield return waitUntilReqDone;

                    if (req.hasError)
                    {
                        Debug.LogWarning($"Failed to read texture from the image source");
                        continue;
                    }
                    image = textureFrame.BuildCPUImage();
                    textureFrame.Release();
                    break;
            }

            switch (taskApi.runningMode)
            {
                case Mediapipe.Tasks.Vision.Core.RunningMode.IMAGE:
                    if (taskApi.TryDetect(image, imageProcessingOptions, ref result))
                    {
                        _handLandmarkerResultAnnotationController.DrawNow(result);
                    }
                    else
                    {
                        _handLandmarkerResultAnnotationController.DrawNow(default);
                    }
                    break;
                case Mediapipe.Tasks.Vision.Core.RunningMode.VIDEO:
                    if (taskApi.TryDetectForVideo(image, GetCurrentTimestampMillisec(), imageProcessingOptions, ref result))
                    {
                        _handLandmarkerResultAnnotationController.DrawNow(result);
                    }
                    else
                    {
                        _handLandmarkerResultAnnotationController.DrawNow(default);
                    }
                    break;
                case Mediapipe.Tasks.Vision.Core.RunningMode.LIVE_STREAM:
                    taskApi.DetectAsync(image, GetCurrentTimestampMillisec(), imageProcessingOptions);
                    break;
            }
        }
    }

    //Fingerpositionen testen mit Hilfe der Abstände
    private bool IsFingerOpen(Mediapipe.Tasks.Components.Containers.NormalizedLandmark wrist,
                              Mediapipe.Tasks.Components.Containers.NormalizedLandmark tip,
                              Mediapipe.Tasks.Components.Containers.NormalizedLandmark pip)
    {
        float distTip = Vector2.Distance(new Vector2(wrist.x, wrist.y), new Vector2(tip.x, tip.y));
        float distPip = Vector2.Distance(new Vector2(wrist.x, wrist.y), new Vector2(pip.x, pip.y));
        return distTip > distPip;
    }

    //Callback setzt wichtige Daten
    private void OnHandLandmarkDetectionOutput(HandLandmarkerResult result, Mediapipe.Image image, long timestamp)
    {
        _handLandmarkerResultAnnotationController.DrawLater(result);

        if (result.handLandmarks != null && result.handLandmarks.Count > 0)
        {
            var hand = result.handLandmarks[0];

            if (hand.landmarks != null && hand.landmarks.Count >= 21)
            {
                //Zeigefinger Position ist entscheidend
                var trackNode = hand.landmarks[7];
                Vector3 currentPos = new Vector3(trackNode.x, 1f - trackNode.y, 0);

                var wrist = hand.landmarks[0];

                //Geste testen
                bool indexOpen = IsFingerOpen(wrist, hand.landmarks[8], hand.landmarks[6]);
                bool middleOpen = IsFingerOpen(wrist, hand.landmarks[12], hand.landmarks[10]);
                bool ringOpen = IsFingerOpen(wrist, hand.landmarks[16], hand.landmarks[14]);
                bool pinkyOpen = IsFingerOpen(wrist, hand.landmarks[20], hand.landmarks[18]);

                string detectedGesture = "Unknown";

                //Geste setzen
                if (indexOpen && !middleOpen && !ringOpen && !pinkyOpen)
                {
                    detectedGesture = "Point";
                }
                else if (indexOpen && middleOpen && ringOpen && pinkyOpen)
                {
                    detectedGesture = "Open";
                }
                else if (!indexOpen && !middleOpen && !ringOpen && !pinkyOpen)
                {
                    detectedGesture = "Fist";
                }

                lock (_dataLock)
                {
                    _latestPosition = currentPos;
                    _latestGesture = detectedGesture;
                    _hasNewData = true;
                }
            }
        }
    }
}

