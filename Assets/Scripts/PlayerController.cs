using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody playerRigidbody;
    [SerializeField] private float moveSpeed = 40f;
    [SerializeField] private InputManager inputManager;

    private void Start()
    {
        inputManager.OnMove.AddListener(MovePlayer);
    }

    private void MovePlayer(Vector2 inputVector)
    {
        Vector3 moveDirection = new Vector3(inputVector.x, inputVector.y, 0);
        playerRigidbody.linearVelocity = moveDirection * moveSpeed;
    }
}