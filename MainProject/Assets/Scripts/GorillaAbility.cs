using UnityEngine;

public class GorillaAbility : AbilityAC
{

    [SerializeField] private float strengthMultiplier = 4.0f;
    [SerializeField] private string ballTag = "Ball";

    protected override bool onTrigger()
    {
        return true;
    }

    private void OnCollisionEnter2D(Collision2D c)
    {
        if (!abilityActive || c.rigidbody == null)
        {
            return;
        }

        if (!c.collider.CompareTag(ballTag) && !c.otherCollider.CompareTag(ballTag))
        {
            return;
        }
        
        c.rigidbody.linearVelocity *= strengthMultiplier;
    }

    protected override void onWindowEnd() { }
    protected override void onWindowFixedUpdate() { }
}
