﻿using UnityEngine;
using System.Collections.Generic;

namespace uWindowCapture
{

public class UwcHorizontalLayouter : UwcLayouter
{
    const int WINDOW_BASE_PIXEL = 1000;

    [SerializeField] 
    [Tooltip("meter / 1000 pixel")]
    float scale = 1f;

    public override void UpdateLayout(Dictionary<System.IntPtr, UwcWindowObject> windows)
    {
        var pos = Vector3.zero;
        var preWidth = 0f;

        var enumerator = windows.GetEnumerator();
        while (enumerator.MoveNext()) {
            var window = enumerator.Current.Value.window;
            var transform = enumerator.Current.Value.transform;
            var baseWidth = transform.GetComponent<MeshFilter>().sharedMesh.bounds.extents.x * WINDOW_BASE_PIXEL / scale;

            var title = window.title;
            if (!string.IsNullOrEmpty(title)) {
                transform.name = title;
            }

            var width = window.width / baseWidth;
            var height = window.height / baseWidth;
            var offset = new Vector3(10 * (preWidth + width) / 2, 0f, 0f);

            if (window.isChild) {
                transform.localScale = new Vector3(width, 1f, height);
                pos += offset;
                transform.position = pos;
            } else {
                if (windows.ContainsKey(window.owner)) {
                    var owner = windows[window.owner];
                    transform.localPosition = new Vector3(0f, 0.1f, 0f);
                    transform.localRotation = Quaternion.identity;
                    transform.localScale = (new Vector3(width / owner.transform.localScale.x, 1f, height / owner.transform.localScale.z));
                }
            }

            preWidth = width;
        }
    }
}

}