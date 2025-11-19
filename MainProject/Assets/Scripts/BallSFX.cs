using UnityEngine;

public class BallSFX : MonoBehaviour
{

    [SerializeField] private AudioClip bounceSFX;
    [SerializeField] private float minVelocity = 0.5f;
    [SerializeField] private float maxVelocity = 10f;
    [SerializeField] private float baseVolume = 0.8f;
    [SerializeField] private float cooldown = 0.1f;

    private float lastPlayTime = -Mathf.Infinity;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (bounceSFX == null || AudioManager.Instance == null) return;

        if (Time.time - lastPlayTime < cooldown) return;

        float speed = collision.relativeVelocity.magnitude;

        if (speed < minVelocity) return;

        float t = Mathf.Clamp01(speed / maxVelocity);
        float volume = baseVolume * t;

        AudioManager.Instance.PlaySFX(bounceSFX, volume);
        lastPlayTime = Time.time;
    }
}
