using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Serialization;

public class EnemyManager : MonoBehaviour
{
    [Header("Spawning")]
    public GameObject enemyPrefab; // Assign your Enemy prefab here
    public float spawnCircleRadius = 10f;
    public float spawnInterval = 5f; // Time between spawning new groups
    public float spawnIntervalWithinGroup = 0.2f; // Time between enemies in the same group
    public float spawnZDepth = 5f;

    [Header("Enemy Stats")]
    public int baseEnemyCount = 3; // Initial number of enemies per group
    public float baseEnemySpeed = 2f; // Initial speed of enemies

    [Header("Group Movement")]
    public float baseEnemyNoiseMultiplier = 0.5f; // How much groups rotate randomly

    [Header("Progression (Per Minute)")]
    public int enemyCountIncreasePerMin = 1;
    public float enemySpeedIncreasePerMin = 0.5f;
    public float offsetMultiplierIncreasePerMin = 0.1f;

    [Header("Runtime Values (Read Only)")]
    [SerializeField] private int currentEnemyCount;
    [SerializeField] private float currentEnemySpeed;
    [SerializeField] private float currentEnemyOffsetMultiplier;

    private Transform playerTransform;
    private List<Transform> activeEnemyGroups = new List<Transform>();
    private List<float> groupPerlinOffsets = new List<float>(); // For unique Perlin noise per group
    private float gameStartTime;

    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            playerTransform = playerObj.transform;
        }
        else
        {
            Debug.LogError("EnemyManager: Player object with tag 'Player' not found!");
            this.enabled = false; // Disable manager if player isn't found
            return;
        }

        gameStartTime = Time.time;
        UpdateProgressionStats(); // Set initial stats

        // Start spawning groups
        InvokeRepeating(nameof(SpawnEnemyGroup), spawnInterval, spawnInterval);
    }

    void Update()
    {
        UpdateProgressionStats();
        RotateEnemyGroups();
        CleanupEmptyGroups(); // Periodically remove empty group transforms
    }

    void UpdateProgressionStats()
    {
        float timeSinceStart = Time.time - gameStartTime;
        float minutesPassed = timeSinceStart / 60f;

        currentEnemyCount = baseEnemyCount + Mathf.FloorToInt(minutesPassed * enemyCountIncreasePerMin);
        currentEnemySpeed = baseEnemySpeed + (minutesPassed * enemySpeedIncreasePerMin);
        currentEnemyOffsetMultiplier = baseEnemyNoiseMultiplier + (minutesPassed * offsetMultiplierIncreasePerMin);
    }

    void SpawnEnemyGroup()
    {
        if (enemyPrefab == null || playerTransform == null) return;

        // Create a parent transform for the group
        GameObject groupGO = new GameObject($"EnemyGroup_{Time.time}");
        Transform enemyGroupTransform = groupGO.transform;
        enemyGroupTransform.position = Vector3.zero; // Or center of screen if needed
        activeEnemyGroups.Add(enemyGroupTransform);
        groupPerlinOffsets.Add(Random.Range(0f, 100f)); // Unique offset for Perlin noise

        StartCoroutine(SpawnEnemiesInGroup(enemyGroupTransform));
    }

    IEnumerator SpawnEnemiesInGroup(Transform groupTransform)
    {
        Vector2 randomDirection = Random.insideUnitCircle.normalized;
        Vector3 firstSpawnPos = (Vector3)(randomDirection * spawnCircleRadius) + new Vector3(0, 0, spawnZDepth); // TODO // Position on the circle
        Vector3 spawnOffsetDirection = Random.insideUnitCircle.normalized * 0.5f; // Slight offset direction for the group

        for (int i = 0; i < currentEnemyCount; i++)
        {
            // Calculate position: first enemy on circle, others slightly offset
            Vector3 spawnPos = firstSpawnPos + spawnOffsetDirection * i;

            GameObject enemyGO = Instantiate(enemyPrefab, spawnPos, Quaternion.identity, groupTransform);
            Enemy enemyScript = enemyGO.GetComponent<Enemy>();

            if (enemyScript != null)
            {
                enemyScript.Init(currentEnemySpeed, playerTransform);
            }
            else
            {
                Debug.LogError("Enemy prefab does not contain an Enemy script!");
            }

            yield return new WaitForSeconds(spawnIntervalWithinGroup);
        }
    }

    void RotateEnemyGroups()
    {
        for (int i = activeEnemyGroups.Count - 1; i >= 0; i--) // Iterate backwards for safe removal
        {
            Transform groupTransform = activeEnemyGroups[i];
            if (groupTransform == null) // Group might have been destroyed
            {
                activeEnemyGroups.RemoveAt(i);
                groupPerlinOffsets.RemoveAt(i);
                continue;
            }

            // Use Perlin noise for smooth random rotation
            float noise = Mathf.PerlinNoise(Time.time * 0.1f, groupPerlinOffsets[i]); // Use unique offset
            float rotationAmount = (noise * 2f - 1f) * currentEnemyOffsetMultiplier * Time.deltaTime * 10f; // Scale noise to -1 to 1 and apply multiplier

            groupTransform.Rotate(0, 0, rotationAmount);
        }
    }

     void CleanupEmptyGroups()
     {
         // Optional: If groups might become empty and you want to remove the parent transform
         for (int i = activeEnemyGroups.Count - 1; i >= 0; i--)
         {
             if (activeEnemyGroups[i] != null && activeEnemyGroups[i].childCount == 0)
             {
                 Destroy(activeEnemyGroups[i].gameObject);
                 activeEnemyGroups.RemoveAt(i);
                 groupPerlinOffsets.RemoveAt(i);
             }
         }
     }


    // Visualize the spawn circle in the editor
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, spawnCircleRadius); // Assumes manager is at (0,0,0) or screen center
    }
}