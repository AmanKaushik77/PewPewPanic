using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody playerRigidbody;
    [SerializeField] private float moveSpeed = 40f;
    [SerializeField] private InputManager inputManager;

    private GameManager gameManager;

    private void Start()
    {
        inputManager.OnMove.AddListener(MovePlayer);

        // Get GameManager reference
        gameManager = FindFirstObjectByType<GameManager>();
        if (gameManager == null)
        {
            Debug.LogError("GameManager not found in the scene!");
        }
    }

    private void MovePlayer(Vector2 inputVector)
    {
        Vector3 moveDirection = new Vector3(inputVector.x, inputVector.y, 0).normalized; // Normalize to maintain consistent speed
        float currentSpeed = moveSpeed * (gameManager != null ? gameManager.SpeedMultiplier : 1f);
        playerRigidbody.linearVelocity = moveDirection * currentSpeed;
    }
}