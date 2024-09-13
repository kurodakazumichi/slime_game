using System.Collections.Generic;

public static class SkillRepository
{
  public static List<ISkillEntityRO> entities = new List<ISkillEntityRO>() 
  {
    new SkillEntity() {
      Id              = SkillId.NormalBullet1,
      Name            = "通常弾",
      MaxExp          = 100,
      FirstRecastTime = 1f,
      LastRecastTime  = 0.2f,
      FirstPower      = 1,
      LastPower       = 10,
      Attr            = (uint)(Attribute.Non),
      Prefab          = "Bullet/NormalBullet1/Object.prefab",
      GrowthType      = Growth.Fast,
      Impact          = 1f,
      IconNo          = 0,
    },
    
    new SkillEntity() {
      Id              = SkillId.NormalBullet2,
      Name            = "通常弾+",
      MaxExp          = 999,
      FirstRecastTime = 1f,
      LastRecastTime  = 0.2f,
      FirstPower      = 20,
      LastPower       = 100,
      Attr            = (uint)(Attribute.Non),
      Prefab          = "Bullet/NormalBullet2/Object.prefab",
      GrowthType      = Growth.Normal,
      Impact          = 2f,
      IconNo          = 0,
    },
    
    new SkillEntity() {
      Id              = SkillId.NormalBullet3,
      Name            = "通常弾++",
      MaxExp          = 9999,
      FirstRecastTime = 1f,
      LastRecastTime  = 0.2f,
      FirstPower      = 200,
      LastPower       = 500,
      Attr            = (uint)(Attribute.Non),
      Prefab          = "Bullet/NormalBullet3/Object.prefab",
      GrowthType      = Growth.Normal,
      Impact          = 3f,
      IconNo          = 0,
    },
    
    new SkillEntity() {
      Id              = SkillId.PiercingBullet1,
      Name            = "貫通弾",
      MaxExp          = 200,
      FirstRecastTime = 1f,
      LastRecastTime  = 0.2f,
      FirstPower      = 1,
      LastPower       = 5,
      Attr            = (uint)(Attribute.Non),
      Prefab          = "Bullet/PiercingBullet1/Object.prefab",
      GrowthType      = Growth.Fast,
      Impact          = 0.5f,
      IconNo          = 6,
    },
    
    new SkillEntity() {
      Id              = SkillId.RapidShot1,
      Name            = "高速弾",
      MaxExp          = 999,
      FirstRecastTime = 1f,
      LastRecastTime  = 0.2f,
      FirstPower      = 1,
      LastPower       = 10,
      Attr            = (uint)(Attribute.Non),
      Prefab          = "Bullet/RapidShot1/Object.prefab",
      GrowthType      = Growth.Normal,
      Impact          = 1f,
      IconNo          = 0,
    },
    
    new SkillEntity() {
      Id              = SkillId.HeavyShot1,
      Name            = "重鉄弾",
      MaxExp          = 999,
      FirstRecastTime = 5f,
      LastRecastTime  = 1f,
      FirstPower      = 10,
      LastPower       = 100,
      Attr            = (uint)(Attribute.Non),
      Prefab          = "Bullet/HeavyShot1/Object.prefab",
      GrowthType      = Growth.Normal,
      Impact          = 10f,
      IconNo          = 0,
    },
    
    new SkillEntity() {
      Id              = SkillId.FireBullet1,
      Name            = "ファイアバレット",
      MaxExp          = 999,
      FirstRecastTime = 1f,
      LastRecastTime  = 0.2f,
      FirstPower      = 1,
      LastPower       = 10,
      Attr            = (uint)(Attribute.Fir),
      Prefab          = "Bullet/FireBullet1/Object.prefab",
      GrowthType      = Growth.Normal,
      Impact          = 1f,
      IconNo          = 2,
    },
    
    new SkillEntity() {
      Id              = SkillId.WaterBullet1,
      Name            = "ウォーターボール",
      MaxExp          = 999,
      FirstRecastTime = 1f,
      LastRecastTime  = 0.2f,
      FirstPower      = 1,
      LastPower       = 10,
      Attr            = (uint)(Attribute.Wat),
      Prefab          = "Bullet/WaterBullet1/Object.prefab",
      GrowthType      = Growth.Normal,
      Impact          = 1f,
      IconNo          = 3,
    },
    
    new SkillEntity() {
      Id              = SkillId.ThunderBullet1,
      Name            = "サンダーウェイブ",
      MaxExp          = 999,
      FirstRecastTime = 1f,
      LastRecastTime  = 0.2f,
      FirstPower      = 1,
      LastPower       = 10,
      Attr            = (uint)(Attribute.Thu),
      Prefab          = "Bullet/ThunderBullet1/Object.prefab",
      GrowthType      = Growth.Normal,
      Impact          = 1f,
      IconNo          = 4,
    },
    
    new SkillEntity() {
      Id              = SkillId.LeafBullet1,
      Name            = "リーフカッター",
      MaxExp          = 999,
      FirstRecastTime = 1f,
      LastRecastTime  = 0.5f,
      FirstPower      = 1,
      LastPower       = 10,
      Attr            = (uint)(Attribute.Lef),
      Prefab          = "Bullet/LeafBullet1/Object.prefab",
      GrowthType      = Growth.Normal,
      Impact          = 1f,
      IconNo          = 1,
    },
    
    new SkillEntity() {
      Id              = SkillId.IceBullet1,
      Name            = "アイスランス",
      MaxExp          = 999,
      FirstRecastTime = 1f,
      LastRecastTime  = 0.2f,
      FirstPower      = 1,
      LastPower       = 10,
      Attr            = (uint)(Attribute.Ice),
      Prefab          = "Bullet/IceBullet1/Object.prefab",
      GrowthType      = Growth.Normal,
      Impact          = 1f,
      IconNo          = 5,
    },
    
    new SkillEntity() {
      Id              = SkillId.WindBullet1,
      Name            = "つむじ風",
      MaxExp          = 999,
      FirstRecastTime = 1f,
      LastRecastTime  = 0.2f,
      FirstPower      = 1,
      LastPower       = 10,
      Attr            = (uint)(Attribute.Win),
      Prefab          = "Bullet/WindBullet1/Object.prefab",
      GrowthType      = Growth.Normal,
      Impact          = 1f,
      IconNo          = 7,
    },
    
  };
}