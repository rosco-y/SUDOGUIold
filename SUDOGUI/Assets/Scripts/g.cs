using System;
using System.Collections;
using System.Collections.Generic;


public static class g
{
    public const int PUZZLESIZE = 9;
    public const int UNKNOWN = 0;
    static Random _rand = new Random();
    public static bool RandomBool()
    {
        return _rand.NextDouble() > 0.8;
    }
}
