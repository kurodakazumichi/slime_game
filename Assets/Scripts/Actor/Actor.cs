using UnityEngine;

public interface IMono
{
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
  Vector3 Position { get; set; }

  /// <summary>
  /// 有効化
  /// </summary>
  void SetActive(bool isActive);
}


public interface IActor : IMono
{
  /// <summary>
  /// ダメージを受ける
  /// </summary>
  DamageInfo TakeDamage(AttackInfo info);
}