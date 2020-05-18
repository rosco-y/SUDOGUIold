using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEditor;

public class SudoPromoted : MonoBehaviour, IPointerClickHandler
{
    Transform _camera;
    public int SudoValue { set; get; }

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
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            Vector3 location = transform.position;
            Quaternion rotation = transform.rotation;
            Destroy(transform.gameObject);
            SudoCube nCube = Instantiate(AssetDatabase.LoadAssetAtPath<SudoCube>($"Assets/Prefabs/UNK.prefab"));
            nCube.transform.position = location;
            nCube.transform.rotation = rotation;
        }
    }

}
