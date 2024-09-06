using UnityEngine;

public interface IActor
{
  /// <summary>
  /// ダメージを受ける
  /// </summary>
  void TakeDamage(AttackStatus p);

  /// <summary>
  /// GameObjectを持つ
  /// </summary>
  GameObject gameObject { get; }

  /// <summary>
  /// Transformを持つ
  /// </summary>
  Transform CachedTransform { get; }
}