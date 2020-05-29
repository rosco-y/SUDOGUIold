using System;
using System.Collections;
using System.Collections.Generic;


public static class g
{
    public const int PUZZLESIZE = 9;
    public const int UNKNOWN = 0;
    static TimeSpan _doubleClickThreshold = new TimeSpan(0, 0, 0, 0, 350); // 3.5/10 of a second
    static DateTime _lastClickTime = DateTime.MinValue;
    public static bool DoubleClick = false;
    static Random _rand = new Random();
    public static bool RandomBool()
    {
        return _rand.NextDouble() > .8;
    }

    public static void Click()
    {
        DoubleClick = (DateTime.Now - _lastClickTime < _doubleClickThreshold);
        _lastClickTime = DateTime.Now;
    }

}
