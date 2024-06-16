using UnityEngine;
using UnityEngine.InputSystem;

public class MyPlayerInput : MonoBehaviour
{
    private InputSystem_Actions inputActions = null;
    private Vector3 moveInput;
    private Vector3 lookInput;

    private void OnEnable()
    {
        Debug.Log("Input Activated");
        inputActions = new InputSystem_Actions();
        inputActions.Enable();

        TogglePlayerInput(true);
        ToggleUIInput(true);
    }

    private void OnDisable()
    {
        TogglePlayerInput(false);
        ToggleUIInput(false);

        inputActions.Disable();
        Debug.Log("Input Deactivated");
    }

    private void Update()
    {
        if (moveInput != Vector3.zero)
            InputEventHandler.InvokeOnMoveInput(moveInput);

        if (lookInput != Vector3.zero)
            InputEventHandler.InvokeOnLookInput(lookInput);
    }

    private void TogglePlayerInput(bool value)
    {
        if (value)
        {
            inputActions.Player.Move.performed += OnPlayerInputMove;
            inputActions.Player.Move.canceled += OnPlayerInputMove;
            inputActions.Player.Look.performed += OnPlayerInputLook;
            inputActions.Player.Look.canceled += OnPlayerInputLook;
            inputActions.Player.Interact.started += OnPlayerInputInteract;
        }
        else
        {
            inputActions.Player.Move.performed -= OnPlayerInputMove;
            inputActions.Player.Move.canceled -= OnPlayerInputMove;
            inputActions.Player.Look.performed -= OnPlayerInputLook;
            inputActions.Player.Look.canceled -= OnPlayerInputLook;
            inputActions.Player.Interact.started -= OnPlayerInputInteract;
        }
    }

    private void ToggleUIInput(bool value)
    {
        Debug.Log("toggle ui input" + value);
        if (value)
        {
            inputActions.UI.Click.performed += OnPlayerInputLeftClickUI;
            inputActions.UI.Escape.performed += OnPlayerInputEscapeUI;
        }
        else
        {
            inputActions.UI.Click.performed -= OnPlayerInputLeftClickUI;
            inputActions.UI.Escape.performed -= OnPlayerInputEscapeUI;
        }
    }

    public void OnPlayerInputInteract(InputAction.CallbackContext context)
    {
        InputEventHandler.InvokeOnAnyInput();
    }

    public void OnPlayerInputMove(InputAction.CallbackContext context)
    {
        Vector2 inputVector = context.ReadValue<Vector2>();
        moveInput = new Vector3(inputVector.x, 0, inputVector.y);
    }

    public void OnPlayerInputLook(InputAction.CallbackContext context)
    {
        Vector2 inputVector = context.ReadValue<Vector2>();
        lookInput = new Vector3(-inputVector.y, -inputVector.x, 0);
    }

    private void OnPlayerInputLeftClickUI(InputAction.CallbackContext context)
    {
        if (context.ReadValue<float>() == 1) // to prevent double clicking, performed seems to trigger on click down and click off
            InputEventHandler.InvokeOnPlayerLeftClickUI();
    }

    private void OnPlayerInputEscapeUI(InputAction.CallbackContext context)
    {
        InputEventHandler.InvokeOnPlayerEscapeUI();
    }
}
