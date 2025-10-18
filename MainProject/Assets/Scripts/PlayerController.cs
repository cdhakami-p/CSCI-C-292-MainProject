using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField] private bool useArrows = false;

    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float turnSpeed = 100f;
    [SerializeField] private float friction = 0.1f;

    private Rigidbody2D rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float forward, turn;

        if (useArrows)
        {
            forward = (Input.GetKey(KeyCode.UpArrow) ? 1 : 0) + (Input.GetKey(KeyCode.DownArrow) ? -1 : 0);
            turn = (Input.GetKey(KeyCode.RightArrow) ? 1 : 0) + (Input.GetKey(KeyCode.LeftArrow) ? -1 : 0);
        }
        else
        {
            forward = (Input.GetKey(KeyCode.W) ? 1 : 0) + (Input.GetKey(KeyCode.S) ? -1 : 0);
            turn = (Input.GetKey(KeyCode.D) ? 1 : 0) + (Input.GetKey(KeyCode.A) ? -1 : 0);
        }

        Vector2 fwd = transform.up;
        Vector2 right = transform.right;

        rb.AddForce(fwd * forward * moveSpeed, ForceMode2D.Force);

        float lateral = Vector2.Dot(rb.linearVelocity, right);
        rb.linearVelocity -= right * lateral * friction;

        rb.MoveRotation(rb.rotation - turn * turnSpeed * Time.fixedDeltaTime);
    }

    public void SetUseArrows(bool value)
    {
        useArrows = value;
    }

}
