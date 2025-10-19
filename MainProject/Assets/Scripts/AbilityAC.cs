using UnityEngine;

public abstract class AbilityAC : MonoBehaviour
{

    [SerializeField] protected float abilityCooldown = 3f;
    protected float nextAbilityTime = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool isAbilityReady()
    {
        return Time.time >= nextAbilityTime;
    }

    public float cooldownRemaining()
    {
        return Mathf.Max(0f, nextAbilityTime - Time.time);
    }

    public void triggerAbility(Rigidbody2D rb)
    {
        if (!isAbilityReady())
        {
            return;
        }
        
        trigger(rb);
        nextAbilityTime = Time.time + abilityCooldown;
    }

    protected abstract void trigger(Rigidbody2D rb);
}
