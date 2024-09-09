using System.Collections.Generic;

public static class EnemyRepository
{
  public static List<IEnemyEntityRO> entities = new List<IEnemyEntityRO>() 
  {
    new EnemyEntity() {
      Id           = EnemyId.BatA,
      No           = 0,
      Name         = "コウモリ",
      HP           = 2,
      Power        = 1,
      AttackAttr   = (uint)(Attribute.Non),
      WeakAttr     = (uint)(Attribute.Nil),
      ResistAttr   = (uint)(Attribute.Nil),
      NullfiedAttr = (uint)(Attribute.Nil),
      SkillId      = SkillId.NormalBullet1,
      Exp          = 1,
      PrefabPath   = "Enemy/000.prefab",
    },
    
    new EnemyEntity() {
      Id           = EnemyId.Enemy011,
      No           = 11,
      Name         = "ゾイダー",
      HP           = 5,
      Power        = 2,
      AttackAttr   = (uint)(Attribute.Non),
      WeakAttr     = (uint)(Attribute.Nil),
      ResistAttr   = (uint)(Attribute.Nil),
      NullfiedAttr = (uint)(Attribute.Nil),
      SkillId      = SkillId.NormalBullet1,
      Exp          = 2,
      PrefabPath   = "Enemy/011.prefab",
    },
    
    new EnemyEntity() {
      Id           = EnemyId.Enemy013,
      No           = 13,
      Name         = "ポルル",
      HP           = 3,
      Power        = 2,
      AttackAttr   = (uint)(Attribute.Non),
      WeakAttr     = (uint)(Attribute.Nil),
      ResistAttr   = (uint)(Attribute.Nil),
      NullfiedAttr = (uint)(Attribute.Nil),
      SkillId      = SkillId.PiercingBullet1,
      Exp          = 1,
      PrefabPath   = "Enemy/013.prefab",
    },
    
    new EnemyEntity() {
      Id           = EnemyId.Enemy016,
      No           = 16,
      Name         = "チュリス",
      HP           = 3,
      Power        = 1,
      AttackAttr   = (uint)(Attribute.Non),
      WeakAttr     = (uint)(Attribute.Nil),
      ResistAttr   = (uint)(Attribute.Nil),
      NullfiedAttr = (uint)(Attribute.Nil),
      SkillId      = SkillId.NormalBullet1,
      Exp          = 1,
      PrefabPath   = "Enemy/016.prefab",
    },
    
  };
}