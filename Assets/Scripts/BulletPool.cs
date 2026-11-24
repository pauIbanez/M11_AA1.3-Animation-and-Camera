using UnityEngine;
using UnityEngine.Pool;

public class BulletPool : MonoBehaviour
{
  public static BulletPool Instance;

  public GameObject bulletPrefab;
  public int defaultCapacity = 50;
  public int maxSize = 200;

  public ObjectPool<GameObject> Pool { get; private set; }

  void Awake()
  {
    Instance = this;

    Pool = new ObjectPool<GameObject>(
        createFunc: CreateBullet,
        actionOnGet: null,
        actionOnRelease: bullet => bullet.SetActive(false),
        actionOnDestroy: bullet => Destroy(bullet),
        collectionCheck: true,
        defaultCapacity: defaultCapacity,
        maxSize: maxSize);
  }

  GameObject CreateBullet()
  {
    GameObject bullet = Instantiate(bulletPrefab, transform);
    bullet.SetActive(false);
    return bullet;
  }
}
