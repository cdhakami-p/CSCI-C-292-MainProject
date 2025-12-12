using UnityEngine;

public class ReplayRecord : MonoBehaviour
{
    private Vector3[] positionBuffer;
    private Quaternion[] rotationBuffer;
    private Vector3[] scaleBuffer;
    private float[] alphaBuffer;
    private Color[] ringBuffer;

    private int bufferSize;

    private Rigidbody2D rb;
    private PlayerController pc;
    private AIController ai;
    private AbilityAC ac;
    private SpriteRenderer sr;
    private SpriteRenderer rr;
    private PlayerAbilityUI abilityUI;

    public void Initialize(int size)
    {
        bufferSize = size;
        positionBuffer = new Vector3[size];
        rotationBuffer = new Quaternion[size];
        scaleBuffer =new Vector3[size];
        alphaBuffer = new float[size];
        ringBuffer = new Color[size];

        rb = GetComponent<Rigidbody2D>();
        pc = GetComponent<PlayerController>();
        ai = GetComponent<AIController>();
        ac = GetComponent<AbilityAC>();
        sr = GetComponent<SpriteRenderer>();

        Transform ring = transform.Find("ring");
        if (ring != null)
        {
            rr = ring.GetComponent<SpriteRenderer>();
        }

        abilityUI = GetComponentInChildren<PlayerAbilityUI>(true);
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
        scaleBuffer[index] = transform.localScale;
        alphaBuffer[index] = (sr != null) ? sr.color.a : 1f;
        ringBuffer[index] = (rr != null) ? rr.color : Color.white;
    }

    public void PlaySample(int index)
    {
        if (index < 0 || index >= bufferSize) return;

        transform.position = positionBuffer[index];
        transform.rotation = rotationBuffer[index];
        transform.localScale = scaleBuffer[index];
        
        if (sr != null)
        {
            Color color = sr.color;
            color.a = alphaBuffer[index];
            sr.color = color;
        }

        if (rr != null)
        {
            rr.color = ringBuffer[index];
        }
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

        if (abilityUI != null)
        {
            abilityUI.enabled = !isReplay;
        }

        if (!isReplay)
        {
            if (sr != null)
            {
                Color c = sr.color;
                c.a = 1f;
                sr.color = c;
            }
        }
    }
}
