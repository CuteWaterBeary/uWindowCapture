﻿using UnityEngine;
using System.Collections.Generic;

namespace uWindowCapture
{

[RequireComponent(typeof(UwcWindowObject))]
public class UwcWindowObjectChildrenManager : MonoBehaviour 
{
    [SerializeField] 
    GameObject childPrefab;

    [SerializeField] 
    [Tooltip("Distance per z-order")]
    float zDistance = 0.02f;

    UwcWindowObject windowObject_;
    Dictionary<int, UwcWindowObject> children = new Dictionary<int, UwcWindowObject>();

    void Awake()
    {
        windowObject_ = GetComponent<UwcWindowObject>();
        windowObject_.onWindowChanged.AddListener(OnWindowChanged);
        OnWindowChanged(windowObject_.window, null);
    }

    void Update()
    {
        UpdateChildren();
    }

    void OnWindowChanged(UwcWindow newWindow, UwcWindow oldWindow)
    {
        if (oldWindow != null) {
            oldWindow.onChildAdded.RemoveListener(OnChildAdded);
            oldWindow.onChildRemoved.RemoveListener(OnChildRemoved);
        }

        if (newWindow != null) {
            newWindow.onChildAdded.AddListener(OnChildAdded);
            newWindow.onChildRemoved.AddListener(OnChildRemoved);

            foreach (var pair in UwcManager.windows) {
                var window = pair.Value;
                if (window.isChild && window.parentWindow.id == newWindow.id) {
                    OnChildAdded(window);
                }
            }
        }
    }

    void OnChildAdded(UwcWindow window)
    {
        if (!childPrefab) return;

        var childObject = Instantiate(childPrefab, transform);
        var childWindowObejct = childObject.GetComponent<UwcWindowObject>();
        childWindowObejct.window = window;
        childWindowObejct.scale = windowObject_.scale;

        children.Add(window.id, childWindowObejct);
    }

    void OnChildRemoved(UwcWindow window)
    {
        UwcWindowObject child;
        children.TryGetValue(window.id, out child);
        if (child) {
            Destroy(child.gameObject);
            children.Remove(window.id);
        }
    }

    void MoveAndScaleChildWindow(UwcWindowObject child)
    {
        var window = child.window;
        var basePixel = child.basePixel;

        var parentRatioX = transform.lossyScale.x / windowObject_.width;
        var parentRatioY = transform.lossyScale.y / windowObject_.height;

        var parentDesktopPos = UwcWindowUtil.ConvertDesktopCoordToUnityPosition(window.parentWindow, basePixel);
        var childDesktopPos = UwcWindowUtil.ConvertDesktopCoordToUnityPosition(window, basePixel);
        var localPos = childDesktopPos - parentDesktopPos;
        localPos.x *= parentRatioX / transform.localScale.x;
        localPos.y *= parentRatioY / transform.localScale.y;
        localPos.z = zDistance * (window.zOrder - window.parentWindow.zOrder) / transform.localScale.z;
        child.transform.localPosition = localPos;

        var worldToLocal = transform.worldToLocalMatrix;
        var worldScale = new Vector3(child.width * parentRatioX, child.height * parentRatioY, 1f);
        var localScale = worldToLocal.MultiplyVector(worldScale);
        child.transform.localScale = localScale;
    }

    void UpdateChildren()
    {
        var enumerator = children.GetEnumerator();
        while (enumerator.MoveNext()) {
            var windowObject = enumerator.Current.Value;
            MoveAndScaleChildWindow(windowObject);
        }
    }
}

}