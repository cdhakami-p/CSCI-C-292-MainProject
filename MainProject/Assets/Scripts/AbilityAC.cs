using UnityEngine;

public abstract class AbilityAC : MonoBehaviour
{

    [SerializeField] protected float abilityCooldown = 6f;
    [SerializeField] protected float abilityDuration = 6f;
    
    protected float nextAbilityTime = 0f;
    protected bool abilityActive = false;
    protected float abilityEndTime = 0f;
    protected Rigidbody2D orb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        nextAbilityTime = Time.time + abilityCooldown;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!abilityActive)
        {
            return;
        }

        onWindowFixedUpdate();
        if (Time.time >= abilityEndTime)
        {
            abilityActive = false;
            onWindowEnd();
            nextAbilityTime = Time.time + abilityCooldown;
        }
    }

    public bool isAbilityReady()
    {
        return !abilityActive && Time.time >= nextAbilityTime;
    }

    public float cooldownRemaining()
    {
        return abilityActive ? Mathf.Max(0, abilityEndTime - Time.time) : Mathf.Max(0, nextAbilityTime - Time.time);
    }

    public void triggerAbility(Rigidbody2D rb)
    {
        if (!isAbilityReady())
        {
            return;
        }

        orb = rb;
        bool startWindow = onTrigger();
        print("Ability activated");

        if (startWindow && abilityDuration > 0f)
        {
            abilityActive = true;
            abilityEndTime = Time.time + abilityDuration;
        }
        else
        {
            nextAbilityTime = Time.time + abilityCooldown;
        }
    }

    public void ResetAbilityCooldown()
    {
        abilityActive = false;
        abilityEndTime = 0f;
        orb = null;

        nextAbilityTime = Time.time + abilityCooldown;
    }

    protected abstract bool onTrigger();
    protected abstract void onWindowFixedUpdate();
    protected abstract void onWindowEnd();
}
