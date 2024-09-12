using System.Collections.Generic;

public static class EnemyRepository
{
  public static List<IEnemyEntityRO> entities = new List<IEnemyEntityRO>() 
  {
    new EnemyEntity() {
      Id           = EnemyId.Enemy011,
      No           = 11,
      Name         = "ゾイダー",
      HP           = 5,
      Power        = 2,
      Speed        = 1f,
      Mass         = 1f,
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
      Speed        = 1.5f,
      Mass         = 0.5f,
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
      Speed        = 0.5f,
      Mass         = 0.3f,
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