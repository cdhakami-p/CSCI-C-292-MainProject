using UnityEngine;

public class WhaleAbility : AbilityAC
{

    [SerializeField] float sizeMultiplier = 2f;
    [SerializeField] float massMultiplier = 2f;

    Vector3 ogScale;
    float ogMass;
    CircleCollider2D cc;
    float ogRadius;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected override bool onTrigger()
    {
        //Originals 
        ogScale = orb.transform.localScale;
        cc = orb.GetComponent<CircleCollider2D>();

        if (orb)
        {
            ogMass = orb.mass;
        }

        if (cc)
        {
            ogRadius = cc.radius;
        }

        //Multipliers
        orb.transform.localScale = ogScale * sizeMultiplier;

        if (orb)
        {
            orb.mass = ogMass * massMultiplier;
        }

        /*
        if (cc)
        {
            cc.radius = ogRadius * sizeMultiplier;
        }
        */

        return true;
    }

    protected override void onWindowEnd() 
    {
        orb.transform.localScale = ogScale;
        
        if (orb)
        {
            orb.mass = ogMass;
        }

        if (cc)
        {
            cc.radius = ogRadius;
        }
    }
    protected override void onWindowFixedUpdate() { }
}
