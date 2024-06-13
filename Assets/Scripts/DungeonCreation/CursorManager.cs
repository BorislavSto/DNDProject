using UnityEngine;

public class CursorManager : MonoBehaviour
{
    public static CursorManager Instance { get; private set; }
    public bool mouseFocus { get; private set; }

    void Awake()
    {
        // Ensure there's only one instance of CursorManager
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Update()
    {
        if (Application.isFocused)
        {
            if (!mouseFocus)
            {
                LockCursor();
                mouseFocus = true;
            }
        }
        else
        {
            if (mouseFocus)
            {
                UnlockCursor();
                mouseFocus = false;
            }
        }
    }

    void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }

    void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
