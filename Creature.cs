using UnityEngine;
using Databox;

public class Creature : MonoBehaviour
{
    public DataboxObject database;
    public GameObject hitbox { get { return transform.Find("hitbox").gameObject; } }
    HitBoxDetector hitBoxDetector { get { return hitbox.GetComponent<HitBoxDetector>(); } }
    //public ParticleSystem creatureParticles { get { return GetComponent<ParticleSystem>(); } }


    public string objectName;
    public float hp;
    public float attackerDamage;
    public bool isDead;
    public bool isHit { get { return hitBoxDetector.isHit; } }
    public float deathTimer = 200.0f;

    public string triggerRootName { get { return hitBoxDetector.triggerRootName; } }

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
            startDeath();
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
        FloatType attackerDamageData = database.GetData<FloatType>(tableName, triggerRootName, "damage");
        attackerDamage = attackerDamageData.Value;
    }

    private void checkHitBox()
    {
        if (isHit)
        {
            collectAttackerData();
            hp = hp - attackerDamage;
            hitBoxDetector.isHit = false;
        }

        isDead = (hp == 0 || hp < 0) ? true : false;
    }

    private void startDeath()
    {
        //creatureParticles.gameObject.SetActive(true);
        deathTimer -= 1;

        if(deathTimer == 0.0f)
        {
            Destroy(this.gameObject);
        }
        
    }
}
