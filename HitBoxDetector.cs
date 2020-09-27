using UnityEngine;
using Databox;

public class HitBoxDetector : MonoBehaviour
{
    Transform triggerRoot;
    Collider trigger;
    Animator triggerRootAnimator;
    public string triggerName;
    public bool isHit;
    public string rootName;
    public string triggerRootName;

    private void Start()
    {
        rootName = this.transform.root.name;
    }

    private void Update()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name != rootName)
        {
            trigger = other;
            triggerName = trigger.gameObject.name;
            
            triggerRoot = trigger.transform.root;
            triggerRootName = triggerRoot.name;
            triggerRootAnimator = triggerRoot.gameObject.GetComponent<Animator>();

            if (other.gameObject.tag == "weapon" && triggerRootAnimator.GetCurrentAnimatorStateInfo(0).IsTag("attack"))
            {
                isHit = true;
            }
        }
    }
}
