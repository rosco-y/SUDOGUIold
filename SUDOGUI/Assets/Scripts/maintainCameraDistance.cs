using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class maintainCameraDistance : MonoBehaviour
{
    // Start is called before the first frame update
    PhysicsRaycaster _raycast;
    void Start()
    {
        _raycast = GetComponent<PhysicsRaycaster>();
    }

    // Update is called once per frame
    void Update()
    {
    }
}
