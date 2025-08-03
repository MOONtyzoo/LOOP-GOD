using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform enemySpawnPoint;


    private List<EnemySpawnData> enemySpawnDataList;
    private List<float> spawnCooldownTimers = new List<float>();
    private List<float> spawnCooldowns = new List<float>();


    private void Update()
    {
        for (int i = 0; i < enemySpawnDataList.Count; i++)
        {
            EnemySpawnData enemySpawnData = enemySpawnDataList[i];
            spawnCooldownTimers[i] += Time.deltaTime;
            if (spawnCooldownTimers[i] > spawnCooldowns[i])
            {
                spawnCooldownTimers[i] = 0.0f;
                spawnCooldowns[i] = enemySpawnData.GetRandomSpawnCooldown();
                if (CountEnemies(enemySpawnData.prefab) < enemySpawnData.maxEnemies)
                {
                    SpawnEnemy(enemySpawnData.prefab);
                }
            }
        }
    }

    private void SpawnEnemy(GameObject enemyPrefab)
    {
        GameObject newEnemy = Instantiate(enemyPrefab, transform);
        newEnemy.transform.position = enemySpawnPoint.transform.position;
        int randomTrack = UnityEngine.Random.Range(0, 5);
        newEnemy.GetComponent<TrackBody>().MoveToTrack(randomTrack);
    }

    private int CountEnemies(GameObject enemyPrefab)
    {
        int count = 0;
        foreach (Transform child in transform)
        {
            if (child.name.Contains(enemyPrefab.name))
            {
                count++;
            }
        }
        return count;
    }

    public void SetEnemySpawnDataList(List<EnemySpawnData> newEnemySpawnData)
    {
        enemySpawnDataList = newEnemySpawnData;

        spawnCooldownTimers.Clear();
        spawnCooldowns.Clear();
        for (int i = 0; i < enemySpawnDataList.Count; i++)
        {
            EnemySpawnData enemySpawnData = enemySpawnDataList[i];
            spawnCooldownTimers.Add(0.0f);
            spawnCooldowns.Add(enemySpawnData.GetRandomSpawnCooldown());
        }
    }

    public void DestroyAllEnemies()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }
}

[Serializable]
public struct EnemySpawnData
{
    public GameObject prefab;
    public float spawnrateMin;
    public float spawnrateMax;
    public int maxEnemies;
    public float attackSpeed;

    public float GetRandomSpawnCooldown()
    {
        return UnityEngine.Random.Range(spawnrateMin, spawnrateMax);
    }
}
