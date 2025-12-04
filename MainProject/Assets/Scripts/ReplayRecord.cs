using UnityEngine;

public class ReplayRecord : MonoBehaviour
{
    private Vector3[] positionBuffer;
    private Quaternion[] rotationBuffer;

    private int bufferSize;

    private Rigidbody2D rb;
    private PlayerController pc;
    private AIController ai;
    private AbilityAC ac;

    public void Initialize(int size)
    {
        bufferSize = size;
        positionBuffer = new Vector3[size];
        rotationBuffer = new Quaternion[size];

        rb = GetComponent<Rigidbody2D>();
        pc = GetComponent<PlayerController>();
        ai = GetComponent<AIController>();
        ac = GetComponent<AbilityAC>();
    }

    private void Awake()
    {
        if (ReplayManager.Instance != null)
        {
            ReplayManager.Instance.RegisterReplay(this);
        }
    }

    public void RecordSample(int index)
    {
        if (index < 0 || index >= bufferSize) return;

        positionBuffer[index] = transform.position;
        rotationBuffer[index] = transform.rotation;
    }

    public void PlaySample(int index)
    {
        if (index < 0 || index >= bufferSize) return;

        transform.position = positionBuffer[index];
        transform.rotation = rotationBuffer[index];
    }

    public void SetReplay(bool isReplay)
    {
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;

            rb.bodyType = isReplay ? RigidbodyType2D.Kinematic : RigidbodyType2D.Dynamic;
        }

        if (pc != null)
        {
            pc.enabled = !isReplay;
        }

        if (ai != null)
        {
            ai.enabled = !isReplay;
        }

        if (ac != null)
        {
            ac.enabled = !isReplay;
        }
    }
}
