using UnityEngine;
using static DungeonGenController;

public class DungeonGenerationCellularAutomata : MonoBehaviour
{
    [SerializeField] private int width = 20;
    [SerializeField] private int height = 20;
    [SerializeField] private int fillProbability = 45;
    [SerializeField] private int iterations = 5;
    [SerializeField] private GameObject floorPrefab;
    [SerializeField] private GameObject wallPrefab;
    [SerializeField] private DungeonGenController controller;

    private void Awake()
    {
        controller.OnGenerate += ControllerOnGenerate;
    }

    private void ControllerOnGenerate(DungeonGenType type)
    {
        if (type is DungeonGenType.CellularAutomata)
            Generate();
    }

    public void Generate()
    {
        bool[,] grid = new bool[width, height];
        InitializeGrid(grid);
        for (int i = 0; i < iterations; i++)
        {
            grid = Simulate(grid);
        }
        DrawGrid(grid);
    }

    void InitializeGrid(bool[,] grid)
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                grid[x, y] = Random.Range(0, 100) < fillProbability;
            }
        }
    }

    bool[,] Simulate(bool[,] oldGrid)
    {
        bool[,] newGrid = new bool[width, height];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                int wallCount = CountWalls(oldGrid, x, y);
                newGrid[x, y] = wallCount > 4;
            }
        }
        return newGrid;
    }

    int CountWalls(bool[,] grid, int x, int y)
    {
        int count = 0;
        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                int nx = x + i;
                int ny = y + j;
                if (nx >= 0 && nx < width && ny >= 0 && ny < height)
                {
                    if (i != 0 || j != 0)
                    {
                        if (grid[nx, ny]) count++;
                    }
                }
                else
                {
                    count++;
                }
            }
        }
        return count;
    }

    void DrawGrid(bool[,] grid)
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3 position = new Vector3(x, 0, y);
                Instantiate(grid[x, y] ? wallPrefab : floorPrefab, position, Quaternion.identity);
            }
        }
    }
}