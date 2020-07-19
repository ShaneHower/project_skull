using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraUtility
{
    private Camera cameraIn;
    private CharacterController targetIn;

    private Transform camera;
    private Transform target;

    private Vector3 offset;

    private float yaw;
    private float pitch;

    public CameraUtility(Camera cameraIn, CharacterController targetIn)
    {
        this.camera = cameraIn.transform;
        this.target = targetIn.transform;

        this.offset = new Vector3(-5.0f, 4.0f, -2.0f) + target.transform.position;

        this.pitch = this.camera.transform.eulerAngles.x;
        this.yaw = this.camera.transform.eulerAngles.y;
    }

    public void followObject()
    {
        // rotate the camera around the player if the middle mouse button is down;
        if(Input.GetKey(KeyCode.Mouse2))
        {
            yaw = Input.GetAxisRaw("Mouse X");
            pitch = Input.GetAxisRaw("Mouse Y");

            // what is quaternion.angleaxis?
            if (yaw != 0.0f)
            {
                this.offset = Quaternion.AngleAxis(yaw, Vector3.up) * this.offset;
            }

            if (pitch != 0.0f)
            {
                this.offset = Quaternion.AngleAxis(pitch, Vector3.right) * this.offset;
            }
        }

        this.camera.position = this.target.position + offset;
        this.camera.LookAt(this.target.position);
    }
}
