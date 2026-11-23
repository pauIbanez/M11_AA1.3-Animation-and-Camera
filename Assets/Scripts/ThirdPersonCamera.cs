using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
  public Transform target;
  public Vector3 shoulderOffset = new Vector3(0.5f, 0f, 0f);
  public float distance = 3f;
  public float sensitivity = 0.1f;
  public float minPitch = -40f;
  public float maxPitch = 70f;

  [Header("Collision")]
  public float collisionRadius = 0.2f;
  public LayerMask collisionMask;
  public float returnSpeed = 5f;

  InputSystem_Actions input;
  float yaw, pitch;
  float currentDistance;

  void Start()
  {
    input = new InputSystem_Actions();
    input.Enable();

    yaw = transform.eulerAngles.y;
    pitch = transform.eulerAngles.x;
    currentDistance = distance;

    Cursor.lockState = CursorLockMode.Locked;
  }

  void LateUpdate()
  {
    Vector2 look = input.Player.Look.ReadValue<Vector2>();
    yaw += look.x * sensitivity;
    pitch -= look.y * sensitivity;
    pitch = Mathf.Clamp(pitch, minPitch, maxPitch);
    Quaternion rot = Quaternion.Euler(pitch, yaw, 0f);

    Vector3 pivot = target.position + rot * shoulderOffset;
    Vector3 back = rot * Vector3.back;

    float wantedDistance = distance;
    if (Physics.SphereCast(pivot, collisionRadius, back, out RaycastHit hit, distance, collisionMask))
    {
      wantedDistance = hit.distance;
    }

    if (wantedDistance < currentDistance)
      currentDistance = wantedDistance;
    else
      currentDistance = Mathf.Lerp(currentDistance, wantedDistance, returnSpeed * Time.deltaTime);

    transform.position = pivot + back * currentDistance;
    transform.rotation = rot;
  }
}
