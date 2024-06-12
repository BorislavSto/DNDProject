using UnityEngine;

[RequireComponent(typeof(Collision))]
public class InteractTriggerArea : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            InputEventHandler.InvokeOnPlayerInInteractZone(true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            InputEventHandler.InvokeOnPlayerInInteractZone(false);
    }
}