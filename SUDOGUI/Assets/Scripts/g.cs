using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;


public static class g
{
    public const int PUZZLESIZE = 9;
    public const int UNKNOWN = 0;
    static TimeSpan _doubleClickThreshold = new TimeSpan(0, 0, 0, 0, 500); // 1/2  of a second
    static DateTime _lastClickTime = DateTime.MinValue;
    public static bool DoubleClick = false;
    static System.Random _rand = new System.Random();
    public static Dictionary<int, LinkedList<GameObject>> DLayers = new Dictionary<int, LinkedList<GameObject>>();
    public static Dictionary<int, LinkedList<GameObject>> RLayers = new Dictionary<int, LinkedList<GameObject>>();
    public static bool RandomBool(double percentTrue)
    {
        return _rand.NextDouble() > 1f - percentTrue;
    }

    public static void Click()
    {
        DoubleClick = (DateTime.Now - _lastClickTime < _doubleClickThreshold);
        _lastClickTime = DateTime.Now;
    }

}
