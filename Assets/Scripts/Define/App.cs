using UnityEngine;

static public class App
{
  public const int ACTIVE_SKILL_MAX = 10;
}

public enum EnemyId
{
  SpiderA, // 最初にとりあえず作った蜘蛛
  BatA, // 適当に追加したコウモリ
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