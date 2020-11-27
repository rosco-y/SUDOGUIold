using System;
using System.Collections.Generic;

using static eMovement;
public enum eMovement
{
    Up,
    Down,
    Right,
    Left,
    Quit
}
public class cCur
{
    int _facing; //facing
    int _top; // dictages what the _sides are.
    int _bottom;
    const int NUMSIDES = 4;
    List<int> _sides;
    public cCur(int facing, int top)
    {
        // WILL %modulo to 0, 1, 2, but want to start with a large
        // number so negative direction rotations won't lead to negative
        // indices.
        //_facing = (NUMSIDES * 100000 + 6) % NUMSIDES;
        _facing = facing;
        _top = top;
        _bottom = 7 - top;
        initializeSides();
    }

    public int Move(eMovement movement)
    {

        switch (movement)
        {
            case Down:
                // make sure and do these assignments in order!
                // ROLLING THE TOP AWAY 90 DEGREES.
                _top = _facing;
                _facing = Bottom;
                _bottom = 7 - _top;
                initializeSides();
                break;
            case Up:
                int ttop = _top;
                _top = 7 - _facing;
                _bottom = _facing;
                _facing = ttop;
                initializeSides();
                break;
            case Right:
                int side = _sides[0];
                int facingIndex = _sides.IndexOf(_facing);
                if (facingIndex < (NUMSIDES + 2))
                {
                    _facing = _sides[facingIndex + 1];
                }
                else
                    _facing = _sides[0];

                _sides.RemoveAt(0);
                _sides.Add(side);
                break;
            case Left:
                int last = _sides.Count - 1;
                side = _sides[last];
                facingIndex = _sides.IndexOf(_facing);
                _sides.RemoveAt(last);
                _sides.Insert(0, side);
                _facing = _sides[facingIndex];
                break;
            default:
                Console.Write($"Unhandled case: {movement.ToString()}");
                break;
        }

        return _facing;
    }

    ~cCur()
    {
        _sides?.Clear();
        _sides = null;
    }

    private void initializeSides()
    {
        switch (Top)
        {
            case 1:
            case 6:
                _sides = new List<int> { 2, 3, 5, 4 };
                break;
            case 3:
            case 4:
                _sides = new List<int> { 1, 2, 6, 5 };
                break;
            case 2:
            case 5:
                _sides = new List<int> { 1, 3, 6, 4 };
                break;
        }
    }

    public int Face
    {
        set
        {
            _facing = value;
            _bottom = _facing;
        }
        get
        {
            return _facing;
        }
    }



    public int Top
    {
        get { return _top; }
        private set
        {
            _top = value;
            Bottom = _top;
        }
    }

    public int Bottom
    {
        get { return _bottom; }
        private set
        {
            _bottom = value;
            _top = _bottom;
        }
    }


    public List<int> Sides
    {
        get
        {
            return _sides;
        }
    }

    //*
    public override string ToString()
    {
        int printLen = _sides.Count * 2;
        string die = "----------------\n";
        die += $"| {_top} | {_bottom} |\n";
        die += new string('-', printLen);
        die += "\n";
        foreach (int face in _sides)
        {
            if (face == Face)
                die += $"[{face}]";
            else
                die += $" {face},";
        }
        die += "|\n";
        die += new string('-', printLen);
        die += new string('\n', 2);
        return die;
    }
    //*/

    /*
    public eMovement GetKey()
    {
        eMovement ret = eMovement.Quit;
        Console.Write("enter UP|DOWN|LEFT|RIGHT|Q :");
        var key = Console.ReadKey();
        bool done = false;
        while (!done)
        {

            done = true;
            switch (key.Key)
            {
                case ConsoleKey.UpArrow:
                    ret = Up;
                    break;
                case ConsoleKey.DownArrow:
                    ret = Down;
                    break;
                case ConsoleKey.LeftArrow:
                    ret = Left;
                    break;
                case ConsoleKey.RightArrow:
                    ret = Right;
                    break;
                case ConsoleKey.Q:
                    ret = Quit;
                    break;
                default:
                    Console.Write("\nenter UP|DOWN|LEFT|RIGHT|Q :");
                    key = Console.ReadKey();
                    done = false;
                    break;
            }
        }
        Console.WriteLine(ret.ToString());
        return ret;
    }
    //*/
}