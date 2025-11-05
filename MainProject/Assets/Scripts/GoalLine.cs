using UnityEngine;

public class GoalLine : MonoBehaviour
{

    [SerializeField] private bool isTopGoalLine = true;

    public string ballTag = "Ball";

    private GameUIManager ui;


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
        if (other.CompareTag(ballTag))
        {
            Debug.Log("Goal!");
            
            if (ui != null)
            {
                ui.AddScore(isTopGoalLine);
            }
        }
    }
}
