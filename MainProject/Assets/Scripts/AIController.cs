using UnityEngine;
using UnityEngine.Rendering;

public class AIController : MonoBehaviour
{

    [SerializeField] private Transform ball;
    [SerializeField] private Transform enemyGoal;

    [SerializeField] private float turnTolerance = 5f;
    [SerializeField] private float turnScale = 45f;
    [SerializeField] private float forwardAngle = 60f;

    [SerializeField] private float boostDistance = 3f;
    [SerializeField] private float boostAngle = 10f;

    [SerializeField] private float abilityDistance = 3f;
    [SerializeField] private float abilityMovement = 0.5f;

    [SerializeField] private float attackDistance = 1f;

    public bool isTopTeam = false;

    private PlayerController pc;
    private AbilityAC ability;
    private Rigidbody2D rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        pc = GetComponent<PlayerController>();
        ability = GetComponent<AbilityAC>();
        rb = GetComponent<Rigidbody2D>();

        if (ball == null)
        {
            var ballObj = GameObject.FindGameObjectWithTag("Ball");
            if (ballObj != null)
            {
                ball = ballObj.transform;
            }
        }

        if (enemyGoal == null)
        {
            if (isTopTeam)
            {
                var goal = GameObject.FindGameObjectWithTag("BottomGoal");
                if (goal != null)
                {
                    enemyGoal = goal.transform;
                }
            }
            else
            {
                var goal = GameObject.FindGameObjectWithTag("TopGoal");
                if (goal != null)
                {
                    enemyGoal = goal.transform;
                }
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (pc == null || ball == null)
        {
            return;
        }

        Vector2 toBall = (ball.position - transform.position);
        float distanceToBall = toBall.magnitude;

        Vector2 dir = toBall;

        if (enemyGoal != null && distanceToBall < attackDistance)
        {
            dir = (Vector2)(enemyGoal.position - transform.position);
        }


        Vector2 forward = transform.up;
        float angle = Vector2.SignedAngle(forward, dir.normalized);

        float turnInput = 0f;
        if (Mathf.Abs(angle) > turnTolerance)
        {
            turnInput = Mathf.Clamp(-angle / turnScale, -1f, 1f);
        }
        else 
        {
            turnInput = 0f;
        }

        float forwardInput = Mathf.Abs(angle) < forwardAngle ? 1f : 0.5f;

        
        bool boost = false;

        if (distanceToBall < boostDistance && Mathf.Abs(angle) < boostAngle)
        {
            boost = true;
        }


        if (ability != null && ability.isAbilityReady())
        {
            if (distanceToBall < abilityDistance)
            {
                Vector2 vel = rb != null ? rb.linearVelocity : Vector2.zero;

                if (vel.sqrMagnitude > 0.01f)
                {
                    float dot = Vector2.Dot(vel.normalized, toBall.normalized);
                    if (dot > abilityMovement)
                    {
                        ability.triggerAbility(rb);
                    }
                } else
                {
                    ability.triggerAbility(rb);
                }
            }
        }

        pc.SetAIInput(forwardInput, turnInput, boost);
    }
}
