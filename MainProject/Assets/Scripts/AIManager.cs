using UnityEngine;

public class AIManager : MonoBehaviour
{
    [SerializeField] private Rigidbody2D ball;
    [SerializeField] private float interval = 0.5f;

    private float nextAssign = 0f;

    private void Start()
    {

    }

    void Update()
    {
        if (ball == null) return;

        if (Time.time >= nextAssign)
        {
            AssignDefender();
            nextAssign = Time.time + interval;
        }
    }

    private void AssignDefender()
    {
        AIController[] ai= FindObjectsByType<AIController>(FindObjectsSortMode.None);
        if (ai.Length == 0) return;

        AIController topDefender = null;
        AIController bottomDefender = null;

        float topDistance = -1f;
        float bottomDistance = -1f;

        foreach (var a in ai)
        {
            if (a.isTopTeam)
            {
                float dist = Vector2.Distance(a.transform.position, ball.transform.position);
                if (topDistance < 0f || dist > topDistance)
                {
                    topDistance = dist;
                    topDefender = a;
                }
            }
            else
            {
                float dist = Vector2.Distance(a.transform.position, ball.transform.position);
                if (bottomDistance < 0f || dist > bottomDistance)
                {
                    bottomDistance = dist;
                    bottomDefender = a;
                }
            }
        }

        foreach (var a in ai)
        {
            if (a == null) continue;
            a.SetDefender(false);
        }

        int topAI = 0;
        int bottomAI = 0;
        foreach (var a in ai)
        {
            if (a == null) continue;
            if (a.isTopTeam)
            {
                topAI++;
            }
            else
            {
                bottomAI++;
            }
        }

        if (topAI > 1 && topDefender != null)
        {
            topDefender.SetDefender(true);
        }

        if (bottomAI > 1 && bottomDefender != null)
        {
            bottomDefender.SetDefender(true);
        }
    }

    public void RegisterBall(Rigidbody2D rb)
    {
        ball = rb;
    }
}
