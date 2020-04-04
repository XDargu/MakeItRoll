using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Utils
{
    private static Vector3[] WorldCorners = new Vector3[4];
    public static Bounds GetRectTransformBounds(RectTransform transform)
    {
        transform.GetWorldCorners(WorldCorners);
        Bounds bounds = new Bounds(WorldCorners[0], Vector3.zero);
        for (int i = 1; i < 4; ++i)
        {
            bounds.Encapsulate(WorldCorners[i]);
        }
        return bounds;
    }

    public static string ToOneDecimal(float value)
    {
        return (Mathf.Floor(value * 10) / 10).ToString("0.#");
    }

    public static string FloatToString(float value)
    {
        if (float.IsNaN(value))
            return "";
        else if (Mathf.Floor(value) != value)
            return value.ToString("#,0.0");
        else
            return value.ToString("#,0");
    }

    public static string GetTimeDifference(DateTime start, DateTime end)
    {
        TimeSpan span = end.Subtract(start);
        return ToReadableString(span);
    }

    public static string ToReadableString(TimeSpan span)
    {
        String readable = "";
        if (span.Hours > 0) { readable += span.Hours + " hours, "; }

        if ((span.Minutes > 0) && (span.Days == 0)) { readable += span.Minutes + " minutes, "; }

        if ((span.Seconds > 0) && (span.Hours == 0)) { readable += span.Seconds + " seconds, "; }

        if (readable.EndsWith(", ")) readable = readable.Substring(0, readable.Length - 2);


        return readable;
    }
}