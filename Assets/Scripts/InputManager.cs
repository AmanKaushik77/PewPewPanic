using UnityEngine;
using UnityEngine.Events;

public class InputManager : MonoBehaviour
{
    public UnityEvent<Vector2> OnMove = new UnityEvent<Vector2>();
    public UnityEvent OnJump = new UnityEvent();

    [SerializeField] private Transform shipTransform;
    [SerializeField] private float tiltAngle = 30f;
    [SerializeField] private float tiltSpeed = 5f;
    [SerializeField] private bool normalizeInput = true;

    private Vector2 currentInput;
    private Quaternion targetRotation;

    void Update()
    {
        Vector2 inputVector = Vector2.zero;
        if (Input.GetKey(KeyCode.W)) inputVector += Vector2.up;
        if (Input.GetKey(KeyCode.S)) inputVector += Vector2.down;
        if (Input.GetKey(KeyCode.A)) inputVector += Vector2.left;
        if (Input.GetKey(KeyCode.D)) inputVector += Vector2.right;
        if (normalizeInput && inputVector.sqrMagnitude > 1f) inputVector = inputVector.normalized;
        currentInput = inputVector;
        OnMove?.Invoke(inputVector);
        if (shipTransform != null)
        {
            float tilt = -inputVector.x * tiltAngle;
            targetRotation = Quaternion.Euler(0, 0, tilt);
            shipTransform.rotation = Quaternion.Lerp(shipTransform.rotation, targetRotation, Time.deltaTime * tiltSpeed);
        }
        if (Input.GetKeyDown(KeyCode.Space)) OnJump?.Invoke();
    }
}
