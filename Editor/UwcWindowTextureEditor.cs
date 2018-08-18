﻿using UnityEngine;
using UnityEditor;

namespace uWindowCapture
{

[CustomEditor(typeof(UwcWindowTexture))]
public class UwcWindowTextureEditor : Editor
{
    UwcWindowTexture texture
    {
        get { return target as UwcWindowTexture; }
    }

    SerializedProperty captureMode;
    SerializedProperty capturePriority;
    SerializedProperty captureRequestTiming;
    SerializedProperty captureFrameRate;
    SerializedProperty cursorDraw;

    void OnEnable()
    {
        captureMode = serializedObject.FindProperty("captureMode");
        capturePriority = serializedObject.FindProperty("capturePriority");
        captureRequestTiming = serializedObject.FindProperty("captureRequestTiming");
        captureFrameRate = serializedObject.FindProperty("captureFrameRate");
        cursorDraw = serializedObject.FindProperty("cursorDraw");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.Space();

        DrawPartialWindowTitle();
        DrawCaptureSettings();

        serializedObject.ApplyModifiedProperties();
    }

    void DrawPartialWindowTitle()
    {
        if (texture.window != null && texture.window.isChild) return;

        var title = EditorGUILayout.TextField("Partial Window Title", texture.partialWindowTitle);
        if (title != texture.partialWindowTitle) {
            texture.partialWindowTitle = title;
        }
    }

    void DrawCaptureSettings()
    {
        EditorGUILayout.PropertyField(captureMode);
        EditorGUILayout.PropertyField(capturePriority);
        EditorGUILayout.PropertyField(captureRequestTiming);
        EditorGUILayout.PropertyField(captureFrameRate);
        EditorGUILayout.PropertyField(cursorDraw);
    }
}

}