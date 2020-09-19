using UnityEngine;

public class TriggerDetector : MonoBehaviour
{
    public bool isActive;
    public string objectName;
    public string colliderType;
    public bool hasTriggered;
    public Animator animator;

    private void Start()
    {
        objectName = this.gameObject.name;
        animator = GetComponentInParent<Animator>();

        // This will eventually be done with some kind of database, this is just proof of concept.
        if (objectName == "weapon")
        {
            colliderType = "enemy";
            hasTriggered = animator.GetCurrentAnimatorStateInfo(0).IsTag("attack");
        }
        else if (objectName == "Ground Detector")
        {
            colliderType = "terrain";
        }
    }

    private void Update()
    {
        if(objectName == "weapon")
        {
            hasTriggered = animator.GetCurrentAnimatorStateInfo(0).IsTag("attack");
        }
        else
        {
            hasTriggered = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == colliderType && !hasTriggered)
        {
            isActive = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == colliderType)
        {
            isActive = false;
        }
    }
}
