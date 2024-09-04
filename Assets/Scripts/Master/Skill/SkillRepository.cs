using System.Collections.Generic;

public static class SkillRepository
{
  public static List<ISkillEntityRO> entities = new List<ISkillEntityRO>() {


    new SkillEntity() {
      Id = SkillId.NormalBullet,
      MaxExp = 100,
      FirstRecastTime = 1f,
      LastRecastTime = 0.2f,
      FirstPower = 1,
      LastPower = 10,
      Attr = (uint)(Attribute.Fir|Attribute.Wat),
      Name = "通常弾"
    },

  };
}