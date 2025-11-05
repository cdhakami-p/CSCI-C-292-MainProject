using Mono.Cecil;
using UnityEngine;
using UnityEngine.Rendering;

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
        GameObject topPrefab = GetPrefab(GameData.topPlayerName);
        Debug.Log("Top Player Name: " + GameData.topPlayerName);
        GameObject bottomPrefab = GetPrefab(GameData.bottomPlayerName);
        Debug.Log("Bottom Player Name: " + GameData.bottomPlayerName);


        Vector3 topSpawnPosition = topSpawn1.position;
        Vector3 bottomSpawnPosition = bottomSpawn1.position;

        var topPlayer1 = Instantiate(topPrefab, topSpawnPosition, Quaternion.Euler(0f,0f,180f));
        var bottomPlayer1 = Instantiate(bottomPrefab, bottomSpawnPosition, Quaternion.identity);
        //0, playerPrefab.Length

        Outline(topPlayer1, Color.red);
        Outline(bottomPlayer1, Color.lightBlue);

        topPlayer1.GetComponent<PlayerController>().setUseArrows(true);
        bottomPlayer1.GetComponent<PlayerController>().setUseArrows(false);

        var ui = FindFirstObjectByType<GameUIManager>();
        if (ui != null)
        {
            var topRb = topPlayer1.GetComponent<Rigidbody2D>();
            var bottomRb = bottomPlayer1.GetComponent<Rigidbody2D>();

            if (topRb != null) {
                ui.RegisterPlayers(true, topRb);
            }

            if (bottomRb != null) {
                ui.RegisterPlayers(false, bottomRb);
            }
        }
    }

    void Outline(GameObject obj, Color color, float thickness = 0.1f)
    {
        var outline = obj.transform.Find("ring")?.GetComponent<SpriteRenderer>();

        if (outline)
        {
            outline.color = color;
        }
    }

    private GameObject GetPrefab(string playerName)
    {
        if (string.IsNullOrEmpty(playerName))
        {
            return playerPrefab[Random.Range(0, playerPrefab.Length)];
        }

        foreach (var prefab in playerPrefab)
        {
            if (prefab != null && prefab.name == playerName)
            {
                return prefab;
            }
        }

        Debug.LogWarning($"PlayerSpawner: no prefab found matching '{playerName}'");
        return playerPrefab[Random.Range(0, playerPrefab.Length)];
    }
}
