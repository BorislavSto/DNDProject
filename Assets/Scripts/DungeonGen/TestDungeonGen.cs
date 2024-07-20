using UnityEngine;
using static DungeonGenController;

public class TestDungeonGen : MonoBehaviour
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
        if (type is DungeonGenType.Mine)
            Generate();
    }

    private void Generate()
    {
        bool[,] acceptedSimulation = SimulatePath((int)startPoint.x, (int)startPoint.y);

        DrawGrid(acceptedSimulation);
    }

    private bool[,] SimulatePath(int startX, int startY)
    {
        // data values with false are walls, so initally the whole dungeon is just walls
        bool[,] data = new bool[dungeonWidth, dungeonHeight];

        Vector2 currentPos = new(startX, startY);

        data[(int)currentPos.x, (int)currentPos.y] = true;

        int currentLength = 0;
        int roomsToSpawn = rooms;

        for (int i = 0; i <= corridorLength; i++)
        {
            currentLength++;

            currentPos = TryToSpawnPath(currentPos);
            data[(int)currentPos.x, (int)currentPos.y] = true; // true for path

            if (currentLength > maxRoomSize.x && currentLength > maxRoomSize.y && roomsToSpawn > 0)
            {
                TryToSpawnRoom(currentPos, data);
                currentLength = 0;
                roomsToSpawn--;
            }
        }

        return data;
    }

    private Vector2 TryToSpawnPath(Vector2 currentPos)
    {
        Vector2 nextPos;
        int attempts = 0;

        do
        {
            nextPos = currentPos + RandomDirection();
            attempts++;
        } while (!IsWithinBounds(nextPos) && attempts < 10);

        return nextPos;
    }

    private void TryToSpawnRoom(Vector2 startPos, bool[,] data)
    {
        int roomWidth = Random.Range((int)minRoomSize.x, (int)maxRoomSize.x + 1);
        int roomHeight = Random.Range((int)minRoomSize.y, (int)maxRoomSize.y + 1);

        for (int i = 0; i < roomWidth; i++)
        {
            for (int j = 0; j < roomHeight; j++)
            {
                int roomX = (int)startPos.x + i;
                int roomY = (int)startPos.y + j;

                if (IsWithinBounds(new Vector2(roomX, roomY)))
                {
                    data[roomX, roomY] = true;
                }
            }
        }
    }

    private bool IsWithinBounds(Vector2 pos)
    {
        return pos.x > 0 && pos.x < dungeonWidth - 1 && pos.y > 0 && pos.y < dungeonHeight - 1;
    }

    Vector2 RandomDirection()
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
        for (int x = 0; x < dungeonWidth; x++)
        {
            for (int y = 0; y < dungeonHeight; y++)
            {
                Vector3 position = new Vector3(x, 0, y);
                Instantiate(data[x, y] ? floorPrefab : wallPrefab, position, Quaternion.identity);
            }
        }
    }
}
