using UnityEngine;

public class NarwalAbility : AbilityAC
{
    [SerializeField] Vector2 offset = new Vector2(0, 1);
    [SerializeField] float releaseForce = 5.0f;
    [SerializeField] string ballTag = "Ball";

    Rigidbody2D brb;
    Collider2D bcol;

    int ogLayer;
    RigidbodyType2D ogType;

    protected override bool onTrigger()
    {
        return true;
    }

    private void OnCollisionEnter2D(Collision2D c)
    {
        if (!abilityActive || c.rigidbody == null || brb != null)
        {
            return;
        }

        if (!c.collider.CompareTag(ballTag) && !c.otherCollider.CompareTag(ballTag))
        {
            return;
        }
        
        brb = c.rigidbody;
        bcol = brb.GetComponent<Collider2D>();
        ogType = brb.bodyType;
        ogLayer = brb.gameObject.layer;
        
        brb.linearVelocity = Vector2.zero;
        brb.angularVelocity = 0f;
        brb.bodyType = RigidbodyType2D.Kinematic;

        if (bcol)
        {
            bcol.enabled = false;
        }

        brb.transform.SetParent(transform, true);
        brb.transform.position = transform.TransformPoint(offset);
        brb.transform.rotation = transform.rotation;
    }

    protected override void onWindowEnd()
    {
        if (brb == null)
        {
            return;
        }
        
        brb.transform.SetParent(null, true);
        brb.bodyType = ogType;
        brb.gameObject.layer = ogLayer;

        if (bcol)
        {
            bcol.enabled = true;
        }

        brb.AddForce(transform.up * releaseForce, ForceMode2D.Impulse);
        
        brb = null;
        bcol = null;
    }

    protected override void onWindowFixedUpdate() { }
}
