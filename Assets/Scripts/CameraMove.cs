using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UIElements;

public class CameraMove : MonoBehaviour
{
    public GameObject followedObject;
    public Vector3 offset = new Vector3(0f, 0f, -10f);

    private void LateUpdate()
    {
        if (followedObject) transform.position = followedObject.transform.position - offset;
    }
}
