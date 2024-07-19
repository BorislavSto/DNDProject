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
                OnGenerate?.Invoke(DungeonGenType.RandomWalk);
                break;
            case DungeonGenType.CellularAutomata:
                OnGenerate?.Invoke(DungeonGenType.CellularAutomata);
                break;
            case DungeonGenType.BinarySpacePartitioning:
                OnGenerate?.Invoke(DungeonGenType.BinarySpacePartitioning);
                break;
            case DungeonGenType.MazeGeneration:
                OnGenerate?.Invoke(DungeonGenType.MazeGeneration);
                break;
            case DungeonGenType.PerlinNoise:
                OnGenerate?.Invoke(DungeonGenType.PerlinNoise);
                break;
            default:
                Debug.LogError("DungeonGenType Type not Implemented");
                break;
        }
    }
}
