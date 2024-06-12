using UnityEngine;
using UnityEngine.InputSystem;

public class DungeonCreationPlayerInput : MonoBehaviour
{
    private const int TERRAIN_SIZE = 3;

    [SerializeField] private GameObject previewObjectTest;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private GameObject TESTETSE;
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

        if (previewVectorCache == vector3)
            return;

        previewVectorCache = vector3;
        Debug.Log(vector3);
        previewObjectTest.transform.position = SetTerrainTransform(vector3);
    }

    private void DungeonCreatorInputLeftClick()
    {
        Vector3 spawnVector = previewVectorCache + new Vector3(0, 2, 0);
        Instantiate(TESTETSE, SetTerrainTransform(spawnVector), Quaternion.identity);

        creationManager.placeTerrainKV.Add(new Vector3IntWithInt(SetTerrainTransform(spawnVector), 1));
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
    // TODO : for a creation tool we need a manager which tracks whats been picked to be placed down,
    // the input and mouse position which track where and when to place
    // and perhaps more im not thinking of now
}
