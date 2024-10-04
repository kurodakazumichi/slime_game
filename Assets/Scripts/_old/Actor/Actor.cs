using UnityEngine;

public interface IActor : IMyMonoBehaviour
{
  /// <summary>
  /// ダメージを受ける
  /// </summary>
  DamageInfo TakeDamage(AttackInfo info);
}