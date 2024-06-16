using System;
using System.Collections.Generic;
using UnityEngine;

public class DungeonCreationManager : MonoBehaviour
{
    private const int TERRAIN_SIZE = 3;

    public static DungeonCreationManager Instance { get; private set; }

    [SerializeField] public Mode currentMode; // TODO FIX accecibility
    [SerializeField] private List<DungeonGenBaseObject> terrainTypes;
    [SerializeField] private List<DungeonGenBaseObject> objectTypes;
    [SerializeField] private List<DungeonGenBaseObject> itemTypes;
    [SerializeField] public LayerMask layerMask;

    public event Action<Mode> OnModeChanged;

    public DungeonGenBaseObject selectedObject;
    public GameObject selectedPrefab;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        InputEventHandler.OnPlayerLeftClickUI += DungeonCreatorInputLeftClick;
        InputEventHandler.OnPlayerEscapeUI += DungeonCreatorEscape;
    }

    private void OnDestroy()
    {
        InputEventHandler.OnPlayerLeftClickUI -= DungeonCreatorInputLeftClick;
        InputEventHandler.OnPlayerEscapeUI -= DungeonCreatorEscape;
    }

    public List<string> GetNamesForCurrentMode()
    {
        switch (currentMode)
        {
            case Mode.Terrain:
                return GetNames(terrainTypes);
            case Mode.Object:
                return GetNames(objectTypes);
            case Mode.Item:
                return GetNames(itemTypes);
            default:
                return new List<string>();
        }
    }

    public GameObject GetPrefabByName(string name)
    {
        switch (currentMode)
        {
            case Mode.Terrain:
                return GetPrefabByName(terrainTypes, name);
            case Mode.Object:
                return GetPrefabByName(objectTypes, name);
            case Mode.Item:
                return GetPrefabByName(itemTypes, name);
            default:
                return null;
        }
    }

    public DungeonGenBaseObject GetObjectByName(string name)
    {
        switch (currentMode)
        {
            case Mode.Terrain:
                return GetObjectByName(terrainTypes, name);
            case Mode.Object:
                return GetObjectByName(objectTypes, name);
            case Mode.Item:
                return GetObjectByName(itemTypes, name);
            default:
                return null;
        }
    }

    public void SetSelectedPrefab(string name)
    {
        selectedPrefab = GetPrefabByName(name);
        selectedObject = GetObjectByName(name);
    }

    public void SetCurrentMode(Mode mode)
    {
        currentMode = mode;
        OnModeChanged?.Invoke(mode);
    }

    public void SetSelectedObject(DungeonGenBaseObject genBaseObject)
    {
        selectedObject = genBaseObject;
    }

    private List<string> GetNames(List<DungeonGenBaseObject> objects)
    {
        List<string> names = new List<string>();
        foreach (DungeonGenBaseObject obj in objects)
        {
            names.Add(obj.ObjectName);
        }
        return names;
    }

    private GameObject GetPrefabByName(List<DungeonGenBaseObject> objects, string name)
    {
        foreach (DungeonGenBaseObject obj in objects)
        {
            if (obj.ObjectName == name)
            {
                return obj.ObjectPrefab;
            }
        }
        return null;
    }

    private DungeonGenBaseObject GetObjectByName(List<DungeonGenBaseObject> objects, string name)
    {
        foreach (DungeonGenBaseObject obj in objects)
        {
            if (obj.ObjectName == name)
            {
                return obj;
            }
        }
        return null;
    }

    private void DungeonCreatorInputLeftClick()
    {
        Debug.LogWarning("try to spawn?");
        if (DungeonCreationPlayerInput.Instance.IsPointerOverUI() || !DungeonCreationPlayerInput.Instance.IsMouseInGameWindow() || !CursorManager.Instance.mouseFocus)
            return;

        Vector3 spawnVector = DungeonCreationPlayerInput.Instance.previewVectorCache;// + new Vector3(0, 2, 0);

        Debug.LogWarning("Spawningsmth?" + selectedPrefab);
        if (selectedPrefab != null)
        {
            switch (currentMode)
            {
                case Mode.Terrain:
                    PlaceTerrain(spawnVector, selectedPrefab);
                    break;
                case Mode.Object:
                    PlaceObject(spawnVector, selectedPrefab);
                    break;
                case Mode.Item:
                    PlaceItem(spawnVector, selectedPrefab);
                    break;
                default:
                    Debug.LogError("Invalid mode!");
                    break;
            }
        }
    }

    private void DungeonCreatorEscape()
    {
        SetCurrentMode(Mode.None);
    }

    private void PlaceTerrain(Vector3 position, GameObject prefab)
    {
        Vector3 terrainPosition = SetTerrainTransform(position);
        Instantiate(prefab, terrainPosition, Quaternion.identity);
        //creationManager.placeTerrainKV.Add(new Vector3IntWithInt(SetTerrainTransform(terrainPosition), 1));
    }

    private void PlaceObject(Vector3 position, GameObject prefab)
    {
        Vector3 readyPos = GetSpawnPositionAboveGround(position, prefab);
        Instantiate(prefab, readyPos, Quaternion.identity);
        // Optionally add to a different collection if needed
    }

    private void PlaceItem(Vector3 position, GameObject prefab)
    {
        Vector3 readyPos = GetSpawnPositionAboveGround(position, prefab);
        Instantiate(prefab, readyPos, Quaternion.identity);
        // Optionally add to a different collection if needed
    }

    private Vector3 GetSpawnPositionAboveGround(Vector3 position, GameObject selectedPrefab)
    {
        RaycastHit hit;
        Vector3 rayStart = position + Vector3.up * selectedPrefab.GetComponent<Collider>().transform.localScale.y; // Start the ray above the position
        int layerMask = (1 << 6) | (1 << 7);

        if (Physics.Raycast(rayStart, Vector3.down, out hit, Mathf.Infinity, layerMask))
        {
            Vector3 i = new Vector3(0, selectedPrefab.GetComponent<Collider>().transform.localScale.y / 2, 0);
            Vector3 vector3 = hit.point + i;
            return vector3;
        }
        else
        {
            Debug.LogError("hit layermask" + hit.collider.gameObject.layer);
        }

        return rayStart;
    }

    private Vector3 SetTerrainTransform(Vector3 vector3)
    {
        float x = Mathf.Floor(vector3.x / TERRAIN_SIZE) * TERRAIN_SIZE;
        float z = Mathf.Floor(vector3.z / TERRAIN_SIZE) * TERRAIN_SIZE;

        x = Mathf.Floor(x / TERRAIN_SIZE) * TERRAIN_SIZE;
        z = Mathf.Floor(z / TERRAIN_SIZE) * TERRAIN_SIZE;

        return new Vector3(x, vector3.y, z);
    }
}
