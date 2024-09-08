﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

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

/// <summary>
/// 弾丸インターフェース
/// </summary>
public interface IBullet
{
  SkillId Id { get; }
  bool IsIdle { get; }

  void Fire(BulletFireInfo info);
  void Terminate();
  void Attack(IActor actor);

  void Intersect();
  GameObject gameObject { get; }
}

public class BulletManager : SingletonMonoBehaviour<BulletManager>
{
  /// <summary>
  /// Bulletのオブジェクトプール一式
  /// </summary>
  Dictionary<int, IObjectPool<GameObject>> bulletPools 
     = new Dictionary<int, IObjectPool<GameObject>>();

  private LinkedList<IBullet> bullets = new LinkedList<IBullet>();

  public int ActiveBulletCount {
    get { return bullets.Count; }
  }

  public void Load()
  {
    ResourceManager.Instance.Load<GameObject>("Bullet/NormalBullet.prefab");
  }

  private GameObject GetPrefab(SkillId id)
  {
    var entity = SkillMaster.FindById(id);

    if (entity is null) {
      Logger.Error($"[BulletManager.GetPrefab] Prefab of {id.ToString()} isn't found.");
      return null;
    }

    return ResourceManager.Instance.GetCache<GameObject>(entity.Prefab);
  }

  protected override void MyAwake()
  {
    MyEnum.ForEach<SkillId>((id) => 
    {
      Logger.Log($"[BulletManager.MyAwake] Create bulletPool id = {id.ToString()}");

      var pool = new LinkedPool<GameObject>(
        () => Instantiate(GetPrefab(id)),
        b => b.gameObject.SetActive(true),
        b => b.gameObject.SetActive(false),
        b => Destroy(b.gameObject)
      );

      bulletPools.Add((int)id, pool);
    });
  }

  public IBullet Get(SkillId id) 
  { 
    if (!bulletPools.TryGetValue((int)id, out var pool)) {
      Logger.Error($"[BulletManager.Get] bulletPools[{id.ToString()}] is null.");
      return null;
    }

    var bullet = pool.Get().GetComponent<IBullet>();
    bullets.AddFirst(bullet);
    return bullet;
  }

  public void Release(IBullet bullet)
  {
    if (!bulletPools.TryGetValue((int)bullet.Id, out var pool)) {
      Logger.Error($"[BulletManager.Release] EnemyPools[{bullet.Id.ToString()}] is null.");
      return;
    }

    pool.Release(bullet.gameObject);
    bullets.Remove(bullet);
  }

  public void Terminate()
  {
    foreach (var bullet in bullets)
    {
      bullet.Terminate();
    }
  }

#if _DEBUG
  //----------------------------------------------------------------------------
  // For Debug
  //----------------------------------------------------------------------------

  /// <summary>
  /// デバッグ用の基底メソッド
  /// </summary>
  public override void OnDebug()
  {
    GUILayout.Label("BulletManager");

    GUILayout.Label($"Active Count = {bullets.Count}");

    if (GUILayout.Button("Terminate")) {
      Terminate();
    }
  }

#endif
}
