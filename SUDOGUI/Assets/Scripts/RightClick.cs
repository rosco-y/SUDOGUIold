using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;
using UnityEditor;
using System;
using System.IO;

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
        Vector3 location = parent.gameObject.transform.position;
        Quaternion rotation = parent.gameObject.transform.rotation;
        Button rbutton = GetComponentInChildren<Button>(); // uniquely named in this block
        TMP_Text rtext = rbutton.GetComponentInChildren<TMP_Text>();
        _error = rtext.text != parent.SudoSolution.ToString();
        Destroy(parent.gameObject);
        placeCube(rtext.text, location, rotation);
    }

    void leftClick()
    {
        Button button = GetComponent<Button>();
        TMP_Text text = button.GetComponentInChildren<TMP_Text>();
        _selected = !_selected;
        if (_selected)
        {
            GetComponentInParent<SudoCube>().SelectedValues[int.Parse(text.text)] = true;
            text.color = Color.blue;
            text.fontStyle = FontStyles.Bold;
        }
        else
        {
            GetComponentInParent<SudoCube>().SelectedValues[int.Parse(text.text)] = false;
            text.color = Color.grey;
            text.fontStyle = FontStyles.Normal;
        }

    }
    void placeCube(string cubeNo, Vector3 location, Quaternion rotation)
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
            nCube.SudoValue = int.Parse(cubeNo);
            nCube.transform.position = location;
            nCube.transform.rotation = rotation;
        }
        catch (Exception x)
        {
            Debug.Log($"RightClick.placeCube(): {x.Message}");
        }

    }

}

