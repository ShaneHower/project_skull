using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;

    public float speed = 150.0f;
    public float dstFromTarget = 4.0f;
    public float pitch;
    public float yaw;
    public float smoothTimePos = 0.18f;

    float xRotate;
    float yRotate;

    Vector3 smoothVP;
    Vector3 targetRotation;

    // Update is called once per frame
    private void Update()
    {
        
    }

    void LateUpdate()
    {
        xRotate = Input.GetAxis("Mouse X");
        yRotate = Input.GetAxis("Mouse Y");
        moveCamera();
    }

    void moveCamera()
    {

        yaw += speed * xRotate * Time.deltaTime; 
        pitch -= speed * yRotate * Time.deltaTime;
        pitch = Mathf.Clamp(pitch, -10, 60);

        targetRotation = new Vector3(pitch, yaw, 0.0f);
        transform.eulerAngles = targetRotation;

        Vector3 newPosition = target.position - (transform.forward * dstFromTarget);

        //SmoothDamp makes the camera feel like the player is pulling it along.  Think of a balloon on a string tied to the PC
        transform.position = Vector3.SmoothDamp(transform.position, newPosition, ref smoothVP, smoothTimePos);
    }
}
