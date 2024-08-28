using UnityEngine;

public enum SkillId
{
  NormalBullet, // 通常弾
}

public enum EnemyId
{
  Enemy, // 最初にとりあえず作った蜘蛛
  Bat01, // 適当に追加したコウモリ
}

/// <summary>
/// Waveの形状
/// </summary>
public enum WaveShape
{
  None,
  Point,
  Circle,
  Line,
  Random,
}