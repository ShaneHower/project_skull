using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;

    public float speed = 150.0f;
    public float dstFromTarget = 4.0f;
    public float pitch;
    public float yaw;
    public float smoothTimePos = 0.18f;

    Vector3 smoothVP;
    Vector3 targetRotation;

    // Update is called once per frame
    void LateUpdate()
    {
        moveCamera();
    }

    void moveCamera()
    {
        yaw += speed * Input.GetAxis("Mouse X") * Time.deltaTime; 
        pitch -= speed * Input.GetAxis("Mouse Y") * Time.deltaTime;
        pitch = Mathf.Clamp(pitch, -10, 60);

        targetRotation = new Vector3(pitch, yaw, 0.0f);
        transform.eulerAngles = targetRotation;

        Vector3 newPosition = target.position - (transform.forward * dstFromTarget);

        //SmoothDamp makes the camera feel like the player is pulling it along.  Think of a balloon on a string tied to the PC
        transform.position = Vector3.SmoothDamp(transform.position, newPosition, ref smoothVP, smoothTimePos);
    }
}
