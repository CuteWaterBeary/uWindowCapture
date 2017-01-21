﻿using System;
using System.Runtime.InteropServices;

#pragma warning disable 114, 465

namespace uWindowCapture
{

public enum DebugMode
{
    None = 0,
    File = 1,
    UnityLog = 2, /* currently has bug when app exits. */
}

public enum CaptureMode
{
    None = -1,
    PrintWindow = 0,
    BitBlt = 1,
    BitBltAlpha = 2,
}

public enum CapturePriority
{
    High = 0,
    Middle = 1,
    Low = 2,
}

public enum MessageType
{
    None = -1,
    WindowAdded = 0,
    WindowRemoved = 1,
    WindowCaptured = 2,
    WindowSizeChanged = 3,
}

[StructLayout(LayoutKind.Sequential)]
public struct Message
{
    [MarshalAs(UnmanagedType.I4)]
    public MessageType type;
    [MarshalAs(UnmanagedType.I4)]
    public int windowId;
    [MarshalAs(UnmanagedType.I8)]
    public IntPtr windowHandle;
}

[StructLayout(LayoutKind.Sequential)]
public struct Point
{
    [MarshalAs(UnmanagedType.I4)]
    public int x;
    [MarshalAs(UnmanagedType.I4)]
    public int y;
}

public static class Lib
{
    public const string name = "uWindowCapture";

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void DebugLogDelegate(string str);

    [DllImport(name, EntryPoint = "UwcInitialize")]
    public static extern void Initialize();
    [DllImport(name, EntryPoint = "UwcFinalize")]
    public static extern void Finalize();
    [DllImport(name, EntryPoint = "UwcSetDebugMode")]
    public static extern void SetDebugMode(DebugMode mode);
    [DllImport(name, EntryPoint = "UwcSetLogFunc")]
    public static extern void SetLogFunc(DebugLogDelegate func);
    [DllImport(name, EntryPoint = "UwcSetErrorFunc")]
    public static extern void SetErrorFunc(DebugLogDelegate func);
    [DllImport(name, EntryPoint = "UwcGetRenderEventFunc")]
    public static extern IntPtr GetRenderEventFunc();
    [DllImport(name, EntryPoint = "UwcUpdate")]
    public static extern void Update();
    [DllImport(name, EntryPoint = "UwcTriggerGpuUpload")]
    public static extern void TriggerGpuUpload();
    [DllImport(name, EntryPoint = "UwcGetMessageCount")]
    private static extern int GetMessageCount();
    [DllImport(name, EntryPoint = "UwcGetMessages")]
    private static extern IntPtr GetMessages_Internal();
    [DllImport(name, EntryPoint = "UwcClearMessages")]
    private static extern void ClearMessages();
    [DllImport(name, EntryPoint = "UwcGetWindowHandle")]
    public static extern IntPtr GetWindowHandle(int id);
    [DllImport(name, EntryPoint = "UwcGetWindowOwner")]
    public static extern IntPtr GetWindowOwner(int id);
    [DllImport(name, EntryPoint = "UwcGetWindowParent")]
    public static extern IntPtr GetWindowParent(int id);
    [DllImport(name, EntryPoint = "UwcGetWindowInstance")]
    public static extern IntPtr GetWindowInstance(int id);
    [DllImport(name, EntryPoint = "UwcGetWindowProcessId")]
    public static extern int GetWindowProcessId(int id);
    [DllImport(name, EntryPoint = "UwcGetWindowThreadId")]
    public static extern int GetWindowThreadId(int id);
    [DllImport(name, EntryPoint = "UwcRequestCaptureWindow")]
    public static extern void RequestCaptureWindow(int id, CapturePriority priority);
    [DllImport(name, EntryPoint = "UwcGetWindowX")]
    public static extern int GetWindowX(int id);
    [DllImport(name, EntryPoint = "UwcGetWindowY")]
    public static extern int GetWindowY(int id);
    [DllImport(name, EntryPoint = "UwcGetWindowWidth")]
    public static extern int GetWindowWidth(int id);
    [DllImport(name, EntryPoint = "UwcGetWindowHeight")]
    public static extern int GetWindowHeight(int id);
    [DllImport(name, EntryPoint = "UwcGetWindowZOrder")]
    public static extern int GetWindowZOrder(int id);
    [DllImport(name, EntryPoint = "UwcGetWindowBufferWidth")]
    public static extern int GetWindowBufferWidth(int id);
    [DllImport(name, EntryPoint = "UwcGetWindowBufferHeight")]
    public static extern int GetWindowBufferHeight(int id);
    [DllImport(name, EntryPoint = "UwcUpdateWindowTitle")]
    private static extern void UpdateWindowTitle(int id);
    [DllImport(name, EntryPoint = "UwcGetWindowTitleLength")]
    private static extern int GetWindowTitleLength(int id);
    [DllImport(name, EntryPoint = "UwcGetWindowTitle", CharSet = CharSet.Unicode)]
    private static extern IntPtr GetWindowTitle_Internal(int id);
    [DllImport(name, EntryPoint = "UwcGetWindowTexturePtr")]
    public static extern IntPtr GetWindowTexturePtr(int id);
    [DllImport(name, EntryPoint = "UwcSetWindowTexturePtr")]
    public static extern void SetWindowTexturePtr(int id, IntPtr texturePtr);
    [DllImport(name, EntryPoint = "UwcGetWindowCaptureMode")]
    public static extern CaptureMode GetWindowCaptureMode(int id);
    [DllImport(name, EntryPoint = "UwcSetWindowCaptureMode")]
    public static extern void SetWindowCaptureMode(int id, CaptureMode mode);
    [DllImport(name, EntryPoint = "UwcIsWindow")]
    public static extern bool IsWindow(int id);
    [DllImport(name, EntryPoint = "UwcIsWindowVisible")]
    public static extern bool IsWindowVisible(int id);
    [DllImport(name, EntryPoint = "UwcIsAltTabWindow")]
    public static extern bool IsAltTabWindow(int id);
    [DllImport(name, EntryPoint = "UwcIsDesktop")]
    public static extern bool IsDesktop(int id);
    [DllImport(name, EntryPoint = "UwcIsWindowEnabled")]
    public static extern bool IsWindowEnabled(int id);
    [DllImport(name, EntryPoint = "UwcIsWindowUnicode")]
    public static extern bool IsWindowUnicode(int id);
    [DllImport(name, EntryPoint = "UwcIsWindowZoomed")]
    public static extern bool IsWindowZoomed(int id);
    [DllImport(name, EntryPoint = "UwcIsWindowIconic")]
    public static extern bool IsWindowIconic(int id);
    [DllImport(name, EntryPoint = "UwcIsWindowHungUp")]
    public static extern bool IsWindowHungUp(int id);
    [DllImport(name, EntryPoint = "UwcIsWindowTouchable")]
    public static extern bool IsWindowTouchable(int id);
    [DllImport(name, EntryPoint = "UwcMoveWindow")]
    public static extern bool MoveWindow(int id, int x, int y);
    [DllImport(name, EntryPoint = "UwcScaleWindow")]
    public static extern bool ScaleWindow(int id, int width, int height);
    [DllImport(name, EntryPoint = "UwcMoveAndScaleWindow")]
    public static extern bool MoveAndScaleWindow(int id, int x, int y, int width, int height);
    [DllImport(name, EntryPoint = "UwcGetForegroundWindow")]
    public static extern IntPtr GetForegroundWindow();
    [DllImport(name, EntryPoint = "UwcGetCursorPosition")]
    public static extern Point GetCursorPosition();
    [DllImport(name, EntryPoint = "UwcGetWindowFromPoint")]
    public static extern IntPtr GetWindowFromPoint(int x, int y);
    [DllImport(name, EntryPoint = "UwcGetWindowUnderCursor")]
    public static extern IntPtr GetWindowUnderCursor();
    [DllImport(name, EntryPoint = "UwcGetScreenWidth")]
    public static extern int GetScreenWidth();
    [DllImport(name, EntryPoint = "UwcGetScreenHeight")]
    public static extern int GetScreenHeight();

    public static Message[] GetMessages()
    {
        var count = GetMessageCount();
        var messages = new Message[count];

        if (count == 0) return messages;

        var ptr = GetMessages_Internal();
        var size = Marshal.SizeOf(typeof(Message));

        for (int i = 0; i < count; ++i) {
            var data = new IntPtr(ptr.ToInt64() + (size * i));
            messages[i] = (Message)Marshal.PtrToStructure(data, typeof(Message));
        }

        ClearMessages();

        return messages;
    }

    public static string GetWindowTitle(int id)
    {
        UpdateWindowTitle(id);
        var len = GetWindowTitleLength(id);
        var ptr = GetWindowTitle_Internal(id);
        if (ptr != IntPtr.Zero) {
            return Marshal.PtrToStringUni(ptr, len);
        } else {
            return "";
        }
    }
}

}