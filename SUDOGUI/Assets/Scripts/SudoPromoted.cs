using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEditor;
using System;

public class SudoPromoted : MonoBehaviour, IPointerClickHandler
{
    Transform _camera;
    public int ID { get; set; }
    public int SudoSolution { set; get; }
    bool _selectedValues;

    private void Start()
    {
        _camera = Camera.main.transform;
    }

    private void Update()
    {
        transform.LookAt(_camera);
        transform.rotation = _camera.rotation;
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        g.Click();
        if (g.DoubleClick)
        {
            rightClick();
        }
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            rightClick(); 
        }
    }

    void rightClick()
    {
        
        int rowCol = ID % 100;
        int layer = ID - rowCol;
        LinkedList<GameObject> curLayer = g.DLayers[layer];
        foreach (GameObject obj in curLayer)
        {
            if (obj.GetComponent<SudoPromoted>()?.ID == ID)
            {
                curLayer.Remove(obj);
                break;
            }
        }
        Vector3 location = transform.position;
        Quaternion rotation = transform.rotation;
        SudoCube nCube = Instantiate(AssetDatabase.LoadAssetAtPath<SudoCube>($"Assets/Prefabs/UNK.prefab"));
        curLayer.AddLast(nCube.gameObject);
        Destroy(transform.gameObject);
        nCube.SudoValue = SudoSolution; // set when UNK was promoted.
        nCube.transform.position = location;
        nCube.transform.rotation = rotation;
    }

}
