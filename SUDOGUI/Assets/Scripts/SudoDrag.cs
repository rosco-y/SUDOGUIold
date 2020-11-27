using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static eTurnDirection;
//using System;
using System.Collections.Specialized;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Security.Cryptography;
using System;

public enum eTurnDirection
{
    LEFT,
    RIGHT,
    UP,
    DOWN
}



public class SudoDrag : MonoBehaviour
{

    cCur _currentSide;
    Quaternion _newRoation;
    Quaternion _curRotation;
    const int SPHERESIDES = 6;
    Quaternion _originalRotation;
    public float _rotateSpeed = 1f;
    Quaternion _toRotation = Quaternion.identity;
    bool _rotationChanged = false;
    GameObject[] _SphereRotations;

    private void Start()
    {

        initializeRotations();
        _currentSide = new cCur(5, 6);
        _curRotation = _newRoation = this.transform.rotation;
    }

    private void initializeRotations()
    {
        int side = 5;
        _SphereRotations = new GameObject[SPHERESIDES + 1];
        for (int i = 0; i < SPHERESIDES + 1; i++)
        {
            _SphereRotations[i] = new GameObject();
        }
        _SphereRotations[side].transform.rotation = Quaternion.Euler(0, 0, 0);

        side = 4;
        _SphereRotations[side].transform.rotation = Quaternion.Euler(0, 90, 0);

        side = 2;
        _SphereRotations[side].transform.rotation = Quaternion.Euler(0, 180, 0);

        side = 3;
        _SphereRotations[side].transform.rotation = Quaternion.Euler(0, 270, 0);

        side = 6;
        _SphereRotations[side].transform.rotation = Quaternion.Euler(90, 0, 0);

        side = 1;
        _SphereRotations[side].transform.rotation = Quaternion.Euler(270, 0, 0);
    }

    private void Awake()
    {
    }

    private void FixedUpdate()
    {
        if (_rotationChanged)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, _newRoation, _rotateSpeed * Time.deltaTime);
            _rotationChanged = !(transform.rotation == _toRotation);
        }
    }




    private void Update()
    {


        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            // KeyCode.RightArraw => eMovement.Left,
            // opposite of the key pressed--so cube rotates toward the face pointed to by the keypress.
            _currentSide.Move(eMovement.Left); 
            _newRoation = _SphereRotations[_currentSide.Face].transform.rotation;
            _rotationChanged = true;
            /////////////////////////////////////////////////////////////
            /// TODO: VERIFY THAT _roatationChanged is needed anymore
            /////////////////////////////////////////////////////////////
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            _currentSide.Move(eMovement.Right);
            _newRoation = _SphereRotations[_currentSide.Face].transform.rotation;
            _rotationChanged = true;
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            _currentSide.Move(eMovement.Up);
            _newRoation = _SphereRotations[_currentSide.Face].transform.rotation;
            _rotationChanged = true;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            _currentSide.Move(eMovement.Down);
            _newRoation = _SphereRotations[_currentSide.Face].transform.rotation;
            _rotationChanged = true;
        }
    }
}

///*private void OnDrawGizmos()
////{
////    Gizmos.color = Color.red;
////    Gizmos.DrawRay(transform.position, transform.right * 15);
////}*/

///* bool _onSide = false;
//void turn(eTurnDirection direction)
//{
//    _originalRotation = transform.rotation;
//    _rotationChanged = true;
//    switch (direction)
//    {
//        case LEFT:
//            _currentSide.Move(eMovement.Left);

//            _toRotation = Quaternion.FromToRotation(Vector3.left, transform.forward);
//            //_toRotation = Quaternion.LookRotation(Vector3.left,  transform.forward);
//            break;
//        case RIGHT:
//            _toRotation = Quaternion.FromToRotation(Vector3.right, transform.forward);
//            // _toRotation = Quaternion.LookRotation(Vector3.right, transform.forward);
//            break;
//        case UP:
//            //*********************************************************/
//            // currently testing
//            //*********************************************************/
//            //_toRotation = Quaternion.FromToRotation(Vector3.forward, transform.up);
//            //_toRotation = Quaternion.LookRotation(Vector3.left, transform.up);

//            //_toRotation = Quaternion.LookRotation(Vector3.left, transform.right);
//            //_toRotation = Quaternion.LookRotation(Vector3.right, transform.right);

//            //_toRotation = Quaternion.LookRotation(Vector3.left, transform.forward);
//            //*********************************************************/
//            //*********************************************************/
//            break;
//        case DOWN:
//            //_toRotation = Quaternion.LookRotation(Vector3.right, transform.forward);
//            //_toRotation = Quaternion.LookRotation(Vector3.right, transform.forward);
//            _toRotation = Quaternion.FromToRotation(Vector3.forward, -transform.up);
//            break;
//        default:
//            //we shouldn't got here, but if we did, then we aren't rotating.
//            _rotationChanged = false;
//            break;
//    }
//    if (_rotationChanged)
//    {
//        float x = correctError(_toRotation.eulerAngles.x);
//        float y = correctError(_toRotation.eulerAngles.y);
//        float z = correctError(_toRotation.eulerAngles.z);
//        _toRotation.eulerAngles = new Vector3(x, y, z);
//    }


//} */

//float correctError(float x)
//{

//    bool negative = x < 0;
//    float minDiff;
//    float angle = 0f;
//    minDiff = Mathf.Abs(x); // x can be < 0;

//    if (Mathf.Abs(x - 90) < minDiff)
//    {
//        minDiff = Mathf.Abs(x - 90);
//        angle = 90f;
//    }
//    if (Mathf.Abs(x - 180) < minDiff)
//    {
//        angle = 180f;
//        minDiff = Mathf.Abs(x - 180);
//    }
//    if (Mathf.Abs(x - 270) < minDiff)
//    {
//        minDiff = Mathf.Abs(x - 270);
//        angle = 270f;
//    }
//    if (Mathf.Abs(x - 360) < minDiff)
//    {
//        angle = 360f;  // reset angle to 0?
//    }

//    if (negative)
//        angle *= -1;

//    return angle;
//}



//}


