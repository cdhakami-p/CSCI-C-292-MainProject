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

    public bool isDefender = false;
    private bool atDefensePosition = false;
    [SerializeField] private float defenderDistance = 2.5f;
    [SerializeField] private float defenderRadius = 7.5f;
    [SerializeField] private Transform ownGoal;

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

        if (ownGoal == null)
        {
            if (isTopTeam)
            {
                var goal = GameObject.FindGameObjectWithTag("TopGoal");
                if (goal != null)
                {
                    ownGoal = goal.transform;
                    print("Assigned own goal for top team");
                }
            }
            else
            {
                var goal = GameObject.FindGameObjectWithTag("BottomGoal");
                if (goal != null)
                {
                    ownGoal = goal.transform;
                    print("Assigned own goal for bottom team");
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

        bool carryingBall = (ball.parent == this.transform);

        Vector2 dir = transform.up;

        bool isDefending = false;

        if (isDefender && ownGoal != null && !carryingBall)
        {
            float ballToGoal = Vector2.Distance(ball.position, ownGoal.position);

            if (ballToGoal > defenderRadius)
            {
                float guard = ownGoal.position.y + (isTopTeam ? -defenderDistance : defenderDistance);
                Vector2 defendPos = new Vector2(ownGoal.position.x, guard);

                float distToDefendPos = Vector2.Distance(transform.position, defendPos);
                atDefensePosition = distToDefendPos <= 3f;

                if (!atDefensePosition)
                {
                    dir = defendPos - (Vector2)transform.position;
                } else {
                    dir = (Vector2)(ball.position - transform.position);
                    //print("At defense position");
                }

                isDefending = true;
            } else
            {
                atDefensePosition = false;
            }
        } else
        {
            atDefensePosition = false;
        }

        if (!isDefending)
        {
            if (carryingBall && enemyGoal != null)
            {
                dir = (Vector2)(enemyGoal.position - transform.position);
            }
            else
            {
                Vector2 toBall = (Vector2)(ball.position - transform.position);
                float distToBall = toBall.magnitude;

                dir = toBall;

                if (enemyGoal != null && distToBall < attackDistance)
                {
                    dir = (Vector2)(enemyGoal.position - transform.position);
                }
            }
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

        if (isDefending && atDefensePosition)
        {
            forwardInput = 0f;
        }


        bool boost = false;
        float distanceToBall = Vector2.Distance(transform.position, ball.position);

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
                    Vector2 toBall = (Vector2)(ball.position - transform.position);
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

    public void SetDefender(bool defender)
    {
        isDefender = defender;
    }
}
