using UnityEngine;
using UnityEngine.Pool;

public class BulletManager : SingletonMonoBehaviour<BulletManager>
{
  public GameObject bulletPrefab;

  IObjectPool<StandardBullet> bulletPool;

  private void Start()
  {
    bulletPool = new LinkedPool<StandardBullet>(
      () => Instantiate(bulletPrefab).GetComponent<StandardBullet>(),
      b => b.gameObject.SetActive(true) ,
      b => b.gameObject.SetActive(false),
      b => Destroy(b.gameObject)
    );
  }

  public StandardBullet Get() { 
    return bulletPool.Get(); 
  }

  public void Release(StandardBullet bullet)
  {
    bulletPool.Release(bullet);
  }
}
