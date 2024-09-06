using UnityEngine;

static public class App
{
  public const int ACTIVE_SKILL_MAX = 10;
  public const int SKILL_MAX_LEVEL = 10;
}

static public class Layer
{
  public static int PlayerAttack => LayerMask.NameToLayer("PlayerAttack");
  public static int EnemyAttack => LayerMask.NameToLayer("EnemyAttack");
}

public enum EnemyId
{
  Undefined = 0,
  BatA, // コウモリ
  SpiderA, // クモ
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

/// <summary>
/// 成長度
/// </summary>
public enum Growth
{
  Normal, // 普通
  Slow,   // 遅い
  Fast,   // 速い
}