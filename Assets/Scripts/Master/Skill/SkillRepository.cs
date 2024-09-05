using System.Collections.Generic;

public static class SkillRepository
{
  public static List<ISkillEntityRO> entities = new List<ISkillEntityRO>()
  {
    new SkillEntity() {
      Id              = SkillId.NormalBullet,
      Name            = "通常弾",
      MaxExp          = 100,
      FirstRecastTime = 1f,
      LastRecastTime  = 0.2f,
      FirstPower      = 1,
      LastPower       = 10,
      Attr            = (uint)(Attribute.Non),
      Prefab          = "Bullet/NormalBullet.prefab",
    },

    new SkillEntity() {
      Id              = SkillId.RapidShot,
      Name            = "高速弾",
      MaxExp          = 999,
      FirstRecastTime = 1f,
      LastRecastTime  = 0.2f,
      FirstPower      = 1,
      LastPower       = 10,
      Attr            = (uint)(Attribute.Non),
      Prefab          = "",
    },

    new SkillEntity() {
      Id              = SkillId.HeavyShot,
      Name            = "重鉄弾",
      MaxExp          = 999,
      FirstRecastTime = 5f,
      LastRecastTime  = 1f,
      FirstPower      = 10,
      LastPower       = 100,
      Attr            = (uint)(Attribute.Non),
      Prefab          = "",
    },

    new SkillEntity() {
      Id              = SkillId.FireBullet,
      Name            = "火炎弾",
      MaxExp          = 999,
      FirstRecastTime = 1f,
      LastRecastTime  = 0.2f,
      FirstPower      = 1,
      LastPower       = 10,
      Attr            = (uint)(Attribute.Fir),
      Prefab          = "",
    },

  };
}