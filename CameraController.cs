using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float speed;
    
    public Transform player;
    public Transform target;

    public float pitch;
    public float yaw;

    // Start is called before the first frame update
    void Start()
    {
        speed = 1.0f;
        //Cursor.visible = false;
        //Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        rotateCamera();
    }

    void rotateCamera()
    {
        yaw += speed * Input.GetAxis("Mouse X"); 
        pitch -= speed * Input.GetAxis("Mouse Y");
        pitch = Mathf.Clamp(pitch, -35, 60);

        transform.LookAt(target);
        target.rotation = Quaternion.Euler(pitch, yaw, 0.0f);
    }
}
