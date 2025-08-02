using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Dialogue", menuName = "GameData/Dialogue")]
public class Dialogue : ScriptableObject
{
    public List<DialogueLine> lines;
}

[Serializable]
public struct DialogueLine
{
    [TextArea(1, 5)] public string text;
}
