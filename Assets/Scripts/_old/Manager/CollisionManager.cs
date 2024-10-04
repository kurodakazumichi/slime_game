using UnityEngine;

public class CollisionManager : SingletonMonoBehaviour<CollisionManager>
{
  private const float serachRadius = 10f;

  /// <summary>
  /// プレイヤーの弾丸と敵の衝突
  /// </summary>
  public void CollidePlayerBulletWithEnemy()
  {
    var pm = PlayerManager.Instance;

    if (!pm.PlayerExists) 
    {
      return;
    }

    // Player付近にある弾丸を収集
    var colliders = Physics.OverlapSphere(
      pm.Position, 
      serachRadius, 
      LayerMask.GetMask(LayerName.PlayerBullet)
    );

    // 収集した弾丸の衝突判定を実行
    foreach (var collider in colliders) 
    {
      var bullet = collider.GetComponent<IBullet>();
      
      if (bullet is not null) {
        bullet.Intersect();
      }
    }
  }

  /// <summary>
  /// 敵の弾丸とプレイヤーの衝突
  /// </summary>
  public void CollideEnemyBulletWithPlayer()
  { 
    var pm = PlayerManager.Instance;

    if (!pm.PlayerExists) {
      return;
    }

    // Playerに衝突している弾丸を収集
    var colliders = Physics.OverlapSphere(
      pm.Position,
      pm.Collider.radius,
      LayerMask.GetMask(LayerName.EnemyBullet)
    );

    // 収集した弾丸の衝突判定を実行
    foreach (var collider in colliders) {
      var bullet = collider.GetComponent<IBullet>();

      if (bullet is not null) {
        bullet.Intersect();
      }
    }
  }
}
