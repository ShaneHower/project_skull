using UnityEngine;
using Databox;

public class Creature : MonoBehaviour
{
    public DataboxObject database;
    GameObject hitbox { get { return transform.Find("hitbox").gameObject; } }
    TriggerDetector hitboxDetector { get { return hitbox.GetComponent<TriggerDetector>(); } }

    public string objectName;
    public float hp;
    public float attackerDamage;
    public bool isDead;
    public bool isHit;

    public bool hitboxTrigger { get { return hitboxDetector.isActive; } }
    public string triggerName { get { return hitboxDetector.triggerName; } }

    void Start()
    {
        collectCreatureData();
    }

    // Update is called once per frame
    void Update()
    {
        checkHitBox();
        
        if (isDead)
        {
            checkDeath();
        }
    }

    private void collectCreatureData()
    {
        objectName = this.gameObject.name;
        string tableName = "stats";

        FloatType hpData = database.GetData<FloatType>(tableName, objectName, "hp");
        hp = hpData.Value;
    }

    private void collectAttackerData()
    {
        string tableName = "stats";
        FloatType attackerDamageData = database.GetData<FloatType>(tableName, triggerName, "damage");
        attackerDamage = attackerDamageData.Value;
    }

    private void checkHitBox()
    {
        if (hitboxTrigger)
        {
            collectAttackerData();
            hp = hp - attackerDamage;
            hitboxDetector.isActive = false;
            Debug.Log(hp);
        }

        isDead = (hp == 0 || hp < 0) ? true : false;
    }

    private void checkDeath()
    {
        Destroy(this.gameObject);
    }
}
