using System.Collections.Generic;

namespace MyGame.Master
{
  public static class SkillRepository
  {
    /// <summary>
    /// 全データを格納しているリスト
    /// </summary>
    public static List<ISkillEntity> entities = new();

    /// <summary>
    /// 全データを同期ロード
    /// </summary>
    public static void Load()
    {
      MyEnum.ForEach<SkillId>(id => 
      {
        if (id == SkillId.Undefined) return;

        var dir    = "Skill";
        var file   = id.ToString();
        var entity = MasterUtil.LoadEntity<SkillEntity>(dir, file);
        entity.Init();
        entities.Add(entity);
      });
    }
  }
}