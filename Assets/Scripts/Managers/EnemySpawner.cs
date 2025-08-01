using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform enemySpawnPoint;
    [SerializeField] private Mage enemyPrefab;

    [Header("Data")]
    [SerializeField] private float spawnTimeMin;
    [SerializeField] private float spawnTimeMax;

    private float spawnCooldownTimer;
    private float spawnCooldown;

    private void Update()
    {
        spawnCooldownTimer += Time.deltaTime;
        if (spawnCooldownTimer > spawnCooldown)
        {
            SpawnEnemy();
            ResetSpawnCooldownTimer();
        }
    }

    private void SpawnEnemy()
    {
        Mage newEnemy = Instantiate(enemyPrefab, transform);
        newEnemy.transform.position = enemySpawnPoint.transform.position;
        int randomTrack = Random.Range(0, 5);
        newEnemy.GetComponent<TrackBody>().MoveToTrack(randomTrack); 
    }

    private void ResetSpawnCooldownTimer()
    {
        spawnCooldownTimer = 0.0f;
        spawnCooldown = Random.Range(spawnTimeMin, spawnTimeMax);
    }
}
