using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class EnemyManager : SingletonMonoBehaviour<EnemyManager>
{
  public Enemy enemyPrefab;
  public Enemy Bat01Prefab;

  Dictionary<int, IObjectPool<Enemy>> enemyPools = new Dictionary<int, IObjectPool<Enemy>>();

  List<Enemy> enemies = new List<Enemy>();

  protected override void MyAwake()
  {
    MyEnum.ForEach<EnemyId>((id) => 
    {
      Debug.Log($"Create EnemyPool id = {id}");

      var enemyPool = new LinkedPool<Enemy>(
        () => Instantiate(GetEnemyPrefab(id)),
        e => e.gameObject.SetActive(true),
        e => e.gameObject.SetActive(false),
        e => Destroy(e.gameObject)
      );

      enemyPools.Add((int)id, enemyPool);
    });
  }
  private Enemy GetEnemyPrefab(EnemyId id)
  {
    switch(id) {
      case EnemyId.Enemy: return enemyPrefab;
      case EnemyId.Bat01: return Bat01Prefab;
      default:
        Debug.LogError($"{id} Prefab isn't found.");
        return null;
    }
  }

  public Enemy Get(EnemyId enemyId)
  {
    if (!enemyPools.TryGetValue((int)enemyId, out var enemyPool)) {
      Debug.Log($"[EnemyManager.Get] EnemyPools[id] is null.");
      return null;
    }

    var e = enemyPool.Get();
    e.Init(enemyId);
    enemies.Add(e);
    return e;
  }

  public void Release(Enemy enemy)
  {
    if(!enemyPools.TryGetValue((int)enemy.Id, out var enemyPool)) {
      Debug.Log($"");
      return;
    }

    enemyPool.Release(enemy);
    enemies.Remove(enemy);
  }

  public void Clear()
  {
    enemies.ForEach(e => enemyPools[(int)e.Id].Release(e));
    enemies.Clear();
  }

  public void Scan(Func<Enemy, bool> func)
  {
    for (int i = 0, count = enemies.Count; i < count; i++) 
    {
      // funcの戻り値がfalseだったらそこでscan終了 
      if (func(enemies[i]) == false) { 
        break; 
      }
    }
  }

  /// <summary>
  /// 指定した座標のもっとも近くにいる敵を探す
  /// </summary>
  public Enemy FindNearestEnemy(Vector3 position)
  {
    // 敵がいなければ当然ながらnullを返す
    if (enemies.Count == 0) {
      return null;
    }

    // もっとも近い距離、最初はfloatの最大値を設定しておく
    float nearestDistance = float.MaxValue;

    // もっとも近い敵のインスタンス、最初はnullを設定しておく
    Enemy nearestEnemy = null;

    // 全ての敵の中からもっとも近い敵を探す
    foreach (Enemy enemy in enemies) 
    {
      var distance = (enemy.transform.position - position).sqrMagnitude;

      // 現時点で一番近い距離よりも近くだったら、その距離と敵を保持
      if (distance < nearestDistance) {
        nearestDistance = distance;
        nearestEnemy = enemy;
      }
    }

    // 最後に残ったのが一番近い敵
    return nearestEnemy;
  }
}
