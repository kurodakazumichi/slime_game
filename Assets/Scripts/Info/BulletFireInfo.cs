using UnityEngine;

/// <summary>
/// 弾丸発射時の情報
/// </summary>
public class BulletFireInfo
{
  public Vector3 Position  = Vector3.zero;       // 位置
  public Vector3 Direction = Vector3.zero;       // 方向
  public IActor Target     = null;               // ターゲット
  public ISkill Skill      = null;               // 弾丸を生成したスキル
  public BulletOwner Owner = BulletOwner.Player; // 弾の所有者
}