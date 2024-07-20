using UnityEngine;
using static DungeonGenController;

public class DungeonGenerationRandomWalk : MonoBehaviour
{
    [SerializeField] private int width = 10;
    [SerializeField] private int height = 10;
    [SerializeField] private int walkLength = 20;
    [SerializeField] private GameObject floorPrefab;
    [SerializeField] private GameObject wallPrefab;
    [SerializeField] private DungeonGenController controller;

    private void Awake()
    {
        controller.OnGenerate += ControllerOnGenerate;
    }

    private void ControllerOnGenerate(DungeonGenType type)
    {
        if (type is DungeonGenType.RandomWalk)
            Generate();
    }

    private void Generate()
    {
        bool[,] grid = new bool[width, height];
        Vector2Int position = new Vector2Int(width / 2, height / 2);

        for (int i = 0; i < walkLength; i++)
        {
            grid[position.x, position.y] = true;
            position += RandomDirection();
            position.x = Mathf.Clamp(position.x, 0, width - 1);
            position.y = Mathf.Clamp(position.y, 0, height - 1);
        }

        DrawGrid(grid);
    }

    Vector2Int RandomDirection()
    {
        int choice = UnityEngine.Random.Range(0, 4);
        return choice switch
        {
            0 => Vector2Int.up,
            1 => Vector2Int.down,
            2 => Vector2Int.left,
            _ => Vector2Int.right,
        };
    }

    void DrawGrid(bool[,] grid)
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3 position = new Vector3(x, 0, y);
                Instantiate(grid[x, y] ? floorPrefab : wallPrefab, position, Quaternion.identity);
            }
        }
    }
}
