using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody playerRigidbody;
    [SerializeField] private float moveSpeed = 40f;
    [SerializeField] private InputManager inputManager;

    [SerializeField] private float xMin = -70f;
    [SerializeField] private float xMax = 70f;
    [SerializeField] private float yMin = 0f;
    [SerializeField] private float yMax = 50f;
    [SerializeField] private float fixedZ = 0f;

    private GameManager gameManager;

    private void Start()
    {
        inputManager.OnMove.AddListener(MovePlayer);
        gameManager = FindFirstObjectByType<GameManager>();
        if (gameManager == null)
            Debug.LogError("GameManager not found in the scene!");

        Vector3 startPos = transform.position;
        startPos.x = Mathf.Clamp(startPos.x, xMin, xMax);
        startPos.y = Mathf.Clamp(startPos.y, yMin, yMax);
        startPos.z = fixedZ;
        transform.position = startPos;
    }

    private void MovePlayer(Vector2 inputVector)
    {
        Vector3 moveDirection = new Vector3(inputVector.x, inputVector.y, 0).normalized;
        float currentSpeed = moveSpeed * (gameManager != null ? gameManager.SpeedMultiplier : 1f);
        playerRigidbody.linearVelocity = moveDirection * currentSpeed;
    }

    private void FixedUpdate()
    {
        Vector3 pos = playerRigidbody.position;
        pos.x = Mathf.Clamp(pos.x, xMin, xMax);
        pos.y = Mathf.Clamp(pos.y, yMin, yMax);
        pos.z = fixedZ;
        playerRigidbody.position = pos;
    }
}
