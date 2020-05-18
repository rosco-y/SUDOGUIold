using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;
using UnityEditor;

public class RightClick : MonoBehaviour, IPointerClickHandler
{
    bool _selected = false;
    public void OnPointerClick(PointerEventData eventData)
    {
        switch (eventData.button)
        {
            case PointerEventData.InputButton.Left:
                //print("LEFT");
                Button button = GetComponentInChildren<Button>();
                TMP_Text text = button.GetComponentInChildren<TMP_Text>();
                _selected = !_selected;
                if (_selected)
                    text.color = Color.black;
                else
                    text.color = Color.grey;
                break;
            case PointerEventData.InputButton.Right:
                GameObject parent = this.GetComponentInParent<SudoCube>().gameObject;
                Vector3 location = parent.transform.position;
                Quaternion rotation = parent.transform.rotation;
                Button rbutton = GetComponentInChildren<Button>(); // uniquely named in this block
                TMP_Text rtext = rbutton.GetComponentInChildren<TMP_Text>();
                Destroy(parent);
                placeCube(rtext.text, location, rotation);
                break;
        }
    }
    void placeCube(string cubeNo, Vector3 location, Quaternion rotation)
    {

        SudoCube nCube;
        nCube = Instantiate(AssetDatabase.LoadAssetAtPath<SudoCube>($"Assets/Prefabs/s{cubeNo}.prefab"));
        nCube.transform.position = location;
        nCube.transform.rotation = rotation;
    }

}

