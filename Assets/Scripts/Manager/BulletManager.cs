using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class BulletManager : SingletonMonoBehaviour<BulletManager>
{
  public GameObject bulletPrefab;

  IObjectPool<GameObject> bulletPool;

  private LinkedList<IBullet> bullets = new LinkedList<IBullet>();

  private void Start()
  {
    bulletPool = new LinkedPool<GameObject>(
      () => Instantiate(bulletPrefab),
      b => b.gameObject.SetActive(true) ,
      b => b.gameObject.SetActive(false),
      b => Destroy(b.gameObject)
    );
  }

  public IBullet Get() 
  { 
    var bullet = bulletPool.Get().GetComponent<IBullet>();
    bullets.AddFirst(bullet);
    return bullet;
    
  }

  public void Release(IBullet bullet)
  {
    bullets.Remove(bullet);
    bulletPool.Release(bullet.gameObject);
  }

  public void Terminate()
  {
    foreach (var bullet in bullets)
    {
      bullet.Terminate();
    }
  }

#if _DEBUG
  //----------------------------------------------------------------------------
  // For Debug
  //----------------------------------------------------------------------------

  /// <summary>
  /// デバッグ用の基底メソッド
  /// </summary>
  public override void OnDebug()
  {
    GUILayout.Label("BulletManager");

    GUILayout.Label($"Active Count = {bullets.Count}");

    if (GUILayout.Button("Terminate")) {
      Terminate();
    }
  }

#endif
}
