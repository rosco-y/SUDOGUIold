using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;
using UnityEditor;
using System;
using System.IO;
using System.Collections.Generic;

public class RightClick : MonoBehaviour, IPointerClickHandler
{
    bool _selected = false;
    DateTime lastLeftClickTime = DateTime.MinValue;
    bool _error;

    public void OnPointerClick(PointerEventData eventData)
    {
        g.Click();
        switch (eventData.button)
        {
            case PointerEventData.InputButton.Left:
                if (g.DoubleClick)
                {
                    rightClick(); // double click is handled the same as left rightclick
                }
                else
                {
                    // record time so double-click can be "sensed"
                    lastLeftClickTime = DateTime.Now;
                    leftClick();
                }
                break;

            case PointerEventData.InputButton.Right:
                rightClick();
                break;
        }
    }


    void rightClick()
    {
        SudoCube parent = this.GetComponentInParent<SudoCube>();
        int ID = parent.ID;
        int rowCol = ID % 100;
        int layer = ID - rowCol;
        LinkedList<GameObject> curLayer;
        Vector3 location = parent.gameObject.transform.position;
        Quaternion rotation = parent.gameObject.transform.rotation;
        Button rbutton = GetComponentInChildren<Button>(); // uniquely named in this block
        TMP_Text rtext = rbutton.GetComponentInChildren<TMP_Text>();
        int solution = parent.SudoSolution;
        _error = rtext.text != solution.ToString();
        curLayer = g.DLayers[layer];
        foreach (GameObject obj in curLayer)
        {
            if (obj.GetComponent<SudoCube>()?.ID == ID)
            {
                curLayer.Remove(obj);
                break;
            }
       }
        Destroy(parent.gameObject);
        placeCube(rtext.text, location, rotation, solution, curLayer);
    }

    void leftClick()
    {
        Button button = GetComponent<Button>();
        TMP_Text text = button.GetComponentInChildren<TMP_Text>();
        _selected = !_selected;
        if (_selected)
        {
            text.faceColor = Color.red;
            text.fontStyle = FontStyles.Italic & FontStyles.Bold;
        }
        else
        {
            text.faceColor = Color.black;
            text.fontStyle = FontStyles.Normal;
        }

    }
    void placeCube(string cubeNo, Vector3 location, Quaternion rotation, int solution, LinkedList<GameObject> curLayer)
    {

        SudoPromoted nCube;
        string sCubeDir;
        if (!_error)
            sCubeDir = $"Assets/Prefabs/u{cubeNo}.prefab";
        else
            sCubeDir = $"Assets/Prefabs/Error/e{cubeNo}.prefab";
        try
        {
            nCube = Instantiate(AssetDatabase.LoadAssetAtPath<SudoPromoted>(sCubeDir));
            nCube.SudoSolution = solution;

            curLayer.AddLast(nCube.gameObject);
            nCube.transform.position = location;
            nCube.transform.rotation = rotation;
        }
        catch (Exception x)
        {
            Debug.Log($"RightClick.placeCube(): {x.Message}");
        }

    }

}

