using UnityEngine;
using Databox;

public class TriggerDetector : MonoBehaviour
{
    //game objects
    Animator animator;
    public DataboxObject database;
    
    // public vars 
    public bool isActive;
    public string objectName;
    public string colliderTag;
    public string triggerName;

    // private vars
    private string useAs;

    private void Start()
    {
        collectData();
    }

    private void collectData()
    {
        objectName = this.gameObject.name;
        string tableName = "trigger_detector_inst_vars";

        StringType colliderTagType = database.GetData<StringType>(tableName, objectName, "type");
        colliderTag = colliderTagType.Value.ToString();

        StringType useAsType = database.GetData<StringType>(tableName, objectName, "use_as");
        useAs = useAsType.Value.ToString().ToLower();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == colliderTag)
        {
            isActive = true;
            triggerName = other.transform.root.name;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // when this class is used as hit detection, isActive has to be changed outside of this script
        // otherwise hp will be continually reduced as isActive doesn't change until it exits the collider
        // I should consider splitting this class into two, hitDetector groundDetector.
        if (other.gameObject.tag == colliderTag && useAs != "hitbox detector")
        {
            isActive = false;
        }
    }
}
