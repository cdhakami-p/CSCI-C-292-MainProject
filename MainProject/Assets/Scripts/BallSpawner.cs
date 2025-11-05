using UnityEngine;

public class BallSpawner : MonoBehaviour
{

    [SerializeField] GameObject ballPrefab;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Debug.LogError("BallSpawner - no ballPrefab assigned");
        SpawnBall();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SpawnBall()
    {
        Vector3 spawnPosition = transform.position;
        GameObject ballObj = Instantiate(ballPrefab, spawnPosition, Quaternion.identity);

        var ui = FindFirstObjectByType<GameUIManager>();
        if (ui != null)
        {
            var rb = ballObj.GetComponent<Rigidbody2D>();
            if (rb != null) {
                ui.RegisterBall(rb);
            }
        }
    }
}
