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
    //public bool[] SelectedValues { get; set; }
    Transform _camera;
    public int ID { get; set; }
    private int _sudoValue;
    private bool _sudoHole = false;
    private int _sudoSolution;


    void Start()
    {
        _camera = Camera.main.transform;
        Canvas canvas = GetComponentInChildren<Canvas>();
        if (canvas != null)
        {
            canvas.worldCamera = Camera.main;
        }
    }



    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            this.gameObject.SetActive(false);
        }
        this.transform.LookAt(_camera);
        transform.rotation = _camera.rotation;
    }

   



    public int SudoValue
    {
        set
        {
            _sudoValue = value;
            SudoHole = (value < 0);
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
        set
        {
            _sudoHole = value;
        }
        get
        {
            return _sudoHole;
        }
    }




}