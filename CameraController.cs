using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;

    public float speed = 1.0f;
    public float dstFromTarget = 6.0f;
    public float pitch;
    public float yaw;
    public float smoothTime = 0.12f;

    Vector3 smoothVelocity;
    Vector3 currentRotation;
    Vector3 targetRotation;

    // Update is called once per frame
    void LateUpdate()
    {
        rotateCamera();
    }

    void rotateCamera()
    {
        yaw += speed * Input.GetAxis("Mouse X"); 
        pitch -= speed * Input.GetAxis("Mouse Y");
        pitch = Mathf.Clamp(pitch, -10, 60);

        targetRotation = new Vector3(pitch, yaw, 0.0f);
        // what is this
        currentRotation = Vector3.SmoothDamp(currentRotation, targetRotation, ref smoothVelocity, smoothTime);

        transform.eulerAngles = currentRotation;
        transform.position = target.position - transform.forward * dstFromTarget;
    }
}
