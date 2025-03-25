using UnityEngine;
using UnityEngine.Events;

public class InputManager : MonoBehaviour
{
    public UnityEvent<Vector2> OnMove = new UnityEvent<Vector2>();
    public UnityEvent OnJump = new UnityEvent();

    [SerializeField] private Transform shipTransform;      // Reference to the ship's transform
    [SerializeField] private float tiltAngle = 30f;        // Maximum tilt angle in degrees
    [SerializeField] private float tiltSpeed = 5f;         // How fast the ship tilts
    [SerializeField] private bool normalizeInput = true;   // Option to normalize diagonal movement

    private Vector2 currentInput;
    private Quaternion targetRotation;

    void Update()
    {
        Vector2 inputVector = Vector2.zero;

        // Movement input
        if (Input.GetKey(KeyCode.W))
        {
            inputVector += Vector2.up;
        }
        if (Input.GetKey(KeyCode.S))
        {
            inputVector += Vector2.down;
        }
        if (Input.GetKey(KeyCode.A))
        {
            inputVector += Vector2.left;
        }
        if (Input.GetKey(KeyCode.D))
        {
            inputVector += Vector2.right;
        }

        // Normalize input if enabled
        if (normalizeInput && inputVector.sqrMagnitude > 1f)
        {
            inputVector = inputVector.normalized;
        }

        // Store current input and invoke movement
        currentInput = inputVector;
        OnMove?.Invoke(inputVector);

        // Calculate and apply tilt
        if (shipTransform != null)
        {
            // Calculate tilt based on horizontal input
            float tilt = -inputVector.x * tiltAngle; // Negative because left (-x) tilts right (+z)

            // Set target rotation
            targetRotation = Quaternion.Euler(0, 0, tilt);

            // Smoothly rotate the ship
            shipTransform.rotation = Quaternion.Lerp(
                shipTransform.rotation,
                targetRotation,
                Time.deltaTime * tiltSpeed
            );
        }

        // Optional jump input
        if (Input.GetKeyDown(KeyCode.Space))
        {
            OnJump?.Invoke();
        }
    }
}