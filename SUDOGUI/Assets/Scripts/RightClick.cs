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

        SudoPromoted nCube;
        nCube = Instantiate(AssetDatabase.LoadAssetAtPath<SudoPromoted>($"Assets/Prefabs/u{cubeNo}.prefab"));
        nCube.SudoValue = int.Parse(cubeNo);
        nCube.transform.position = location;
        nCube.transform.rotation = rotation;
    }

}

