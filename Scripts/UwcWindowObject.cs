﻿using UnityEngine;

namespace uWindowCapture
{

public class UwcWindowObject : MonoBehaviour
{
    const int MIDDLE_PRIORITY_MAX_Z = 5;

    public Window window { get; set; }
    public UwcWindowObject parent;

    public CaptureMode captureMode = CaptureMode.PrintWindow;
    public int skipFrame = 10;
    public bool updateTitleEveryFrame = false;

    int updatedFrame_ = 0;
    Material material_;
    Renderer renderer_;
    bool hasBeenCaptured_ = false;

    void Awake()
    {
        renderer_ = GetComponent<Renderer>();
        material_ = renderer_.material; // clone
    }

    void Start()
    {
        captureMode = window.captureMode;
        window.RequestCapture(CapturePriority.High);
        window.onCaptured.AddListener(OnCaptured);
        renderer_.enabled = false;
    }

    void Update()
    {
        if (material_.mainTexture != window.texture) {
            material_.mainTexture = window.texture;
        }

        if (updateTitleEveryFrame) {
            gameObject.name = window.title;
        }

        if (hasBeenCaptured_) {
            renderer_.enabled = !window.isIconic && window.isVisible;
        }

        updatedFrame_++;
    }

    void OnWillRenderObject()
    {
        window.captureMode = captureMode;

        if (updatedFrame_ % skipFrame == 0) {
            var priority = CapturePriority.Low;
            if (window == UwcManager.cursorWindow) {
                priority = CapturePriority.High;
            } else if (window.zOrder < MIDDLE_PRIORITY_MAX_Z) {
                priority = CapturePriority.Middle;
            }
            window.RequestCapture(priority);
        }
    }

    void OnCaptured()
    {
        hasBeenCaptured_ = true;
    }
}

}