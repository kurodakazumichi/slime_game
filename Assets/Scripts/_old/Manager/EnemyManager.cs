using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using MyGame.Master;
using MyGame.Core.System;

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
  /// 敵の持つスキルID
  /// </summary>
  SkillId SkillId { get; }

  /// <summary>
  /// 敵の持つ経験値
  /// </summary>
  int Exp { get; }

  /// <summary>
  /// 速度
  /// </summary>
  Vector3 Velocity { get; }

  /// <summary>
  /// 死亡時コールバック
  /// </summary>
  Action<IEnemy> OnDead { set; }

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

  /// <summary>
  /// {attributes}に弱点属性が含まれるならばtrue
  /// </summary>
  bool IsWeakness(uint attributes);
}

public class EnemyManager : SingletonMonoBehaviour<EnemyManager>
#if _DEBUG
  ,IDebugable
#endif
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
  LinkedList<IEnemy> enemies = new LinkedList<IEnemy>();

  //============================================================================
  // Properties
  //============================================================================

  /// <summary>
  /// 敵死亡時のコールバック、敵を取得した際にこの変数に設定されているActionを敵にセットする。
  /// </summary>
  public Action<IEnemy> OnDeadEnemy { private get; set; }

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
    e.OnDead = OnDeadEnemy;
    enemies.AddLast(e);
    
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

    enemy.OnDead = null;
    enemyPool.Release(enemy.gameObject);
    enemies.Remove(enemy);
  }

  /// <summary>
  /// アクティブな敵を全てオブジェクトプールに戻す
  /// </summary>
  public void Clear()
  {
    foreach (var e in enemies) {
      enemyPools[(int)e.Id].Release(e.gameObject);
    }
    enemies.Clear();
  }

  /// <summary>
  /// アクティブな敵をスキャン
  /// </summary>
  /// <param name="func"></param>
  public void Scan(Func<IEnemy, bool> func)
  {
    foreach(var e in enemies) {
      // funcの戻り値がfalseだったらそこでscan終了 
      if (func(e) == false) {
        break;
      }
    }
  }

  /// <summary>
  /// 指定した座標のもっとも近くにいる敵を探す
  /// </summary>
  public IEnemy FindNearest(Vector3 position)
  {
    return FindNearest(position, this.enemies);
  }

  /// <summary>
  /// 指定した座標のもっとも近くにいる敵を探す
  /// </summary>
  private IEnemy FindNearest(Vector3 position, LinkedList<IEnemy> enemies)
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

  public IEnemy FindRandom()
  {
    // 敵がいなければ当然ながらnullを返す
    if (enemies.Count == 0) {
      return null;
    }

    int count = UnityEngine.Random.Range(0, enemies.Count);

    foreach (var enemy in enemies) 
    {
      if (count == 0) {
        return enemy;
      }

      --count;
    }

    return null;
  }

  /// <summary>
  /// {attr}が弱点になる一番近い敵を探す。
  /// 指定した属性を弱点に持つ敵がいない場合は、単純に一番近い敵を探します。
  /// </summary>
  public IEnemy FindWeakness(Vector3 position, uint attr)
  {
    var list = new LinkedList<IEnemy>();

    // {attr}が弱点になる敵を収集する
    foreach (var enemy in enemies) 
    {
      if (enemy.IsWeakness(attr)) {
        list.AddLast(enemy);
      }
    }

    // 弱点属性をもつ敵がいなければ、全体から一番近いやつ
    if (list.Count == 0) {
      return FindNearest(position);
    } 
    
    // 弱点属性をもつ敵の中で一番近いやつ
    else {
      return FindNearest(position, list);
    }
  }

  /// <summary>
  /// 群れアルゴリズムにより進むべき方向を求める
  /// </summary>
  public Vector3 Boids(Vector3 to, IEnemy main, float radius)
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

    // 周りに2個体以上がいなければ現状維持とする
    if (list.Count == 0) {
      return to;
    }

    Vector3 v1 = Vector3.zero; // 分離、一番近い個体と反対方向のベクトル
    Vector3 v2 = Vector3.zero; // 整列、周囲の固体の速度の平均
    Vector3 v3 = Vector3.zero; // 結合、群れの中心に向かうベクトル

    foreach (var enemy in list) 
    {
      v1 += (main.Position - enemy.Position);
      v2 += enemy.Velocity;
      v3 += enemy.Position;
    }

    // 分離ベクトル
    v1 /= list.Count;

    // 整列ベクトル
    v2 /= list.Count;

    // 結合ベクトル
    v3 /= list.Count;
    v3 = v3 - main.Position;

    var v = (v1*0.7f)+(v2*0.1f)+(v3*0.2f);

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
