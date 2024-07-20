using System.Collections.Generic;
using UnityEngine;
using static DungeonGenController;

public class DungeonGenerationBSPDungeon : MonoBehaviour
{
    [SerializeField] private int width = 50;
    [SerializeField] private int height = 50;
    [SerializeField] private int minRoomSize = 6;
    [SerializeField] private GameObject roomPrefab;
    [SerializeField] private GameObject corridorPrefab;
    [SerializeField] private DungeonGenController controller;

    private void Awake()
    {
        controller.OnGenerate += ControllerOnGenerate;
    }

    private void ControllerOnGenerate(DungeonGenType type)
    {
        if (type is DungeonGenType.BinarySpacePartitioning)
            Generate();
    }

    private void Generate()
    {
        List<Rect> rooms = new List<Rect>();
        List<Rect> corridors = new List<Rect>();
        DivideSpace(new Rect(0, 0, width, height), rooms, corridors);
        DrawDungeon(rooms, corridors);
    }

    void DivideSpace(Rect space, List<Rect> rooms, List<Rect> corridors)
    {
        if (space.width > minRoomSize * 2 || space.height > minRoomSize * 2)
        {
            bool splitHorizontally = space.width > space.height;
            if (splitHorizontally)
            {
                int split = Random.Range(minRoomSize, (int)space.width - minRoomSize);
                Rect left = new Rect(space.x, space.y, split, space.height);
                Rect right = new Rect(space.x + split, space.y, space.width - split, space.height);
                corridors.Add(new Rect(space.x + split - 1, space.y, 2, space.height)); // Vertical corridor
                DivideSpace(left, rooms, corridors);
                DivideSpace(right, rooms, corridors);
            }
            else
            {
                int split = Random.Range(minRoomSize, (int)space.height - minRoomSize);
                Rect top = new Rect(space.x, space.y, space.width, split);
                Rect bottom = new Rect(space.x, space.y + split, space.width, space.height - split);
                corridors.Add(new Rect(space.x, space.y + split - 1, space.width, 2)); // Horizontal corridor
                DivideSpace(top, rooms, corridors);
                DivideSpace(bottom, rooms, corridors);
            }
        }
        else
        {
            rooms.Add(space);
        }
    }

    void DrawDungeon(List<Rect> rooms, List<Rect> corridors)
    {
        foreach (Rect room in rooms)
        {
            Vector3 position = new Vector3(room.x + room.width / 2, 0, room.y + room.height / 2);
            Vector3 scale = new Vector3(room.width, 1, room.height);
            GameObject roomObj = Instantiate(roomPrefab, position, Quaternion.identity);
            roomObj.transform.localScale = scale;
        }

        foreach (Rect corridor in corridors)
        {
            Vector3 position = new Vector3(corridor.x + corridor.width / 2, 0, corridor.y + corridor.height / 2);
            Vector3 scale = new Vector3(corridor.width, 1, corridor.height);
            GameObject corridorObj = Instantiate(corridorPrefab, position, Quaternion.identity);
            corridorObj.transform.localScale = scale;
        }
    }
}
