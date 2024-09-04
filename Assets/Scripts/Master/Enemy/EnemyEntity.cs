using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ReadOnlyのEnemyEntityインターフェース
/// </summary>
public interface IEnemyEntityRO
{
  EnemyId Id { get; }
  string Name { get; }
  float HP { get; }
  float Power { get; }
  uint AttackAttr { get; }
  uint WeakAttr { get; }
  uint NullfiedAttr { get; }
  SkillId SkillId { get; }
  int Exp { get; }
  string PrefabPath { get; }
  string IconPath { get; }
}

public class EnemyEntity : IEnemyEntityRO 
{
  /// <summary>
  /// Enemy ID
  /// </summary>
  public EnemyId Id { get; set; }

  /// <summary>
  /// 名称
  /// </summary>
  public string Name { get; set; }

  /// <summary>
  /// HP
  /// </summary>
  public float HP { get; set; }

  /// <summary>
  /// 力、敵の攻撃力に使われる
  /// </summary>
  public float Power { get; set; }

  /// <summary>
  /// 攻撃属性
  /// </summary>
  public uint AttackAttr { get; set; }

  /// <summary>
  /// 弱点属性
  /// </summary>
  public uint WeakAttr { get; set; }

  /// <summary>
  /// 無効化属性
  /// </summary>
  public uint NullfiedAttr { get; set; }

  /// <summary>
  /// 敵の持つスキルID
  /// </summary>
  public SkillId SkillId { get; set; }

  /// <summary>
  /// 敵を倒した時に得られる経験値
  /// </summary>
  public int Exp { get; set; }

  /// <summary>
  /// 敵のPrefabがあるパス
  /// </summary>
  public string PrefabPath { get; set; }

  /// <summary>
  /// 敵のアイコン画像があるパス
  /// </summary>
  public string IconPath { get; set; }
}