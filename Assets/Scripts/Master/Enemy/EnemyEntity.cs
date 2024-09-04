using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ReadOnly��EnemyEntity�C���^�[�t�F�[�X
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
  /// ����
  /// </summary>
  public string Name { get; set; }

  /// <summary>
  /// HP
  /// </summary>
  public float HP { get; set; }

  /// <summary>
  /// �́A�G�̍U���͂Ɏg����
  /// </summary>
  public float Power { get; set; }

  /// <summary>
  /// �U������
  /// </summary>
  public uint AttackAttr { get; set; }

  /// <summary>
  /// ��_����
  /// </summary>
  public uint WeakAttr { get; set; }

  /// <summary>
  /// ����������
  /// </summary>
  public uint NullfiedAttr { get; set; }

  /// <summary>
  /// �G�̎��X�L��ID
  /// </summary>
  public SkillId SkillId { get; set; }

  /// <summary>
  /// �G��|�������ɓ�����o���l
  /// </summary>
  public int Exp { get; set; }

  /// <summary>
  /// �G��Prefab������p�X
  /// </summary>
  public string PrefabPath { get; set; }

  /// <summary>
  /// �G�̃A�C�R���摜������p�X
  /// </summary>
  public string IconPath { get; set; }
}