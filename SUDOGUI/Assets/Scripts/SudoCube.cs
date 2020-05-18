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
    Transform _camera;
    int _sudoValue;
    bool[] _buttonEnabled;
    bool _sudoHole = false;
    int _sudoSolution;
    SudoCube[] _cubes;
    void Start()
    {
        _camera = Camera.main.transform;
        _buttonEnabled = new bool[g.PUZZLESIZE + 1];
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

    public void ButtonClick(TMPro.TMP_Text text)
    {
        Vector3 location = transform.position;
        Quaternion rotation = transform.rotation;
        int buttonNo = int.Parse(text.text);
        Destroy(transform.gameObject);
        placeCube(buttonNo, location, rotation);
        //int buttonNo = int.Parse(text.text);
        //_buttonEnabled[buttonNo] = !_buttonEnabled[buttonNo];
        //if (_buttonEnabled[buttonNo])
        //{
        //    text.color = Color.black;
        //    text.fontStyle = FontStyles.Bold;
        //}
        //else
        //{
        //    text.color = Color.grey;
        //    text.fontStyle = FontStyles.Normal;
        //}
    }

    void placeCube(int cubeNo, Vector3 location, Quaternion rotation)
    {
        SudoCube nCube;
        nCube = Instantiate(_cubes[cubeNo]);
        nCube.transform.position = location;
        nCube.transform.rotation = rotation;
    }


}