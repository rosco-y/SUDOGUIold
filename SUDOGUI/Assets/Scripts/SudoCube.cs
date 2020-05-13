using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SudoCube : MonoBehaviour
{

    bool[] _buttonEnabled;

    private void Start()
    {
        _buttonEnabled = new bool[10];
        for (int i = 0; i < 10; i++)
        {
            _buttonEnabled[i] = false;
        }
    }
    public void ButtonClick(TMPro.TMP_Text text)
    {
        int buttonNo = int.Parse(text.text);
        _buttonEnabled[buttonNo] = !_buttonEnabled[buttonNo];
        if (_buttonEnabled[buttonNo])
            text.color = Color.black;
        else
            text.color = Color.grey;
    }
}
