using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private InputManager inputManager;
    [SerializeField] private Transform targetTransform;
    [SerializeField] private Transform cameraPivot;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private LayerMask collisionLayers;

    private float defaultPosition;
    private Vector3 cameraFollowVelocity = Vector3.zero;
    private Vector3 cameraVectorPosition = Vector3.zero;

    [SerializeField] private float cameraCollisionOffset = 0.2f;
    [SerializeField] private float minimumCollisionOffset = 0.2f;
    [SerializeField] private float cameraCollisionRadius = 0.2f;
    [SerializeField] private float cameraFollowSpeed = 0.2f;

    private void Awake()
    {
        inputManager = FindFirstObjectByType<InputManager>();

        PlayerController player = FindFirstObjectByType<PlayerController>();
        if (player != null)
        {
            targetTransform = player.transform;
        }

        cameraTransform = Camera.main.transform;
        defaultPosition = cameraTransform.localPosition.z;
    }

    private void LateUpdate()
    {
        HandleAllCameraMovement();
    }

    private void HandleAllCameraMovement()
    {
        FollowTarget();
        HandleCameraCollisions();
    }

    private void FollowTarget()
    {
        if (targetTransform == null) return;

        Vector3 targetPosition = Vector3.SmoothDamp(
            transform.position, targetTransform.position, ref cameraFollowVelocity, cameraFollowSpeed);

        transform.position = targetPosition;
    }

    private void HandleCameraCollisions()
    {
        float targetPosition = defaultPosition;
        RaycastHit hit;
        Vector3 direction = (cameraTransform.position - cameraPivot.position).normalized;

        if (Physics.SphereCast(
                cameraPivot.position, cameraCollisionRadius, direction, out hit, Mathf.Abs(targetPosition), collisionLayers))
        {
            float distance = Vector3.Distance(cameraPivot.position, hit.point);
            targetPosition = distance - cameraCollisionOffset;
        }

        targetPosition = Mathf.Max(targetPosition, -minimumCollisionOffset);
        cameraVectorPosition.z = Mathf.Lerp(cameraTransform.localPosition.z, targetPosition, Time.deltaTime * 10f);
        cameraTransform.localPosition = cameraVectorPosition;
    }
}
