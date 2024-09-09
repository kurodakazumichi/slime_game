using UnityEngine;

public interface IActor
{
  /// <summary>
  /// ダメージを受ける
  /// </summary>
  void TakeDamage(AttackInfo info);

  /// <summary>
  /// GameObjectを持つ
  /// </summary>
  GameObject gameObject { get; }

  /// <summary>
  /// Transformを持つ
  /// </summary>
  Transform CachedTransform { get; }

  /// <summary>
  /// 座標
  /// </summary>
  Vector3 Position { get; }
}