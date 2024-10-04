using UnityEngine;

public static class App
{
  public const int ACTIVE_SKILL_MAX = 5;
  public const int SKILL_MAX_LEVEL = 10;
  public const int ENEMY_MAX_LEVEL = 10;
  public const float CAMERA_ANGLE_X = 45f;
  public const float BATTLE_CIRCLE_RADIUS = 10f;
  public static float BATTLE_CIRCLRE_AREA => Mathf.Pow(BATTLE_CIRCLE_RADIUS, 2) * Mathf.PI;
  public static Vector3 CAMERA_OFFSET = new Vector3(0, 14, -14);
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
/// EnemyWaveの役割
/// </summary>
public enum EnemyWaveRole
{
  None,
  Wait,
  Random,
  Circle,
  Forward,
  Backword,
  Left,
  Right,
}

/// <summary>
/// スキルのAim種別
/// </summary>
public enum SkillAimingType
{
  None,    // なし(ランダム方向)
  Nearest, // もっとも近い
  Weakest, // 弱点
  Random,  // ランダムな相手
  Player,  // プレイヤー
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
  Undefined, // 未定義
  Normal, // 普通
  Slow,   // 遅い
  Fast,   // 速い
}

public enum BattleResult
{
  Undefined, // 未定義
  Win,       // 勝利
  Lose,      // 敗北
}