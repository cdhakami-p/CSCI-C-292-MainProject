using UnityEngine;

public class ParrotAbility : AbilityAC
{
    [SerializeField] float boostStrength = 10f;

    protected override bool onTrigger()
    {
        if (orb != null)
        {
            orb.AddForce(orb.transform.up * boostStrength, ForceMode2D.Impulse);
        }
        
        return false;
    }

    protected override void onWindowEnd() { }
    protected override void onWindowFixedUpdate() { }
}
