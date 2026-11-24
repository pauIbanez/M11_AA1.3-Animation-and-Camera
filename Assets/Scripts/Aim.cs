using UnityEngine;

public class Aim : MonoBehaviour
{
  public Transform firePoint;
  public RectTransform realAimUI;
  public Camera cam;
  public float maxDistance = 1000f;
  public LayerMask mask;

  Vector3 aimPoint;

  void Start()
  {
    if (cam == null)
      cam = Camera.main;
  }

  void LateUpdate()
  {

    if (Physics.Raycast(firePoint.position, firePoint.forward, out RaycastHit hit, maxDistance, mask))
      aimPoint = hit.point;
    else
      aimPoint = firePoint.position + firePoint.forward * maxDistance;


    Vector3 screenPos = cam.WorldToScreenPoint(aimPoint);


    bool visible = screenPos.z > 0f;
    realAimUI.gameObject.SetActive(visible);
    if (visible)
      realAimUI.position = screenPos;

    Debug.DrawLine(firePoint.position, aimPoint, Color.red);
  }
}
