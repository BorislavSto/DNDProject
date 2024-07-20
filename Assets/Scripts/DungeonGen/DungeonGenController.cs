using System;
using UnityEngine;

public class DungeonGenController : MonoBehaviour
{
    public enum DungeonGenType
    {
        RandomWalk,
        CellularAutomata,
        BinarySpacePartitioning,
        MazeGeneration,
        PerlinNoise,
        Mine,
        MineVisualized,
    }

    [SerializeField] private DungeonGenType type;
    [SerializeField] private bool GenerateOnStart;
    public event Action<DungeonGenType> OnGenerate;

    private void Start()
    {
        if (!GenerateOnStart)
            return;

        switch (type)
        {
            case DungeonGenType.RandomWalk:
            case DungeonGenType.CellularAutomata:
            case DungeonGenType.BinarySpacePartitioning:
            case DungeonGenType.MazeGeneration:
            case DungeonGenType.PerlinNoise:
            case DungeonGenType.Mine:
            case DungeonGenType.MineVisualized:
                OnGenerate?.Invoke(type);
                break;
            default:
                Debug.LogError("DungeonGenType Type not Implemented");
                break;
        }
    }
}
