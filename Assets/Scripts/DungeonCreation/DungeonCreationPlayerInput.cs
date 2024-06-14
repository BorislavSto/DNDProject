using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class DungeonCreationPlayerInput : MonoBehaviour
{
    private const int TERRAIN_SIZE = 3;

    [SerializeField] private GameObject previewObjectTest;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private DungeonCreationManager creationManager;

    private Vector3 previewVectorCache;
    private Camera mainCamera;

    private void Awake()
    {
        InputEventHandler.OnPlayerLeftClick += DungeonCreatorInputLeftClick;
        mainCamera = Camera.main;
    }

    private void OnDestroy()
    {
        InputEventHandler.OnPlayerLeftClick -= DungeonCreatorInputLeftClick;
    }

    private void Update()
    {
        Vector3 vector3 = GetMouseWorldPosition();

        if (previewVectorCache == vector3 || IsPointerOverUI() || !IsMouseInGameWindow() || !CursorManager.Instance.mouseFocus)
            return;

        previewVectorCache = vector3;
        Debug.Log(vector3);
        previewObjectTest.transform.position = SetTerrainTransform(vector3);
    }

    private bool IsPointerOverUI()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }

    private bool IsMouseInGameWindow()
    {
        Vector3 mousePosition = Input.mousePosition;
        return mousePosition.x >= 0 && mousePosition.x <= Screen.width &&
               mousePosition.y >= 0 && mousePosition.y <= Screen.height;
    }

    private void DungeonCreatorInputLeftClick()
    {
        Debug.LogWarning("try to spawn?");
        if (IsPointerOverUI() || !IsMouseInGameWindow() || !CursorManager.Instance.mouseFocus)
            return;

        Vector3 spawnVector = previewVectorCache + new Vector3(0, 2, 0);
        GameObject selectedPrefab = creationManager.selectedPrefab;
        Debug.LogWarning("Spawningsmth?" + selectedPrefab);
        if (selectedPrefab != null)
        {
            switch (creationManager.currentMode)
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

    private void PlaceTerrain(Vector3 position, GameObject prefab)
    {
        Vector3 terrainPosition = SetTerrainTransform(position);
        Instantiate(prefab, terrainPosition, Quaternion.identity);
        //creationManager.placeTerrainKV.Add(new Vector3IntWithInt(SetTerrainTransform(terrainPosition), 1));
    }

    private void PlaceObject(Vector3 position, GameObject prefab)
    {
        // Implement object-specific placement logic here
        Instantiate(prefab, position, Quaternion.identity);
        // Optionally add to a different collection if needed
    }

    private void PlaceItem(Vector3 position, GameObject prefab)
    {
        // Implement item-specific placement logic here
        Instantiate(prefab, position, Quaternion.identity);
        // Optionally add to a different collection if needed
    }

    private Vector3 SetTerrainTransform(Vector3 vector3)
    {
        float x = Mathf.Floor(vector3.x / TERRAIN_SIZE) * TERRAIN_SIZE;
        float z = Mathf.Floor(vector3.z / TERRAIN_SIZE) * TERRAIN_SIZE;

        x = Mathf.Floor(x / TERRAIN_SIZE) * TERRAIN_SIZE;
        z = Mathf.Floor(z / TERRAIN_SIZE) * TERRAIN_SIZE;

        return new Vector3(x, 0, z);
    }

    Vector3 GetMouseWorldPosition()
    {
        Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, layerMask))
        {
            return hit.point;
        }
        return Vector3.zero;
    }
}

public class DungeonCreationUI : MonoBehaviour
{
    public Dropdown modeDropdown;
    public Dropdown typeDropdown;
    public Button confirmButton;

    public DungeonCreationManager dungeonManager;

    void Start()
    {
        // Initialize the mode dropdown
        modeDropdown.ClearOptions();
        modeDropdown.AddOptions(new List<string> { "Terrain", "Object", "Item" });
        modeDropdown.onValueChanged.AddListener(OnModeChanged);

        // Initialize the type dropdown
        typeDropdown.ClearOptions();

        // Initialize the confirm button
        confirmButton.onClick.AddListener(OnConfirm);

        // Set the initial mode
        dungeonManager.currentMode = Mode.Terrain;
        UpdateTypeDropdown();
    }

    void OnModeChanged(int index)
    {
        dungeonManager.currentMode = (Mode)index;
        UpdateTypeDropdown();
    }

    void UpdateTypeDropdown()
    {
        typeDropdown.ClearOptions();
        //typeDropdown.AddOptions(dungeonManager.GetTypesForCurrentMode());
    }

    void OnConfirm()
    {
        string selectedType = typeDropdown.options[typeDropdown.value].text;
        Debug.Log($"Selected Mode: {dungeonManager.currentMode}, Selected Type: {selectedType}");
        // Here you can call a method in DungeonManager to handle the selection
    }
    // TODO : for a creation tool we need a manager which tracks whats been picked to be placed down,
    // the input and mouse position which track where and when to place
    // and perhaps more im not thinking of now
}
