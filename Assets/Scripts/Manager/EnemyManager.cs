using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

/// <summary>
/// 敵のインターフェース
/// </summary>
public interface IEnemy : IActor
{
  /// <summary>
  /// 識別子の取得
  /// </summary>
  EnemyId Id { get; }

  /// <summary>
  /// 速度
  /// </summary>
  Vector3 Velocity { get; }

  /// <summary>
  /// 初期化
  /// </summary>
  void Init(EnemyId id, int lv);

  /// <summary>
  /// 所属Waveを設定する
  /// </summary>
  void SetOwnerWave(EnemyWave wave);

  /// <summary>
  /// 活動を開始する
  /// </summary>
  void Run();

  /// <summary>
  /// 敵を殺す
  /// </summary>
  void Kill();
}

public class EnemyManager : SingletonMonoBehaviour<EnemyManager>
{
  //============================================================================
  // Variables
  //============================================================================

  /// <summary>
  /// オブジェクトプール、敵の種類ごとに管理
  /// </summary>
  Dictionary<int, IObjectPool<GameObject>> enemyPools = new Dictionary<int, IObjectPool<GameObject>>();

  /// <summary>
  /// 現在アクティブな敵のリスト
  /// </summary>
  List<IEnemy> enemies = new List<IEnemy>();

  //============================================================================
  // Methods
  //============================================================================

  //----------------------------------------------------------------------------
  // Public
  //----------------------------------------------------------------------------

  /// <summary>
  /// 暫定:全ての敵のPrefabリソースをロード
  /// </summary>
  public void Load()
  {
    MyEnum.ForEach<EnemyId>((id) => 
    {
      if (id != EnemyId.Undefined) {
        ResourceManager.Instance.Load<GameObject>(EnemyMaster.FindById(id).PrefabPath);
      }
    });
  }

  /// <summary>
  /// IDを元に敵オブジェクトを取得
  /// </summary>
  public IEnemy Get(EnemyId enemyId, int lv)
  {
    if (!enemyPools.TryGetValue((int)enemyId, out var enemyPool)) {
      Logger.Error($"[EnemyManager.Get] EnemyPools[{enemyId.ToString()}] is null.");
      return null;
    }

    var e = enemyPool.Get().GetComponent<IEnemy>();
    e.Init(enemyId, lv);
    enemies.Add(e);
    return e;
  }

  /// <summary>
  /// 敵をオブジェクトプールに戻す
  /// </summary>
  public void Release(IEnemy enemy)
  {
    if (!enemyPools.TryGetValue((int)enemy.Id, out var enemyPool)) {
      Logger.Error($"[EnemyManager.Release] EnemyPools[{enemy.Id.ToString()}] is null.");
      return;
    }

    enemyPool.Release(enemy.gameObject);
    enemies.Remove(enemy);
  }

  /// <summary>
  /// アクティブな敵を全てオブジェクトプールに戻す
  /// </summary>
  public void Clear()
  {
    enemies.ForEach(e => enemyPools[(int)e.Id].Release(e.gameObject));
    enemies.Clear();
  }

  /// <summary>
  /// アクティブな敵をスキャン
  /// </summary>
  /// <param name="func"></param>
  public void Scan(Func<IEnemy, bool> func)
  {
    for (int i = 0, count = enemies.Count; i < count; i++) {
      // funcの戻り値がfalseだったらそこでscan終了 
      if (func(enemies[i]) == false) {
        break;
      }
    }
  }

  /// <summary>
  /// 指定した座標のもっとも近くにいる敵を探す
  /// </summary>
  public IEnemy FindNearestEnemy(Vector3 position)
  {
    // 敵がいなければ当然ながらnullを返す
    if (enemies.Count == 0) {
      return null;
    }

    // もっとも近い距離、最初はfloatの最大値を設定しておく
    float nearestDistance = float.MaxValue;

    // もっとも近い敵のインスタンス、最初はnullを設定しておく
    IEnemy nearestEnemy = null;

    // 全ての敵の中からもっとも近い敵を探す
    foreach (IEnemy enemy in enemies) {
      var distance = (enemy.CachedTransform.position - position).sqrMagnitude;

      // 現時点で一番近い距離よりも近くだったら、その距離と敵を保持
      if (distance < nearestDistance) {
        nearestDistance = distance;
        nearestEnemy = enemy;
      }
    }

    // 最後に残ったのが一番近い敵
    return nearestEnemy;
  }

  /// <summary>
  /// 群れアルゴリズムにより進むべき方向を求める
  /// </summary>
  public Vector3 Boids(IEnemy main, float radius, Vector3 toTarget)
  {
    List<IEnemy> list = new();

    // 視界にいる別固体を集める
    foreach (var enemy in enemies) 
    {
      if (main == enemy) {
        continue;
      }

      if (CollisionUtil.IsCollideAxB(main.Position, enemy.Position, radius)) {
        list.Add(enemy);
      }
    }

    // 周りに個体がいなければ現状維持とする
    if (list.Count == 0) {
      return toTarget;
    }

    Vector3 v1;                // 分離、一番近い個体と反対方向のベクトル
    Vector3 v2 = Vector3.zero; // 整列、周囲の固体の速度の平均
    Vector3 v3 = Vector3.zero; // 結合、群れの中心に向かうベクトル

    // もっとも近い距離、最初はfloatの最大値を設定しておく
    float nearestDistance = float.MaxValue;

    // もっとも近い敵のインスタンス、最初はnullを設定しておく
    IEnemy nearestEnemy = null;

    foreach (var enemy in list) 
    {
      var distance = (main.Position - enemy.Position).sqrMagnitude;

      if (distance < nearestDistance) {
        nearestEnemy = enemy;
      }

      v2 += enemy.Velocity;
      v3 += enemy.Position;
    }

    // 分離ベクトル
    v1 = (main.Position - nearestEnemy.Position).normalized;

    // 整列ベクトル
    v2 /= list.Count;
    v2.Normalize();

    // 結合ベクトル
    v3 /= list.Count;
    v3.Normalize();

    var v = (toTarget*0.2f)+(v1*0.3f)+(v2*0.3f)+(v3*0.2f);
    
    return v.normalized;
  }


  //----------------------------------------------------------------------------
  // Life Cycle
  //----------------------------------------------------------------------------
  protected override void MyAwake()
  {
    base.MyAwake();

    MyEnum.ForEach<EnemyId>((id) => 
    {
      Logger.Log($"[EnemyManager.MyAwake] Create EnemyPool id = {id}");

      var enemyPool = new LinkedPool<GameObject>(
        () => Instantiate(GetEnemyPrefab(id)),
        e => e.gameObject.SetActive(true),
        e => e.gameObject.SetActive(false),
        e => Destroy(e.gameObject)
      );

      enemyPools.Add((int)id, enemyPool);
    });
  }

  //----------------------------------------------------------------------------
  // For Me
  //----------------------------------------------------------------------------
  /// <summary>
  /// 敵のPrefabを取得する。事前にロードを済ませておく必要がある。
  /// </summary>
  private GameObject GetEnemyPrefab(EnemyId id)
  {
    var entity = EnemyMaster.FindById(id);

    if (entity is null) {
      Logger.Error($"[EnemyManager.GetEnemyPrefab] Prefab of {id} isn't found.");
      return null;
    }

    return ResourceManager.Instance.GetCache<GameObject>(entity.PrefabPath);
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
    EnemyManagerDebugger.OnGUI();
  }

  /// <summary>
  /// EnemyManagerのdebugger
  /// </summary>
  public static class EnemyManagerDebugger
  {
    /// <summary>
    /// 入力用EnemyID
    /// </summary>
    private static string enemyId = string.Empty;
    private static int lv = 1;

    /// <summary>
    /// 描画
    /// </summary>
    public static void OnGUI()
    {
      using (new GUILayout.HorizontalScope()) {
        GUILayout.Label("EnemyID");
        enemyId = GUILayout.TextField(enemyId);

        GUILayout.Label($"Lv {lv}");
        lv = (int)GUILayout.HorizontalSlider(lv, 1f, 10f);

        if (GUILayout.Button("Make")) {
          var enemy = Instance.Get(MyEnum.Parse<EnemyId>(enemyId), lv);
          enemy.Run();
        }
      }

      if (GUILayout.Button("All Kill")) {
        foreach (var enemy in Instance.enemies) {
          enemy.Kill();
        }
      }
    }
  }
#endif
}
