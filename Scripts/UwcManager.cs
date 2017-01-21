﻿using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

namespace uWindowCapture
{

public class UwcManager : MonoBehaviour
{
    private static UwcManager instance_;
    public static UwcManager instance 
    {
        get { return CreateInstance(); }
    }

    public static UwcManager CreateInstance()
    {
        if (instance_ != null) return instance_;

        var manager = FindObjectOfType<UwcManager>();
        if (manager) {
            instance_ = manager;
            return manager;
        }

        var go = new GameObject("uWindowCapture");
        instance_ = go.AddComponent<UwcManager>();
        return instance_;
    }

    public DebugMode debugMode = DebugMode.File;
    public static event Lib.DebugLogDelegate onDebugLog = msg => Debug.Log(msg);
    public static event Lib.DebugLogDelegate onDebugErr = msg => Debug.LogError(msg);

    public class WindowAddedEvent : UnityEvent<Window> {}
    private WindowAddedEvent onWindowAdded_ = new WindowAddedEvent();
    public static WindowAddedEvent onWindowAdded
    {
        get { return instance.onWindowAdded_; }
    }

    public class WindowRemovedEvent : UnityEvent<System.IntPtr> {}
    private WindowRemovedEvent onWindowRemoved_ = new WindowRemovedEvent();
    public static WindowRemovedEvent onWindowRemoved
    {
        get { return instance.onWindowRemoved_; }
    }

    System.IntPtr renderEventFunc_;

    Dictionary<System.IntPtr, Window> windows_ = new Dictionary<System.IntPtr, Window>();
    static public Dictionary<System.IntPtr, Window> windows
    {
        get { return instance.windows_; }
    }

    System.IntPtr cursorWindowHandle_ = System.IntPtr.Zero;
    static public Window cursorWindow
    {
        get { return Find(instance.cursorWindowHandle_); }
    }

    void Awake()
    {
        Lib.SetDebugMode(debugMode);
        Lib.Initialize();
        renderEventFunc_ = Lib.GetRenderEventFunc();
    }

    void Start()
    {
        StartCoroutine(Render());
    }

    void OnApplicationQuit()
    {
        Lib.Finalize();
    }

    void OnEnable()
    {
        Lib.SetLogFunc(onDebugLog);
        Lib.SetErrorFunc(onDebugErr);
    }

    void OnDisable()
    {
        Lib.SetLogFunc(null);
        Lib.SetErrorFunc(null);
    }

    IEnumerator Render()
    {
        for (;;) {
            yield return new WaitForEndOfFrame();
            GL.IssuePluginEvent(renderEventFunc_, 0);
            Lib.TriggerGpuUpload();
        }
    }

    void Update()
    {
        Lib.Update();
        UpdateWindowInfo();
        UpdateMessages();
    }

    void UpdateWindowInfo()
    {
        cursorWindowHandle_ = Lib.GetWindowUnderCursor();
    }

    void UpdateMessages()
    {
        var messages = Lib.GetMessages();

        for (int i = 0; i < messages.Length; ++i) {
            var message = messages[i];
            switch (message.type) {
                case MessageType.WindowAdded: {
                    var window = new Window(message.windowHandle, message.windowId);
                    windows.Add(message.windowHandle, window);
                    onWindowAdded.Invoke(window);
                    break;
                }
                case MessageType.WindowRemoved: {
                    var window = Find(message.windowHandle);
                    if (window != null) {
                        window.isAlive = false;
                        onWindowRemoved.Invoke(message.windowHandle);
                        windows.Remove(message.windowHandle);
                    }
                    break;
                }
                case MessageType.WindowCaptured: {
                    var window = Find(message.windowHandle);
                    if (window != null && window.onCaptured != null) {
                        window.onCaptured.Invoke();
                    }
                    break;
                }
                case MessageType.WindowSizeChanged: {
                    var window = Find(message.windowHandle);
                    if (window != null && window.onSizeChanged != null) {
                        window.onSizeChanged.Invoke();
                    }
                    break;
                }
                case MessageType.IconCaptured: {
                    var window = Find(message.windowHandle);
                    if (window != null && window.onSizeChanged != null) {
                        window.onIconCaptured.Invoke();
                    }
                    break;
                }
                default: {
                    break;
                }
            }
        }
    }

    static public Window Find(System.IntPtr handle)
    {
        if (handle == System.IntPtr.Zero) return null;
        if (windows.ContainsKey(handle)) {
            return windows[handle];
        }
        return null;
    }

    static public Window Find(string title)
    {
        var enumerator = windows.GetEnumerator();
        while (enumerator.MoveNext()) {
            var window = enumerator.Current.Value;
            if (window.title.IndexOf(title) != -1) {
                return window;
            }
        }
        return null;
    }

    static public List<Window> FindAll(string title)
    {
        var list = new List<Window>();
        var enumerator = windows.GetEnumerator();
        while (enumerator.MoveNext()) {
            var window = enumerator.Current.Value;
            if (window.title.IndexOf(title) != -1) {
                list.Add(window);
            }
        }
        return list;
    }
}

}