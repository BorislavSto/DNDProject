using UnityEngine;
using UnityEngine.InputSystem;

public class DungeonCreationPlayerInput : MonoBehaviour
{
    [SerializeField] private GameObject previewObjectTest;
    [SerializeField] private LayerMask layerMask;

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
        previewObjectTest.transform.position = vector3;
    }

    private void DungeonCreatorInputLeftClick(bool value)
    {

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
