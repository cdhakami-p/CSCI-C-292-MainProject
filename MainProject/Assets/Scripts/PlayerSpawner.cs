using Mono.Cecil;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerSpawner : MonoBehaviour
{

    [SerializeField] private GameObject[] playerPrefab;

    [SerializeField] private Transform topSpawn1;
    [SerializeField] private Transform topSpawn2;
    [SerializeField] private Transform topSpawn3;

    [SerializeField] private Transform bottomSpawn1;
    [SerializeField] private Transform bottomSpawn2;
    [SerializeField] private Transform bottomSpawn3;

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

        SpawnPlayers(GameData.selectedMode);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void SpawnPlayers(string mode)
    {
        if (string.IsNullOrEmpty(mode))
        {
            mode = "1v1 - Solo";
        }

        if (mode == "1v1 - Solo")
        {
            SpawnOne(bottomSpawn1, GameData.bottomPlayerName, isBottomTeam: true, isHuman: true);
            SpawnOne(topSpawn1, null, isBottomTeam: false, isHuman: false);
        }
        else if (mode == "1v1 - Co-op")
        {
            SpawnOne(bottomSpawn1, GameData.bottomPlayerName, isBottomTeam: true, isHuman: true);
            SpawnOne(topSpawn1, GameData.topPlayerName, isBottomTeam: false, isHuman: true);
        }
        else if (mode == "3v3 - Solo")
        {
            SpawnOne(bottomSpawn1, GameData.bottomPlayerName, isBottomTeam: true, isHuman: true);
            SpawnOne(bottomSpawn2, null, isBottomTeam: true, isHuman: false);
            SpawnOne(bottomSpawn3, null, isBottomTeam: true, isHuman: false);

            SpawnOne(topSpawn1, null, isBottomTeam: false, isHuman: false);
            SpawnOne(topSpawn2, null, isBottomTeam: false, isHuman: false);
            SpawnOne(topSpawn3, null, isBottomTeam: false, isHuman: false);
        }
        else if (mode == "3v3 - Co-op")
        {
            SpawnOne(bottomSpawn1, GameData.bottomPlayerName, isBottomTeam: true, isHuman: true);
            SpawnOne(bottomSpawn2, null, isBottomTeam: true, isHuman: false);
            SpawnOne(bottomSpawn3, null, isBottomTeam: true, isHuman: false);

            SpawnOne(topSpawn1, GameData.topPlayerName, isBottomTeam: false, isHuman: true);
            SpawnOne(topSpawn2, null, isBottomTeam: false, isHuman: false);
            SpawnOne(topSpawn3, null, isBottomTeam: false, isHuman: false);
        }
        else
        {
            Debug.LogError("PlayerSpawner - unknown mode: " + mode);
            SpawnOne(bottomSpawn1, GameData.bottomPlayerName, isBottomTeam: true, isHuman: true);
            SpawnOne(topSpawn1, null, isBottomTeam: false, isHuman: false);
        }
    }

    GameObject SpawnOne(Transform spawn, string playerName, bool isBottomTeam, bool isHuman)
    {
        if (spawn == null)
        {
            Debug.LogError("PlayerSpawner - spawn point is null");
            return null;
        }

        GameObject prefab = GetPrefab(playerName);

        Quaternion rotation = isBottomTeam ? Quaternion.identity : Quaternion.Euler(0, 0, 180);

        GameObject player = Instantiate(prefab, spawn.position, rotation);

        Outline(player, isBottomTeam ? Color.cyan : Color.red);

        var controller = player.GetComponent<PlayerController>();
        if (controller != null)
        {
            if (isHuman)
            {
                controller.setUseArrows(!isBottomTeam);
            }
            else
            {
                var ai = player.AddComponent<AIController>();
                ai.isTopTeam = !isBottomTeam;
                controller.EnableAI();
            }
        }

        var ui = FindFirstObjectByType<GameUIManager>();
        if (ui != null)
        {
            var rb = player.GetComponent<Rigidbody2D>();

            if (rb != null)
            {
                ui.RegisterPlayers(!isBottomTeam, rb);
            }
        }

        return player;
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

        return playerPrefab[Random.Range(0, playerPrefab.Length)];
    }
}
