using UnityEngine;
using UnityEngine.Pool;

public class BulletManager : SingletonMonoBehaviour<BulletManager>
{
  public GameObject bulletPrefab;

  IObjectPool<Bullet> bulletPool;

  private void Start()
  {
    bulletPool = new LinkedPool<Bullet>(
      () => Instantiate(bulletPrefab).GetComponent<Bullet>(),
      b => b.gameObject.SetActive(true) ,
      b => b.gameObject.SetActive(false),
      b => Destroy(b.gameObject)
    );
  }

  public Bullet Get() { 
    return bulletPool.Get(); 
  }

  public void Release(Bullet bullet)
  {
    bulletPool.Release(bullet);
  }
}
