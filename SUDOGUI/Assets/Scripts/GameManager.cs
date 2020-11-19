﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    #region PRIVATE DATA
    int[][][] _puzzleData;

    bool[] _layerActive;
    [SerializeField]
    bool _makeHoles = true;

    [SerializeField]
    double _percentHoles = .4f;
    #endregion                *
    #region PUBLIC DATA
    public SudoCube[] _cubes;
    #endregion

    #region CONSTRUCTION
    void Start()
    {
        _layerActive = new bool[g.PUZZLESIZE];
        for (int i = 0; i < g.PUZZLESIZE; i++)
        {
            _layerActive[i] = true;
        }
        readCSVData();
        if (_makeHoles)
            makeHoles();
        placeDataInCubes();
    }

    int[] pattern;
    private void makeHoles()
    {
        System.Random rand = new System.Random();

        for (int L = 0; L < g.PUZZLESIZE; L++)
        {
            for (int R = 0; R < g.PUZZLESIZE; R++)
            {
                for (int C = 0; C < g.PUZZLESIZE; C++)
                {
                    //if (true)//*/
                    if (g.RandomBool(_percentHoles))//*/
                        _puzzleData[L][R][C] *= -1;
                }
            }
        }
    }

    private void initializePuzzleArray()
    {
        _puzzleData = new int[g.PUZZLESIZE][][];
        for (int L = 0; L < g.PUZZLESIZE; L++)
        {
            _puzzleData[L] = new int[g.PUZZLESIZE][];
            for (int R = 0; R < g.PUZZLESIZE; R++)
            {
                for (int C = 0; C < g.PUZZLESIZE; C++)
                {
                    _puzzleData[L][R] = new int[g.PUZZLESIZE];
                }
            }
        }
    }
    #endregion


    void numberTheFaces()
    {
        initializePuzzleArray();

        for (int X = 0; X < g.PUZZLESIZE; X++)
        {
            for (int Y = 0; Y < g.PUZZLESIZE; Y++)
            {
                for (int Z = 0; Z < g.PUZZLESIZE; Z++)
                {
                    _puzzleData[X][Y][Z] = -1; // UNK
                    if (X == 0 && Y != 0 && Z != 0)
                        _puzzleData[X][Y][Z] = 2; // WEST SIDE
                    else if (X == g.PUZZLESIZE - 1)
                        _puzzleData[X][Y][Z] = 4; // EAST SIDE
                    if (Y == 0)
                        _puzzleData[X][Y][Z] = 5; // NORTH SIDE
                    else if (Y == g.PUZZLESIZE - 1)
                        _puzzleData[X][Y][Z] = 6; // SOUTH SIDE;
                    if (Z == 0)
                        _puzzleData[X][Y][Z] = 1; //FRONT SIDE
                    else if (Z == g.PUZZLESIZE - 1)
                        _puzzleData[X][Y][Z] = 3; // BACK SIDE

                }
            }
        }
    }

    /// <summary>
    /// placeDataInCubes
    /// build array of cubes.
    /// 
    /// (this code is only supposed to build one row of cubes--
    /// I'm checking if this will work.)
    /// </summary>
    void placeDataInCubes()
    {
        const float LEFT = -400f;
        const float TOP = 400F;
        const float FRONT = -400f;
        float curX;
        float curY;
        float curZ;
        float space = 125f;
        float dblSpace = 160f;


        curZ = FRONT;
        bool zFirst = true;
        GameObject _sudoKube = new GameObject("sudoKube");
        for (int z = 0; z < g.PUZZLESIZE; z++)
        {
            LinkedList<GameObject> curLayerList = new LinkedList<GameObject>();
            g.DLayers.Add(z, curLayerList);
            if (!zFirst)
            {
                if (z % 3 == 0)
                    curZ += dblSpace;  // move in positive direction, front - back
                else
                    curZ += space;
            }
            zFirst = false;

            curY = TOP; // building array top-bottom, left-right.

            bool yFirst = true;
            for (int y = 0; y < g.PUZZLESIZE; y++)
            {
                if (!yFirst)
                {
                    if (y % 3 == 0)
                        curY -= dblSpace; // move in negative direction, top to bottom
                    else
                        curY -= space;
                }
                yFirst = false;

                curX = LEFT;

                for (int x = 1; x < g.PUZZLESIZE + 1; x++)
                {
                    SudoCube nCube;
                    int v = _puzzleData[z][y][x - 1];
                    if (v > 0)
                        nCube = Instantiate(_cubes[v]);
                    else
                        nCube = Instantiate(_cubes[0]);

                    nCube.SudoValue = v; // (- solution when cube is a unsolved)

                    nCube.transform.position = new Vector3(curX, curY, curZ);
                    nCube.ID = z * 100 + y * 10 + x;  // zyx = layer-row-col
                    nCube.gameObject.transform.parent = _sudoKube.transform;
                    curLayerList.AddLast(nCube.gameObject);
                    if (x % 3 == 0)
                        curX += dblSpace; // move in positive direction, left to right
                    else
                        curX += space;
                } // x
            }// y
        } //z 
    }

    public void HideLayer(TMP_Text text)
    {
        if (text.text == "A")
        {
            for (int i = 0; i < g.PUZZLESIZE; i++)
            {
                int L = 0;
                foreach (GameObject go in g.DLayers[i])
                {
                    _layerActive[L] = true;
                    go.SetActive(true);
                }
            }
        }
        else
        {
            int layer = int.Parse(text.text) - 1;
            LinkedList<GameObject> _objs = g.DLayers[layer];
            bool active = !_layerActive[layer];
            _layerActive[layer] = active;
            foreach (GameObject go in _objs)
            {
                go.SetActive(active);
                text.faceColor = active ? Color.black : Color.red;
            }
        }
    }

    private void readCSVData()
    {
        initializePuzzleArray();

        try
        {
            string filename = @"D:\PROJECTS\SUDOKUBE\DATA\DATA.CSV";
            StreamReader rdr = new StreamReader(filename);
            string lineRead = rdr.ReadLine();
            if (lineRead.StartsWith("LID"))
                lineRead = rdr.ReadLine(); // read past header row
            do
            {
                parseReadLine(lineRead);
            }
            while ((lineRead = rdr.ReadLine()) != null);
        }
        catch (Exception x)
        {
            print($"Error in readCSVData(): {x.Message}");
        }
    }


    private void parseReadLine(string lineRead)
    {
        try
        {
            int commaPos = lineRead.IndexOf(',');
            int lid = int.Parse(lineRead.Substring(0, commaPos));
            lineRead = lineRead.Substring(commaPos + 1);
            commaPos = lineRead.IndexOf(',');
            int rid = int.Parse(lineRead.Substring(0, commaPos));
            lineRead = lineRead.Substring(commaPos + 1);
            for (int c = 0; c < g.PUZZLESIZE; c++)
            {
                int value;
                if (lineRead.Contains(","))
                {
                    commaPos = lineRead.IndexOf(',');
                    value = int.Parse(lineRead.Substring(0, commaPos));
                    //_puzzleData[lid][rid][c] = value;
                    lineRead = lineRead.Substring(commaPos + 1);
                }
                else
                    value = int.Parse(lineRead);

                _puzzleData[lid][rid][c] = value;
            }


        }
        catch (Exception x)
        {
            throw new Exception($"Error in GameManager.parseReadLine(): {x.Message}");
        }
    }
}