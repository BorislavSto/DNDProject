using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class DungeonCreationPlayerInput : MonoBehaviour
{
    private const int TERRAIN_SIZE = 3;

    public static DungeonCreationPlayerInput Instance { get; private set; }

    [SerializeField] private DungeonGenBaseObject previewObjectTest;
    [SerializeField] private LayerMask layerMask;
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

        if (previewVectorCache == vector3 || IsPointerOverUI() || !IsMouseInGameWindow() || !CursorManager.Instance.mouseFocus)
            return;

        previewVectorCache = vector3;
        Debug.Log(vector3);

        if (creationManager.selectedObject == null)
            PeviewObject(SetTerrainTransform(vector3), previewObjectTest);
        else
            PeviewObject(SetTerrainTransform(vector3), creationManager.selectedObject);

        //previewObjectTest.transform.position = SetTerrainTransform(vector3);
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

    private void PeviewObject(Vector3 pos, DungeonGenBaseObject gameObject) // TODO : bug? is preview obj null, no its not
    {
        GameObject previewObject = gameObject.ObjectPrefab; /// all bugged to shit lmao

        if (previewObject.activeSelf) ///
        {
            Instantiate(previewObject, pos, Quaternion.identity); ///
            previewGameObject = previewObject;
        }
        else
        {
            previewGameObject = null;
            Destroy(previewObject);
        }
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
