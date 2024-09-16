using System.Collections.Generic;

public static class EnemyRepository
{
  public static List<IEnemyEntityRO> entities = new List<IEnemyEntityRO>() 
  {
    new EnemyEntity() {
      Id           = EnemyId.Enemy000,
      No           = 0,
      Name         = "リーフィ",
      HP           = 30,
      Power        = 10,
      Speed        = 1f,
      Mass         = 5.0f,
      AttackAttr   = (uint)(Attribute.Lef),
      WeakAttr     = (uint)(Attribute.Fir),
      ResistAttr   = (uint)(Attribute.Win),
      NullfiedAttr = (uint)(Attribute.Nil),
      SkillId      = SkillId.LeafBullet1,
      Exp          = 10,
      PrefabPath   = "Enemy/000/000.prefab",
    },
    
    new EnemyEntity() {
      Id           = EnemyId.Enemy003,
      No           = 3,
      Name         = "フォルク",
      HP           = 30,
      Power        = 10,
      Speed        = 1f,
      Mass         = 3.0f,
      AttackAttr   = (uint)(Attribute.Fir),
      WeakAttr     = (uint)(Attribute.Wat),
      ResistAttr   = (uint)(Attribute.Fir),
      NullfiedAttr = (uint)(Attribute.Nil),
      SkillId      = SkillId.FireBullet1,
      Exp          = 10,
      PrefabPath   = "Enemy/003/003.prefab",
    },
    
    new EnemyEntity() {
      Id           = EnemyId.Enemy006,
      No           = 6,
      Name         = "クミュル",
      HP           = 30,
      Power        = 10,
      Speed        = 1f,
      Mass         = 3.0f,
      AttackAttr   = (uint)(Attribute.Wat),
      WeakAttr     = (uint)(Attribute.Thu),
      ResistAttr   = (uint)(Attribute.Wat),
      NullfiedAttr = (uint)(Attribute.Nil),
      SkillId      = SkillId.WaterBullet1,
      Exp          = 10,
      PrefabPath   = "Enemy/006/006.prefab",
    },
    
    new EnemyEntity() {
      Id           = EnemyId.Enemy011,
      No           = 11,
      Name         = "ゾイダー",
      HP           = 5,
      Power        = 2,
      Speed        = 1f,
      Mass         = 1f,
      AttackAttr   = (uint)(Attribute.Non),
      WeakAttr     = (uint)(Attribute.Fir),
      ResistAttr   = (uint)(Attribute.Lef),
      NullfiedAttr = (uint)(Attribute.Nil),
      SkillId      = SkillId.NormalBullet1,
      Exp          = 2,
      PrefabPath   = "Enemy/011/011.prefab",
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
      SkillId      = SkillId.WindBullet1,
      Exp          = 1,
      PrefabPath   = "Enemy/013/013.prefab",
    },
    
    new EnemyEntity() {
      Id           = EnemyId.Enemy014,
      No           = 14,
      Name         = "ポクルル",
      HP           = 10,
      Power        = 3,
      Speed        = 1f,
      Mass         = 2f,
      AttackAttr   = (uint)(Attribute.Non),
      WeakAttr     = (uint)(Attribute.Nil),
      ResistAttr   = (uint)(Attribute.Nil),
      NullfiedAttr = (uint)(Attribute.Nil),
      SkillId      = SkillId.PiercingBullet1,
      Exp          = 1,
      PrefabPath   = "Enemy/014/014.prefab",
    },
    
    new EnemyEntity() {
      Id           = EnemyId.Enemy016,
      No           = 16,
      Name         = "チュリス",
      HP           = 7,
      Power        = 2,
      Speed        = 1f,
      Mass         = 0.3f,
      AttackAttr   = (uint)(Attribute.Non),
      WeakAttr     = (uint)(Attribute.Nil),
      ResistAttr   = (uint)(Attribute.Nil),
      NullfiedAttr = (uint)(Attribute.Nil),
      SkillId      = SkillId.NormalBullet1,
      Exp          = 3,
      PrefabPath   = "Enemy/016/016.prefab",
    },
    
    new EnemyEntity() {
      Id           = EnemyId.Enemy020,
      No           = 20,
      Name         = "フラウ",
      HP           = 30,
      Power        = 10,
      Speed        = 1f,
      Mass         = 4.0f,
      AttackAttr   = (uint)(Attribute.Lef),
      WeakAttr     = (uint)(Attribute.Fir),
      ResistAttr   = (uint)(Attribute.Win),
      NullfiedAttr = (uint)(Attribute.Nil),
      SkillId      = SkillId.LeafBullet1,
      Exp          = 10,
      PrefabPath   = "Enemy/020/020.prefab",
    },
    
    new EnemyEntity() {
      Id           = EnemyId.Enemy022,
      No           = 22,
      Name         = "ヒザル",
      HP           = 25,
      Power        = 10,
      Speed        = 1f,
      Mass         = 7.0f,
      AttackAttr   = (uint)(Attribute.Fir),
      WeakAttr     = (uint)(Attribute.Wat),
      ResistAttr   = (uint)(Attribute.Fir),
      NullfiedAttr = (uint)(Attribute.Nil),
      SkillId      = SkillId.FireBullet1,
      Exp          = 10,
      PrefabPath   = "Enemy/022/022.prefab",
    },
    
    new EnemyEntity() {
      Id           = EnemyId.Enemy024,
      No           = 024,
      Name         = "ウル",
      HP           = 20,
      Power        = 10,
      Speed        = 1f,
      Mass         = 5.0f,
      AttackAttr   = (uint)(Attribute.Ice),
      WeakAttr     = (uint)(Attribute.Fir),
      ResistAttr   = (uint)(Attribute.Ice),
      NullfiedAttr = (uint)(Attribute.Nil),
      SkillId      = SkillId.IceBullet1,
      Exp          = 10,
      PrefabPath   = "Enemy/024/024.prefab",
    },
    
    new EnemyEntity() {
      Id           = EnemyId.Enemy027,
      No           = 27,
      Name         = "ライネ",
      HP           = 30,
      Power        = 8,
      Speed        = 1f,
      Mass         = 5.0f,
      AttackAttr   = (uint)(Attribute.Thu),
      WeakAttr     = (uint)(Attribute.Win),
      ResistAttr   = (uint)(Attribute.Thu),
      NullfiedAttr = (uint)(Attribute.Nil),
      SkillId      = SkillId.ThunderBullet1,
      Exp          = 10,
      PrefabPath   = "Enemy/027/027.prefab",
    },
    
  };
}