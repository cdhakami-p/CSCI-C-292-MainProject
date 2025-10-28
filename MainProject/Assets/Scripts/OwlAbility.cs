using UnityEngine;

public class OwlAbility : AbilityAC
{

    [SerializeField] float invisibleAlpha = 0.3f;
    [SerializeField] float visibleAlpha = 1f;

    SpriteRenderer[] srs;

    [SerializeField] string owlLayer = "OwlLayer";
    int ogLayer;

    void setAlpha(float a)
    {
        if (srs == null)
        {
            srs = GetComponentsInChildren<SpriteRenderer>(true);
        }

        for (int i = 0; i < srs.Length; i++)
        {
            var c = srs[i].color;
            c.a = a;
            srs[i].color = c;
        }
    }

    protected override bool onTrigger()
    {
        setAlpha(invisibleAlpha);

        ogLayer = gameObject.layer;
        gameObject.layer = LayerMask.NameToLayer(owlLayer);

        return true;
    }

    protected override void onWindowEnd() 
    {
        gameObject.layer = ogLayer;
        setAlpha(visibleAlpha);
    }
    protected override void onWindowFixedUpdate() { }
}
