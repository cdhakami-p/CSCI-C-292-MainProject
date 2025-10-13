using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{

    [SerializeField] private GameObject[] playerPrefab;

    [SerializeField] private Transform topSpawn1;
    [SerializeField] private Transform bottomSpawn1;

    //[SerializeField] private int topPlayerIndex = 0;
    //[SerializeField] private int bottomPlayerIndex = 1;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (playerPrefab.Length < 2)
        {
            Debug.LogError("PlayerSpawner - not enough player prefabs assigned");
            return;
        }

        if (topSpawn1 == null || bottomSpawn1 == null)
        {
            Debug.LogError("PlayerSpawner - spawn points not assigned");
            return;
        }

        SpawnPlayers();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SpawnPlayers()
    {
        Vector3 topSpawnPosition = topSpawn1.position;
        Vector3 bottomSpawnPosition = bottomSpawn1.position;
        Instantiate(playerPrefab[Random.Range(0,playerPrefab.Length-1)], topSpawnPosition, Quaternion.identity);
        Instantiate(playerPrefab[Random.Range(0, playerPrefab.Length-1)], bottomSpawnPosition, Quaternion.identity);
    }
}
