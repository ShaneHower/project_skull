using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundDetector : MonoBehaviour
{
    public bool isGrounded;

    // Update is called once per frame
    protected void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "terrain")
        {
            isGrounded = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "terrain")
        {
            isGrounded = false;
        }
    }
}
