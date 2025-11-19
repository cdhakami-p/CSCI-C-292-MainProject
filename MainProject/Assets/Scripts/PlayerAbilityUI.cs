using UnityEditor.Build;
using UnityEngine;

public class PlayerAbilityUI : MonoBehaviour
{
    [SerializeField] private SpriteRenderer ringRenderer;

    [SerializeField] private Color defaultColor = Color.white;
    [SerializeField] private Color boostReadyColor = Color.green;
    [SerializeField] private Color abilityReadyColor = Color.yellow;
    
    [SerializeField] private float pulseSpeed = 0.5f;

    private PlayerController pc;
    private AbilityAC ac;
    
    private bool prevBoostReady = false;
    private bool prevAbilityReady = false;

    private Coroutine pulseRoutine;

    private void Start()
    {
        if (ringRenderer != null)
        {
            defaultColor = ringRenderer.color;
        }
    }

    private void Awake()
    {
        pc = GetComponentInParent<PlayerController>();
        ac = GetComponentInParent<AbilityAC>();
        
        if (ringRenderer == null)
        {
            var ring = transform.Find("ring");
            if (ring != null)
            {
                ringRenderer = ring.GetComponent<SpriteRenderer>();
            }
        }
    }

    private void Update()
    {
        if (ringRenderer == null) return;

        bool boostReady = pc != null && pc.IsBoostReady();
        bool abilityReady = ac != null && ac.isAbilityReady();

        if (boostReady && !prevBoostReady)
        {
            TriggerPulse(boostReadyColor);
        }

        if (abilityReady && !prevAbilityReady)
        {
            TriggerPulse(abilityReadyColor);
        }

        prevBoostReady = boostReady;
        prevAbilityReady = abilityReady;
    }

    private void TriggerPulse(Color pulseColor)
    {
        if (pulseRoutine != null)
        {
            StopCoroutine(pulseRoutine);
        }

        pulseRoutine = StartCoroutine(PulseOnce(pulseColor));
    }

    private System.Collections.IEnumerator PulseOnce(Color pulseColor)
    {
        ringRenderer.color = pulseColor;

        float elapsed = 0f;
        while (elapsed < pulseSpeed)
        {
            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }

        ringRenderer.color = defaultColor;
        pulseRoutine = null;
    }
}

