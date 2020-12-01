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
        // dieFaces.cCur.Move()
        switch (movement)
        {
            case Up:
                // make sure and do these assignments in order!
                // (to avoid reassigning freshly assigned variables: a = b; c = a;
                _top = _facing;
                _facing = Bottom;
                _bottom = 7 - _top;
                initializeSides();
                break;
            case Down:
                int ttop = _top;
                _top = 7 - _facing;
                _bottom = _facing;
                _facing = ttop;
                initializeSides();
                break;
            case Right:
                //int side = _sides[0];
                //int facingIndex = _sides.IndexOf(_facing);
                //if (facingIndex < (NUMSIDES))
                //{
                //    _facing = _sides[facingIndex];
                //}
                //else
                //    _facing = _sides[0];

                int facingIndex = _sides.IndexOf(_facing);
                rotate(_sides, eMovement.Right);
                // rotate
                _facing = _sides[facingIndex];
                break;
            case Left:
                //int last = _sides.Count - 1;
                //int side = _sides[last];
                facingIndex = _sides.IndexOf(_facing);
                //_sides.RemoveAt(last);
                //_sides.Insert(0, side);
                rotate(_sides, eMovement.Left);
                _facing = _sides[facingIndex];
                //facingIndex = _sides.IndexOf(_facing);
                //rotate(_sides, eMovement.Left);
                //// rotate
                //_facing = _sides[facingIndex];
                break;
            default:
                Console.Write($"Unhandled case: {movement.ToString()}");
                break;
        }

        return _facing;
    }

    void rotate(List<int> ls, eMovement movement)
    {
        
        if (movement == eMovement.Right)
        {
            int len = ls.Count - 1;
            int lastValue = ls[len];
            ls.RemoveAt(len);
            ls.Insert(0, lastValue);
        }
        else if (movement == eMovement.Left)
        {
            int firstValue = ls[0];
            ls.RemoveAt(0);
            ls.Add(firstValue);
        }
        
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
                if (Top == 6)
                    _sides.Reverse();
                break;
            case 3:
            case 4:
                _sides = new List<int> { 1, 2, 6, 5 };
                if (Top == 4)
                    _sides.Reverse();
                break;
            case 2:
            case 5:
                _sides = new List<int> { 1, 3, 6, 4 };
                if (Top == 2)
                    _sides.Reverse();
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
        int printLen = _sides.Count * 4;
        string die = "----------------\n";
        die += $"{_top}/{_bottom} \n";
        die += new string('-', printLen);
        die += "\n";
        foreach (int face in _sides)
        {
            if (face == Face)
                die += $"<*{face}*>";
            else
                die += $" {face}";
            if (face != _sides[_sides.Count-1])
                die += ",";
        }
        die += "\n";
        die += new string('-', printLen);
        //die += new string('\n', 2);
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