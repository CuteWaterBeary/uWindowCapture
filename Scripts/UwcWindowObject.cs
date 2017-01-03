﻿using UnityEngine;
using System.Collections;

namespace uWindowCapture
{

public class UwcWindowObject : MonoBehaviour
{
    public Window window { get; set; }
    public int lastUpdatedTime { get; set; }
    public CaptureMode captureMode = CaptureMode.PrintWindow;

    public int skipFrame = 10;
    int updatedFrame = 0;

    Material material_;
    bool hasCaptured_ = false;

    IEnumerator Start()
    {
        captureMode = window.captureMode;
        material_ = GetComponent<Renderer>().material; // clone
        window.onSizeChanged += OnSizeChanged;

        yield return new WaitForSeconds(0.1f);
        window.onCaptured += OnCaptured;
    }

    void OnDisable()
    {
        window.onCaptured -= OnCaptured;
        window.onSizeChanged -= OnSizeChanged;
    }

    void Update()
    {
        if (material_.mainTexture != window.texture) {
            material_.mainTexture = window.texture;
        }

        if (!hasCaptured_) {
            window.shouldBeUpdated = true;
        }
    }

    void OnWillRenderObject()
    {
        window.captureMode = captureMode;

        if (window.handle != Lib.GetForegroundWindow()) return;
        if (window.isHungup) return;

        if (updatedFrame % skipFrame == 0) {
            window.shouldBeUpdated = true;
        }

        updatedFrame++;
    }

    void OnCaptured()
    {
        hasCaptured_ = true;
        window.onCaptured -= OnCaptured;
    }

    void OnSizeChanged()
    {
        window.shouldBeUpdated = true;
    }
}

}