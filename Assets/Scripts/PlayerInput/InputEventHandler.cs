using System;
using UnityEngine;

public static class InputEventHandler
{
    public static event Action OnInteractInput;
    public static event Action<Vector3> OnMoveInput;
    public static event Action<Vector3> OnLookInput;
    public static event Action<bool> OnPlayerInInteractZone;
    public static event Action OnPlayerLeftClickUI;
    public static event Action OnPlayerEscapeUI;

    public static void InvokeOnAnyInput()
    {
        OnInteractInput?.Invoke();
    }

    public static void InvokeOnMoveInput(Vector3 value)
    {
        OnMoveInput?.Invoke(value);
    }

    public static void InvokeOnLookInput(Vector3 value)
    {
        OnLookInput?.Invoke(value);
    }

    public static void InvokeOnPlayerInInteractZone(bool value)
    {
        OnPlayerInInteractZone?.Invoke(value);
    }

    public static void InvokeOnPlayerLeftClickUI()
    {
        OnPlayerLeftClickUI?.Invoke();
    }

    public static void InvokeOnPlayerEscapeUI()
    {
        OnPlayerEscapeUI?.Invoke();
    }
}
