using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SPYderController : MonoBehaviour
{
    public float movementSpeed = 2.0f;
    public float rotationSpeed = 10f;

    private Vector3 moveDirection = Vector3.zero;
    private SPYderControls playerInput;
    private Rigidbody spiderRb;

    private void OnEnable()
    {
        playerInput.Enable();
    }

    private void Awake()
    {
        playerInput = new SPYderControls();
    }

    private void Start()
    {
        spiderRb = GetComponent<Rigidbody>();
        spiderRb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
    }

    private void Update()
    {
        Vector2 moveInput = playerInput.Player.Move.ReadValue<Vector2>();
        moveDirection = new Vector3(moveInput.x, 0, moveInput.y).normalized;
    }

    private void FixedUpdate()
    {
        MoveInsect();
    }

    private void MoveInsect()
    {
        if (moveDirection != Vector3.zero)
        {
            // Rotate towards movement direction
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);

            // Move using Rigidbody
            spiderRb.linearVelocity = new Vector3(moveDirection.x * movementSpeed, spiderRb.linearVelocity.y, moveDirection.z * movementSpeed);
        }
        else
        {
            spiderRb.linearVelocity = new Vector3(0, spiderRb.linearVelocity.y, 0);
        }
    }

    private void OnDisable()
    {
        playerInput.Disable();
    }
}
