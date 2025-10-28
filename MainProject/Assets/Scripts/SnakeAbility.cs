using UnityEngine;

public class SnakeAbility : AbilityAC
{

    [SerializeField] float speedMultiplier = 6f;

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
        return true;
    }
    protected override void onWindowEnd() { }
    protected override void onWindowFixedUpdate() 
    {
        if (orb)
        {
            orb.AddForce(orb.transform.up * speedMultiplier, ForceMode2D.Force);
        }
    }
}
