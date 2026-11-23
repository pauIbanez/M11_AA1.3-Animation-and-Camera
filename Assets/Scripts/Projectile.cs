using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
  public float disappearTime = 5f;
  public Vector3 forceMin = new Vector3(-1, -1, 50);
  public Vector3 forceMax = new Vector3(1, 1, 100);
  public LayerMask layers;
  public float collisionForceMultiplier = 2f;
  public float radius = .1f;
  [HideInInspector]
  public Rigidbody rb;

  Vector3 lastPos;

  public GameObject spawnOnCollide;
  public float holeLifetime = 10f;


  void Awake()
  {
    rb = GetComponent<Rigidbody>();
  }

  void OnEnable()
  {
    transform.localPosition = Vector3.zero;
    transform.localRotation = Quaternion.identity;
    rb.linearVelocity = Vector3.zero;
    rb.angularVelocity = Vector3.zero;

    Vector3 force = new Vector3(
        Random.Range(forceMin.x, forceMax.x),
        Random.Range(forceMin.y, forceMax.y),
        Random.Range(forceMin.z, forceMax.z));
    rb.AddForce(transform.TransformDirection(force));

    lastPos = transform.position;
    Invoke(nameof(Disappear), disappearTime);
  }

  void OnDisable()
  {
    CancelInvoke();
  }

  void Disappear()
  {
    gameObject.SetActive(false);
  }

  private void FixedUpdate()
  {
    Vector3 dir = transform.position - lastPos;

    Debug.DrawRay(lastPos, dir, Color.blue, disappearTime);

    RaycastHit hit;

    if (Physics.SphereCast(lastPos, radius, dir, out hit, dir.magnitude, layers))
    {
      Hitted(hit);
    }

    lastPos = transform.position;
  }

  void Hitted(RaycastHit hit)
  {
    if (hit.rigidbody)
    {
      hit.rigidbody.AddForceAtPosition(rb.linearVelocity * rb.mass * collisionForceMultiplier, this.transform.position);
    }

    GameObject hole = Instantiate(spawnOnCollide, hit.point, Quaternion.LookRotation(hit.normal));
    hole.transform.SetParent(hit.transform, true);
    Destroy(hole, holeLifetime);
    Disappear();
  }
}
