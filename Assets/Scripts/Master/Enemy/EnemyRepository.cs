using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace MyGame.Master
{
  public static class EnemyRepository
  {
    /// <summary>
    /// 敵の全データを格納しているリスト
    /// </summary>
    public static List<IEnemyEntity> entities = new();

    /// <summary>
    /// 敵の全データを同期ロード
    /// </summary>
    public static void Load()
    {
      MyEnum.ForEach<EnemyId>(id => 
      {
        if (id == EnemyId.Undefined) return;

        var entity = LoadEntity(id);
        entity.Init();
        entities.Add(entity);
      });
    }

    /// <summary>
    /// EnemyIdに対応するMasterデータをロードする
    /// </summary>
    private static EnemyEntity LoadEntity(EnemyId id)
    {
      var path = $"Master/Enemy/{id.ToString()}.asset";
      var handle = Addressables.LoadAssetAsync<EnemyEntity>(path);
      handle.WaitForCompletion();
      return handle.Result;
    }
  }
}