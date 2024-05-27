using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform;
    private float moveSpeed = 5f;
    private float lookSpeed = 10f;

    private float yaw;
    private float pitch;

    #region Setup
    private void Awake()
    {
        EventHandler.OnMoveInput += Movement;
        EventHandler.OnLookInput += Looking;
    }

    private void OnDestroy()
    {
        EventHandler.OnMoveInput -= Movement;
        EventHandler.OnLookInput -= Looking;
    }
    #endregion

    private void Movement(Vector3 value)
    {
        this.transform.Translate(value * moveSpeed * Time.deltaTime);
    }

    private void Looking(Vector3 value)
    {
        pitch += value.x * lookSpeed * Time.deltaTime;
        yaw -= value.y * lookSpeed * Time.deltaTime;
        pitch = Mathf.Clamp(pitch, -90f, 90f);

        transform.rotation = Quaternion.Euler(0, yaw, 0);
        cameraTransform.localRotation = Quaternion.Euler(pitch, 0, 0);
    }
}
