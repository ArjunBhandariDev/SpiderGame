using System.Collections;
using UnityEngine;

public class SPYderController : MonoBehaviour
{
    public IKInsectController insectController;

    public float movementSpeed = 2.0f;
    public float rotationSpeed = 200f;

    private Vector3 moveDirection = Vector3.zero;
    private bool isMoving = false;
    private SPYderControls playerInput;

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
        StartCoroutine(MoveCoroutine());
    }

    private void Update()
    {
        Vector2 moveInput = playerInput.Player.Move.ReadValue<Vector2>();

        moveDirection = new Vector3(moveInput.x, 0, moveInput.y).normalized;

        isMoving = moveDirection.magnitude > 0;
    }

    private IEnumerator MoveCoroutine()
    {
        while (true)
        {
            if (isMoving)
            {
                MoveInsect();
            }
            else
            {
                insectController.AgentMove(Vector3.zero);
            }
            yield return new WaitForSeconds(0.02f);
        }
    }

    private void MoveInsect()
    {
        if (moveDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
        insectController.AgentMove(moveDirection * movementSpeed * Time.deltaTime);
    }

    private void OnDisable()
    {
        playerInput.Disable();
    }
}
