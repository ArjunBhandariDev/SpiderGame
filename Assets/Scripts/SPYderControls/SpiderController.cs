using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;  // Ensure you are using the New Input System

[RequireComponent(typeof(Rigidbody))]
public class SpiderController : MonoBehaviour
{
    public float speed = 3f;
    public float rotationSpeed = 10f;

    private Rigidbody _rigidbody;
    public CinemachineCamera mainCamera;
    private Vector2 moveInput;  // Stores movement input from New Input System
    private SPYderControls playerControls;  // Reference to Input System

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        playerControls = new SPYderControls();  // Initialize input system
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    private void Start()
    {
        // Assign the main camera (ensure Cinemachine is tagged as "MainCamera")

        // Freeze rotation to prevent unwanted physics tilting
        _rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
    }

    private void Update()
    {
        // Read movement input from the Input System
        moveInput = playerControls.Player.Move.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        MoveSpider();
    }

    private void MoveSpider()
    {
        // If no movement input, stop applying force
        if (moveInput.sqrMagnitude < 0.01f)
        {
            _rigidbody.linearVelocity = new Vector3(0, _rigidbody.linearVelocity.y, 0);
            return;
        }

        // Get camera forward & right vectors (ignoring Y-axis to prevent unwanted vertical movement)
        Vector3 camForward = mainCamera.transform.forward;
        Vector3 camRight = mainCamera.transform.right;
        camForward.y = 0;
        camRight.y = 0;
        camForward.Normalize();
        camRight.Normalize();

        // Compute movement direction relative to camera
        Vector3 moveDirection = (camForward * moveInput.y + camRight * moveInput.x).normalized;

        // Rotate spider to face movement direction smoothly
        if (moveDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
        }

        // Apply movement using Rigidbody velocity (no AddForce for direct control)
        _rigidbody.linearVelocity = new Vector3(moveDirection.x * speed, _rigidbody.linearVelocity.y, moveDirection.z * speed);
    }
}
