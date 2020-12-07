using System;
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
        if (!_makeHoles)
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
        // zyx = layer-row-col
        float fCurCol;
        float fCurRow;
        float fCurLayer;
        float space = 125f;
        float dblSpace = 160f;
        GameObject[][][] cubes = new GameObject[g.PUZZLESIZE+1][][];
        for (int layer = 0; layer < g.PUZZLESIZE; layer++)
        {
            cubes[layer] = new GameObject[g.PUZZLESIZE+1][];
            for (int row = 0; row < g.PUZZLESIZE; row++)
            {
                cubes[layer][row] = new GameObject[g.PUZZLESIZE+1];
            }

        }

        fCurLayer = FRONT;
        bool zFirst = true;
        GameObject _sudoKube = new GameObject("sudoKube");
        for (int iLayer = 0; iLayer < g.PUZZLESIZE; iLayer++)
        {

            LinkedList<GameObject> curLayerList = new LinkedList<GameObject>();
            g.DLayers.Add(iLayer, curLayerList);
            if (!zFirst)
            {
                if (iLayer % 3 == 0)
                    fCurLayer += dblSpace;  // move in positive direction, front - back
                else
                    fCurLayer += space;
            }
            zFirst = false;

            fCurRow = TOP; // building array top-bottom, left-right.

            int _canvasID = 0;
            bool yFirst = true;
            for (int iRow = 0; iRow < g.PUZZLESIZE; iRow++)
            {
                if (!yFirst)
                {
                    if (iRow % 3 == 0)
                        fCurRow -= dblSpace; // move in negative direction, top to bottom
                    else
                        fCurRow -= space;
                }
                yFirst = false;

                fCurCol = LEFT;

                // indexing iCol from 1 to 9 (instead of 0 to 8)
                for (int iCol = 1; iCol < g.PUZZLESIZE + 1; iCol++)
                {
                    SudoCube nCube;
                    int iSudoValue = _puzzleData[iLayer][iRow][iCol - 1];
                    if (iSudoValue > 0)
                        nCube = Instantiate(_cubes[iSudoValue]);
                    else
                        nCube = Instantiate(_cubes[0]);

                    if (nCube.SudoHole)
                    {
                        unkCanvas can = nCube.GetComponent<unkCanvas>();
                        can.CanvasID = ++_canvasID;
                    }

                    cubes[iLayer][iRow][iCol] = nCube.gameObject;

                    nCube.SudoValue = iSudoValue; // (- solution when cube is a unsolved)

                    nCube.transform.position = new Vector3(fCurCol, fCurRow, fCurLayer);
                    nCube.ID = iLayer * 100 + iRow * 10 + iCol;  // zyx = layer-row-col
                    nCube.gameObject.transform.parent = _sudoKube.transform;
                    curLayerList.AddLast(nCube.gameObject);
                    if (iCol % 3 == 0)
                        fCurCol += dblSpace; // move in positive direction, left to right
                    else
                        fCurCol += space;
                } // iCol
            }// iRow
        } //iLayer 

        // add all ncube objects to the GameObject[][][] array, so they
        // can be selectively added to the g.CLayers Dictionary<int, List<GameObject>>
        // so I can try to hide row layers using the hideLayerButtons on the game Canva=s.

        for (int ilayer = 0; ilayer < g.PUZZLESIZE; ilayer++)
        {
            for (int irow = 0; irow < g.PUZZLESIZE; irow++)
            {
                for (int icol = 0; icol < g.PUZZLESIZE; icol++)
                {
                    GameObject check = cubes[ilayer][irow][icol];
                    int iUse = icol; // try the icol.
                    if (!g.CLayers.ContainsKey(icol))
                    {
                        g.CLayers.Add(iUse, new LinkedList<GameObject>());
                    }
                    if (iUse == icol)
                    {
                        if (!g.CLayers[iUse].Contains(check))
                            g.CLayers[iUse].AddLast(check);
                    }
                }
            }
        }
    }

    public void HideLayer(TMP_Text text)
    {
        if (text.text == "A")
        {
            // i = 0 to 9, check for boundry issues.
            for (int i = 0; i < g.PUZZLESIZE+1; i++)
            {
                int L = 0;
                if (g.CLayers.ContainsKey(i))
                {
                    foreach (GameObject go in g.CLayers[i])
                    {
                        _layerActive[L] = true;
                        go.SetActive(true);
                    }
                }
            }
        }
        else
        {
            int layer = int.Parse(text.text);
            LinkedList<GameObject> _objs = g.CLayers[layer];
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