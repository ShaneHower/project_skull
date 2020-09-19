using UnityEngine;

public class GroundDetector : MonoBehaviour
{
    public bool isGrounded;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "terrain")
        {
            isGrounded = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "terrain")
        {
            isGrounded = false;
        }
    }
}
