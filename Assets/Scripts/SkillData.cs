using System.Collections.Generic;
using UnityEngine;

public interface ISkillEntityRO
{
  SkillId Id { get; }

  int MaxExp { get; }
  float FirstRecastTime { get; }
  float LastRecastTime { get; }
  int FirstPower { get; }
  int LastPower { get; }
  int Attr { get; }
  string Name { get; }
}

public class SkillEntity : ISkillEntityRO
{
  public SkillEntity(SkillId id, int exp, float fr, float fl, int fp, int lp, int attr, string name)
  {
    Id = id;
    MaxExp = exp;
    FirstRecastTime = fr;
    LastRecastTime = fl;
    FirstPower = fp;
    LastPower = lp;
    Attr = attr;
    Name = name;
  }

  public SkillId Id { get; set; }
  public int MaxExp { get; set; }
  public float FirstRecastTime { get; set; }
  public float LastRecastTime { get; set; }
  public int FirstPower { get; set; }
  public int LastPower { get; set; }
  public int Attr { get; set; }
  public string Name { get; set; }
}


public static class SkillData
{
  private static Dictionary<int, ISkillEntityRO> entities = new Dictionary<int, ISkillEntityRO>() {
    [(int)SkillId.NormalBullet] = new SkillEntity(SkillId.NormalBullet, 30, 1f, 0.1f, 1, 10, (int)Attribute.Non, "í èÌíe"),
  };

  public static ISkillEntityRO FindById(SkillId id)
  {
    return entities[(int)id];
  }
}
