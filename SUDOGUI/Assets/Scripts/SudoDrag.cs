using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static eTurnDirection;
//using System;
using System.Collections.Specialized;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Security.Cryptography;

public enum eTurnDirection
{
    LEFT,
    RIGHT,
    UP,
    DOWN
}



public class SudoDrag : MonoBehaviour
{

    Quaternion _originalRotation;
    public float _rotateSpeed = 1f;
    Quaternion _toRotation = Quaternion.identity;

    private void Awake()
    {
    }

    private void FixedUpdate()
    {

        transform.rotation = Quaternion.Slerp(transform.rotation, _toRotation, _rotateSpeed * Time.deltaTime);
    }




    private void Update()
    {


        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            turn(LEFT);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            turn(RIGHT);
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            turn(DOWN); // down goes to the "up" side.
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            turn(UP);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.right * 15);
    }

    //bool _onSide = false;
    void turn(eTurnDirection direction)
    {
        _originalRotation = transform.rotation;
        float curX = _originalRotation.eulerAngles.x;
        float curY = _originalRotation.eulerAngles.y;
        float curZ = _originalRotation.eulerAngles.z;

        switch (direction)
        {
            case LEFT:
                _toRotation = Quaternion.FromToRotation(Vector3.left, transform.forward);
                //_toRotation = Quaternion.Euler(0f, curY + 90f, 0f); // Y
                break;
            case RIGHT:
                _toRotation = Quaternion.FromToRotation(Vector3.right, transform.forward);
                //_toRotation = Quaternion.Euler(0f, curY - 90f, 0f); // -Y
                break;
            case UP:
                _toRotation = Quaternion.FromToRotation(Vector3.down, transform.forward);
                break;
            case DOWN:
                _toRotation = Quaternion.FromToRotation(Vector3.up, transform.forward);
                break;
        }
        float x = correctError(_toRotation.eulerAngles.x);
        float y = correctError(_toRotation.eulerAngles.y);
        float z = correctError(_toRotation.eulerAngles.z);
        _toRotation.eulerAngles = new Vector3(x, y, z);
        //transform.Rotate(_toRotation.eulerAngles, Space.World);

    }

    float correctError(float x)
    {

        bool negative = x < 0;
        float minDiff;
        float angle = 0f;
        minDiff = Mathf.Abs(x); // x can be < 0;

        if (Mathf.Abs(x - 90) < minDiff)
        {
            minDiff = Mathf.Abs(x - 90);
            angle = 90f;
        }
        if (Mathf.Abs(x - 180) < minDiff)
        {
            angle = 180f;
            minDiff = Mathf.Abs(x - 180);
        }
        if (Mathf.Abs(x - 270) < minDiff)
        {
            minDiff = Mathf.Abs(x - 270);
            angle = 270f;
        }
        if (Mathf.Abs(x - 360) < minDiff)
        {
            angle = 360f;  // reset angle to 0?
        }

        if (negative)
            angle *= -1;

        return angle;
    }



}


