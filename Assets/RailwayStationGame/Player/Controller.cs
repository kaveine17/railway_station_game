using UnityEngine;
using UnityEngine.InputSystem;

public class FlyCameraController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;

    [Header("Look")]
    [SerializeField] private float mouseSensitivity = 0.1f;

    private float _yaw;
    private float _pitch;

    private void Start()
    {
        // Инициализируем углы из текущего поворота
        Vector3 euler = transform.eulerAngles;
        _yaw = euler.y;
        _pitch = euler.x;
        if (_pitch > 180f) _pitch -= 360f; // чтобы диапазон был -180..180

        // Прячем курсор
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        HandleLook();
        HandleMovement();
    }

    private void HandleLook()
    {
        if (Mouse.current == null) return;

        Vector2 delta = Mouse.current.delta.ReadValue();
        _yaw += delta.x * mouseSensitivity;
        _pitch -= delta.y * mouseSensitivity;
        _pitch = Mathf.Clamp(_pitch, -89f, 89f);

        transform.rotation = Quaternion.Euler(_pitch, _yaw, 0f);
    }

    private void HandleMovement()
    {
        if (Keyboard.current == null) return;

        Vector3 input = Vector3.zero;

        if (Keyboard.current.wKey.isPressed) input += Vector3.forward;
        if (Keyboard.current.sKey.isPressed) input += Vector3.back;
        if (Keyboard.current.aKey.isPressed) input += Vector3.left;
        if (Keyboard.current.dKey.isPressed) input += Vector3.right;
        if (Keyboard.current.spaceKey.isPressed) input += Vector3.up;
        if (Keyboard.current.leftCtrlKey.isPressed) input += Vector3.down;

        if (input.sqrMagnitude > 0f)
        {
            // Движение относительно локальных осей камеры
            Vector3 move = transform.TransformDirection(input.normalized);
            transform.position += move * moveSpeed * Time.deltaTime;
        }
    }
}