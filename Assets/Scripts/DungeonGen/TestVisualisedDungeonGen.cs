using System.Collections;
using UnityEngine;
using static DungeonGenController;

public class TestVisualisedDungeonGen : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private int dungeonWidth = 50;
    [SerializeField] private int dungeonHeight = 50;
    [SerializeField] private int corridorLength = 20;
    [SerializeField] private int corridorWidth = 2;
    [SerializeField] private int rooms = 2;
    [SerializeField] private bool pathLeadsToExit;
    [SerializeField] private Vector2 maxRoomSize = new(3, 3);
    [SerializeField] private Vector2 minRoomSize = new(2, 2);
    [SerializeField] private Vector2 startPoint = new(0, 1);

    [Header("Refrences")]
    [SerializeField] private GameObject floorPrefab;
    [SerializeField] private GameObject wallPrefab;
    [SerializeField] private DungeonGenController controller;

    private void Awake()
    {
        controller.OnGenerate += ControllerOnGenerate;
    }

    private void ControllerOnGenerate(DungeonGenType type)
    {
        if (type is DungeonGenType.MineVisualized)
            StartCoroutine(Generate());
    }

    private IEnumerator Generate()
    {
        bool[,] data = new bool[dungeonWidth, dungeonHeight];
        Vector2 currentPos = startPoint;
        data[(int)currentPos.x, (int)currentPos.y] = true;
        DrawGrid(data);

        for (int i = 0; i <= corridorLength; i++)
        {
            yield return new WaitForSeconds(1f);

            currentPos = TryToSpawnPath(currentPos, data);
            data[(int)currentPos.x, (int)currentPos.y] = true;
            DrawGrid(data);
        }
    }

    private Vector2 TryToSpawnPath(Vector2 currentPos, bool[,] data)
    {
        Vector2 nextPos;
        int attempts = 0;
        do
        {
            nextPos = currentPos + RandomDirection();
            attempts++;
        } while ((!IsWithinBounds(nextPos) || data[(int)nextPos.x, (int)nextPos.y]) && attempts < 10);

        return nextPos;
    }

    private bool IsWithinBounds(Vector2 pos)
    {
        return pos.x > 0 && pos.x < dungeonWidth - 1 && pos.y > 0 && pos.y < dungeonHeight - 1;
    }

    private Vector2 RandomDirection()
    {
        int choice = Random.Range(0, 4);
        return choice switch
        {
            0 => Vector2.up,
            1 => Vector2.down,
            2 => Vector2.left,
            _ => Vector2.right,
        };
    }

    private void DrawGrid(bool[,] data)
    {
        // Clear previous objects
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        for (int x = 0; x < dungeonWidth; x++)
        {
            for (int y = 0; y < dungeonHeight; y++)
            {
                Vector3 position = new Vector3(x, 0, y);
                Instantiate(data[x, y] ? floorPrefab : wallPrefab, position, Quaternion.identity, transform);
            }
        }
    }
}
