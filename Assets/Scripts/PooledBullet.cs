using UnityEngine;

public class PooledBullet : MonoBehaviour
{
  public Projectile projectile;
  public float lifetime = 5f;

  void OnEnable()
  {
    projectile.gameObject.SetActive(true);
    Invoke(nameof(ReturnToPool), lifetime);
  }

  void OnDisable()
  {
    CancelInvoke();
  }

  void ReturnToPool()
  {
    BulletPool.Instance.Pool.Release(gameObject);
  }
}
