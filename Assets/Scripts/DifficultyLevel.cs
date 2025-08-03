using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DifficultyLevel", menuName = "GameData/DifficultyLevel")]
public class DifficultyLevel : ScriptableObject
{
    [SerializeField] public float lapLength;
    [SerializeField] public float loopGodFollowSpeed;
    [SerializeField] public List<GameObject> trackPrefabs;
    [SerializeField] public List<EnemySpawnData> enemySpawnData;

    [Serializable]
    public struct EnemySpawnData
    {
        public GameObject prefab;
        public float spawnrateMin;
        public float spawnrateMax;
        public float attackSpeed;
    }
}
