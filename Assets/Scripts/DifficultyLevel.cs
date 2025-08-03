using UnityEngine;

[CreateAssetMenu(fileName = "DifficultyLevel", menuName = "GameData/DifficultyLevel")]
public class DifficultyLevel : ScriptableObject
{
    [SerializeField] public float lapLength;
}
