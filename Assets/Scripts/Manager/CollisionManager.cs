using UnityEngine;

public class CollisionManager : SingletonMonoBehaviour<CollisionManager>
{
  private const float _serachRadius = 10f;

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

    var attacks = Physics.OverlapSphere(pm.PlayerOriginPosition, _serachRadius, LayerMask.GetMask("PlayerAttack"));

    foreach (var attack in attacks) 
    {
      var bullet = attack.GetComponent<IBullet>();

      if (bullet == null) continue;

      var enemies = Physics.OverlapSphere(bullet.CachedTransform.position, bullet.collider.radius, LayerMask.GetMask("Enemy"));

      foreach (var enemy in enemies)
      {
        var actor = enemy.GetComponent<Actor>();
        bullet.Attack(actor);
      }
    }
  }
}
