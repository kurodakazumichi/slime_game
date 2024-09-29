using MyGame.Master;
using System.Collections.Generic;

public static class SkillRepository
{
  public static List<ISkillEntity> entities = new();

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