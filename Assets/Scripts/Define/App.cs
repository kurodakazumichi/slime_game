using UnityEngine;

static public class App
{
  public const int ACTIVE_SKILL_MAX = 10;
  public const int SKILL_MAX_LEVEL = 10;
  public const int ENEMY_MAX_LEVEL = 10;
}

static public class LayerName
{
  public static string PlayerBullet = "PlayerBullet";
  public static string EnemyBullet  = "EnemyBullet";
  public static string Enemy        = "Enemy";
}

static public class Layer
{
  public static int PlayerBullet => LayerMask.NameToLayer(LayerName.PlayerBullet);
  public static int EnemyBullet  => LayerMask.NameToLayer(LayerName.EnemyBullet);
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

public enum BulletOwner
{
  Player,
  Enemy,
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

/// <summary>
/// 属性
/// </summary>
public enum Attribute
{
  Nil = 0,      // 設定なし
  Non = 1 << 0, // 無
  Fir = 1 << 1, // 火
  Wat = 1 << 2, // 水
  Thu = 1 << 3, // 雷
  Ice = 1 << 4, // 氷
  Tre = 1 << 5, // 木
  Hol = 1 << 6, // 聖
  Dar = 1 << 7, // 闇
}