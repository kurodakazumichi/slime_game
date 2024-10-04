using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ダメージ詳細
/// </summary>
public enum DamageDetail
{
  Undefined,               // 未定義
  NormalDamage,            // 通常ダメージ
  WeaknessDamage,          // 弱点ダメージ
  ResistanceDamage,        // 耐性ダメージ
  NullfiedDamage,          // 無効ダメージ
  NullfiedByInvincibility, // 無敵により無効
}

/// <summary>
/// ダメージ情報
/// </summary>
public struct DamageInfo
{
  //============================================================================
  // Properties
  //============================================================================
  /// <summary>
  /// ダメージ量
  /// </summary>
  public float Damage { get; set; }

  /// <summary>
  /// ダメージ詳細
  /// </summary>
  public DamageDetail Detail { get; set; }

  /// <summary>
  /// ヒットフラグ
  /// </summary>
  public bool IsHit => Detail != DamageDetail.NullfiedByInvincibility;

  /// <summary>
  /// ダメージがある
  /// </summary>
  public bool HasDamage => 0 < Damage;

  //============================================================================
  // Methods
  //============================================================================

  //----------------------------------------------------------------------------
  // Constractor
  //----------------------------------------------------------------------------

  public DamageInfo(float damage, DamageDetail detail = DamageDetail.NormalDamage)
  {
    this.Damage = damage;
    this.Detail = detail;
  }

}
