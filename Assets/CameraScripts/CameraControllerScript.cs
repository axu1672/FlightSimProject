using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class CameraControllerScript : MonoBehaviour
{
    public GameObject player;

    private bool initialized = false;

    private Transform target;

    private Vector3 offset;
    // Start is called before the first frame update
    void Start()
    {
        if (player == null)
        {
            return;
        }
        try
        {
            target = player.transform;
            offset = new Vector3(-25, 0, 7);
            initialized = true;
        }
        catch (Exception ex)
        {
            initialized = false;
        }      
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (initialized)
        {
            target = player.transform;
            float desiredAngle = target.eulerAngles.y;
            Quaternion rotation = Quaternion.Euler(90, desiredAngle, -90);

            transform.position = target.position - (rotation * offset);
            transform.LookAt(target);
        }
    }
}
