using UnityEngine;

public class CollisionManager : SingletonMonoBehaviour<CollisionManager>
{
  private const float serachRadius = 10f;

  /// <summary>
  /// プレイヤーの攻撃が敵に衝突する
  /// </summary>
  public void CollidePlayerAttackWithEnemy()
  {
    var pm = PlayerManager.Instance;

    if (!pm.PlayerExists) 
    {
      return;
    }

    var bullets = Physics.OverlapSphere(
      pm.PlayerOriginPosition, 
      serachRadius, 
      LayerMask.GetMask(LayerName.PlayerBullet)
    );

    foreach (var item in bullets) 
    {
      var bullet = item.GetComponent<IBullet>();

      if (bullet == null) continue;

      var enemies = Physics.OverlapSphere(
        bullet.CachedTransform.position, 
        bullet.collider.radius, 
        LayerMask.GetMask(LayerName.Enemy)
      );

      foreach (var enemy in enemies)
      {
        var actor = enemy.GetComponent<Actor>();
        bullet.Attack(actor);
      }
    }
  }
}
