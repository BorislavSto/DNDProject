using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class DungeonCreationPlayerInput : MonoBehaviour
{
    private const int TERRAIN_SIZE = 3;

    public static DungeonCreationPlayerInput Instance { get; private set; }

    [SerializeField] private DungeonGenBaseObject previewObjectTest;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private LayerMask mask;
    [SerializeField] private DungeonCreationManager creationManager;

    public Vector3 previewVectorCache; // TODO FIX
    private GameObject previewGameObject;
    private Camera mainCamera;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        mainCamera = Camera.main;
    }

    private void Update()
    {
        Vector3 vector3 = GetMouseWorldPosition();

        if (previewVectorCache == vector3 && creationManager.currentMode == Mode.None)
            PeviewObject(vector3, previewObjectTest);

        if (previewVectorCache == vector3 || IsPointerOverUI() || !IsMouseInGameWindow() || !CursorManager.Instance.mouseFocus)
            return;

        previewVectorCache = vector3;

        switch (creationManager.currentMode)
        {
            case Mode.None:
                PeviewObject(vector3, previewObjectTest);
                break;

            case Mode.Terrain:
                PeviewObject(SetTerrainTransform(vector3), creationManager.selectedObject);
                break;

            case Mode.Object:
                PeviewObject(vector3, creationManager.selectedObject);
                break;

            case Mode.Item:
                PeviewObject(vector3, creationManager.selectedObject);
                break;

            default:
                Debug.LogError("Error in Creation Manager, Mode somehow out of range");
                break;
        }
    }

    public bool IsPointerOverUI()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }

    public bool IsMouseInGameWindow()
    {
        Vector3 mousePosition = Input.mousePosition;
        return mousePosition.x >= 0 && mousePosition.x <= Screen.width &&
               mousePosition.y >= 0 && mousePosition.y <= Screen.height;
    }

    private void PeviewObject(Vector3 pos, DungeonGenBaseObject gameObject)
    {
        GameObject previewObject = gameObject.ObjectPrefabPreview;
        Vector3 readyPos = GetSpawnPositionAboveGround(pos, previewObject);

        if (previewGameObject == null || !previewGameObject.activeSelf)
        {
            previewGameObject = Instantiate(previewObject, readyPos, Quaternion.identity);
        }
        else
        {
            if (previewGameObject != previewObject)
            {
                DestroyImmediate(previewGameObject, true);
                previewGameObject = null;
                PeviewObject(readyPos, gameObject);
            }
            previewGameObject.transform.position = readyPos;
        }
    }

    private Vector3 GetSpawnPositionAboveGround(Vector3 position, GameObject selectedPrefab)
    {
        RaycastHit hit;
        Vector3 rayStart = position + Vector3.up * selectedPrefab.GetComponent<Collider>().transform.localScale.y;
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

    Vector3 GetMouseWorldPosition()
    {
        Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
        int mask2 = ~mask;
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, mask2))
        {
            return hit.point;
        }
        return Vector3.zero;
    }
}
