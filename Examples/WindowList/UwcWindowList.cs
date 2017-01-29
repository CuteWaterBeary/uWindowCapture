﻿using UnityEngine;
using System.Collections.Generic;

namespace uWindowCapture
{

public class UwcWindowList : MonoBehaviour 
{
    [SerializeField] GameObject windowListItem;
    [SerializeField] Transform listRoot;
    Dictionary<int, UwcWindowListItem> items_ = new Dictionary<int, UwcWindowListItem>();

    void Start()
    {
        UwcManager.onWindowAdded.AddListener(OnWindowAdded);
        UwcManager.onWindowRemoved.AddListener(OnWindowRemoved);

        foreach (var pair in UwcManager.windows) {
            OnWindowAdded(pair.Value);
        }
    }

    void OnWindowAdded(UwcWindow window)
    {
        if (!window.isAltTabWindow || string.IsNullOrEmpty(window.title)) return;

        var gameObject = Instantiate(windowListItem, listRoot, false);
        var listItem = gameObject.GetComponent<UwcWindowListItem>();
        listItem.window = window;
        items_.Add(window.id, listItem);

        window.RequestCapture(CapturePriority.Low);
    }

    void OnWindowRemoved(UwcWindow window)
    {
        UwcWindowListItem listItem;
        items_.TryGetValue(window.id, out listItem);
        if (listItem) {
            Destroy(listItem.gameObject);
        }
    }
}

}