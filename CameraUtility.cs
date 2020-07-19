using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraUtility
{
    public Camera camera;
    private Vector3 offset;
    private float yaw;

    public CameraUtility(Camera camera)
    {
        this.camera = camera;
        this.offset = new Vector3(10.0f, 4.0f, 3.0f);
        this.yaw = this.camera.transform.eulerAngles.y;
    }

    public void followObject(CharacterController target)
    {
        this.camera.transform.position = target.transform.position + this.offset;
        rotateCamera();
        Debug.Log(this.yaw);
    }

    public void rotateCamera()
    {
        if(Input.GetKey(KeyCode.Mouse2))
        {
            this.yaw += Input.GetAxis("Mouse X"); 
            this.camera.transform.eulerAngles = new Vector3(0.0f, this.yaw, 0.0f); 
        }
        
    }
}
