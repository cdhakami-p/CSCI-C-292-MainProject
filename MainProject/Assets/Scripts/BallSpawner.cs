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
        Instantiate(ballPrefab, spawnPosition, Quaternion.identity);
    }
}
