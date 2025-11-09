using UnityEditor.UI;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class PlayerController : MonoBehaviour
{

    [SerializeField] private bool useArrows = false;

    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float turnSpeed = 100f;
    [SerializeField] private float friction = 0.1f;

    [SerializeField] private float boostStrength = 10f;
    [SerializeField] private float boostCooldown = 3f;

    private Rigidbody2D rb;

    private float nextBoostTime = 0f;
    private bool prevBoostPressed = false;

    private bool isAI = false;
    private float aiForward = 0f;
    private float aiTurn = 0f;
    private bool aiBoost = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        nextBoostTime = Time.time + boostCooldown;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float forward, turn;
        bool boost;

        if (isAI)
        {
            forward = aiForward;
            turn = aiTurn;
            boost = aiBoost;
        }
        else
        {
            if (useArrows)
            {
                forward = (Input.GetKey(KeyCode.UpArrow) ? 1 : 0) + (Input.GetKey(KeyCode.DownArrow) ? -1 : 0);
                turn = (Input.GetKey(KeyCode.RightArrow) ? 1 : 0) + (Input.GetKey(KeyCode.LeftArrow) ? -1 : 0);
                boost = Keyboard.current.rightShiftKey.isPressed;
            }
            else
            {
                forward = (Input.GetKey(KeyCode.W) ? 1 : 0) + (Input.GetKey(KeyCode.S) ? -1 : 0);
                turn = (Input.GetKey(KeyCode.D) ? 1 : 0) + (Input.GetKey(KeyCode.A) ? -1 : 0);
                boost = Keyboard.current.leftShiftKey.isPressed;
            }
        }

        

        Vector2 fwd = transform.up;
        Vector2 right = transform.right;

        rb.AddForce(fwd * forward * moveSpeed, ForceMode2D.Force);

        float lateral = Vector2.Dot(rb.linearVelocity, right);
        rb.linearVelocity -= right * lateral * friction;

        rb.MoveRotation(rb.rotation - turn * turnSpeed * Time.fixedDeltaTime);

        bool boostPressed = boost && !prevBoostPressed;
        prevBoostPressed = boost;

        if (boostPressed && Time.time >= nextBoostTime)
        {
            rb.AddForce(fwd * boostStrength, ForceMode2D.Impulse);
            nextBoostTime = Time.time + boostCooldown;
        }
    }

    public void setUseArrows(bool value)
    {
        useArrows = value;
        var ac = GetComponent<AbilityController>();
        if (ac)
        {
            ac.setUseArrows(value);
        }
    }

    public void ResetBoostCooldown()
    {
        nextBoostTime = Time.time + boostCooldown;
        prevBoostPressed = false;
    }

    public void EnableAI()
    {
        isAI = true;
    }

    public void SetAIInput(float forward, float turn, bool boost)
    {
        aiForward = forward;
        aiTurn = turn;
        aiBoost = boost;
    }
}
