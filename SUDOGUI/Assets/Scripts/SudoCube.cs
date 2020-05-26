using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SudoCube : MonoBehaviour
{
    // Start is called before the first frame update
    public bool[] SelectedValues { get; set; }
    Transform _camera;
    private int _sudoValue;
    private bool _sudoHole = false;
    private int _sudoSolution;


    SudoCube[] _cubes;
    void Start()
    {
        SelectedValues = new bool[g.PUZZLESIZE + 1]; // so offsets are not needed.
        _camera = Camera.main.transform;
        Canvas canvas = GetComponentInChildren<Canvas>();
        if (canvas != null)
        {
            canvas.worldCamera = Camera.main;
        }
        loadCubePrefabArray();
    }

    void loadCubePrefabArray()
    {
        _cubes = new SudoCube[10];

        _cubes[0] = AssetDatabase.LoadAssetAtPath<SudoCube>("Assets/Prefabs/UNK.prefab"); 

        for (int i = 1; i < g.PUZZLESIZE+1; i++)
        {
            _cubes[i] = AssetDatabase.LoadAssetAtPath<SudoCube>($"Assets/Prefabs/s{i}.prefab"); 
        }
    }


    // Update is called once per frame
    void Update()
    {
        this.transform.LookAt(_camera);
        transform.rotation = _camera.rotation;
    }



    public int SudoValue
    {
        set
        {
            _sudoValue = value;
            _sudoHole = (value < 0);
            _sudoSolution = Mathf.Abs(value);
        }
        get
        {
            return _sudoValue;
        }
    }

    public int SudoSolution
    {
        get
        {
            return _sudoSolution;
        }
    }

    /// <summary>
    /// true if this was a cell that the user needed to solve.
    /// </summary>
    public bool SudoHole
    {
        get
        {
            return _sudoHole;
        }
    }




}