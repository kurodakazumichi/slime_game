using UnityEngine;

/// <summary>
/// 弾丸発射時の情報
/// </summary>
public struct BulletFireInfo
{
  public Vector3     Position;  // 位置
  public Vector3     Direction; // 方向
  public IActor      Target;    // ターゲット
  public ISkill      Skill;     // 弾丸を生成したスキル
  public BulletOwner Owner;     // 弾の所有者
}