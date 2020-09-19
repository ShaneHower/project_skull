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
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == colliderTag)
        {
            isActive = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == colliderTag)
        {
            isActive = false;
        }
    }
}
