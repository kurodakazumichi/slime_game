using System.Collections.Generic;

public static class EnemyRepository
{
  public static List<IEnemyEntityRO> entities = new List<IEnemyEntityRO>() 
  {
    new EnemyEntity() {
      Id           = EnemyId.BatA,
      Name         = "コウモリ",
      HP           = 2,
      Power        = 1,
      AttackAttr   = (uint)(Attribute.Non),
      WeakAttr     = (uint)(Attribute.Nil),
      ResistAttr   = (uint)(Attribute.Nil),
      NullfiedAttr = (uint)(Attribute.Nil),
      SkillId      = SkillId.NormalBullet,
      Exp          = 1,
      PrefabPath   = "Enemy/BatA.prefab",
      IconPath     = "Icon/Enemy/BatA.png",
    },
    
    new EnemyEntity() {
      Id           = EnemyId.Enemy011,
      Name         = "クモ",
      HP           = 5,
      Power        = 2,
      AttackAttr   = (uint)(Attribute.Non),
      WeakAttr     = (uint)(Attribute.Nil),
      ResistAttr   = (uint)(Attribute.Nil),
      NullfiedAttr = (uint)(Attribute.Nil),
      SkillId      = SkillId.NormalBullet,
      Exp          = 2,
      PrefabPath   = "Enemy/011.prefab",
      IconPath     = "Icon/Enemy/SpiderA.png",
    },
    
  };
}