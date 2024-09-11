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
      IconNo          = 5,
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
      IconNo          = 0,
    },
    
    new SkillEntity() {
      Id              = SkillId.FireBullet1,
      Name            = "火炎弾",
      MaxExp          = 999,
      FirstRecastTime = 1f,
      LastRecastTime  = 0.2f,
      FirstPower      = 1,
      LastPower       = 10,
      Attr            = (uint)(Attribute.Fir),
      Prefab          = "Bullet/FireBullet1/Object.prefab",
      GrowthType      = Growth.Normal,
      IconNo          = 0,
    },
    
  };
}