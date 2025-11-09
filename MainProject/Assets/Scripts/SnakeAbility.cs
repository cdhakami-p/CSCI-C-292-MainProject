using UnityEngine;

public class SnakeAbility : AbilityAC
{

    [SerializeField] float speedMultiplier = 6f;

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
