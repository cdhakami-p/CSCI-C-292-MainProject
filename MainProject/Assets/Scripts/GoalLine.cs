using UnityEngine;

public class GoalLine : MonoBehaviour
{

    [SerializeField] private bool isTopGoalLine = true;

    public string ballTag = "Ball";

    private GameUIManager ui;
    private bool canScore = true;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ui = FindFirstObjectByType<GameUIManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!canScore)
        {
            return;
        }

        if (other.CompareTag(ballTag))
        {
            Debug.Log("Goal!");
            
            if (ui != null)
            {
                ui.GoalScored(isTopGoalLine);
            }

            canScore = false;
        }
    }

    public void SetCanScore(bool value)
    {
        canScore = value;
    }
}
