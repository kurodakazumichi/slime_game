using System.Collections.Generic;
using UnityEngine;

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

        var dir    = "Enemy";
        var file   = id.ToString();
        var entity = MasterUtil.LoadEntity<EnemyEntity>(dir, file);
        entity.Init();
        entities.Add(entity);
      });
    }
  }
}