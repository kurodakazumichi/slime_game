using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ReadOnlyのSkillEntityインターフェース
/// </summary>
public interface ISkillEntity
{
  SkillId Id { get; }

  int MaxExp { get; }
  float FirstRecastTime { get; }
  float LastRecastTime { get; }
  int FirstPower { get; }
  int LastPower { get; }
  int FirstPenetrableCount { get; }
  int LastPenetrableCount { get; }
  float SpeedGrowthRate { get; }
  uint Attr { get; }
  string Name { get; }
  string Prefab { get; }
  Growth GrowthType { get; }
  float Impact { get; }
  int IconNo { get; }
  SkillAimingType Aiming { get; }

}

public static class SkillMaster
{
  public static ISkillEntity FindById(SkillId id)
  {
    return SkillRepository.entities.Find(entity => entity.Id == id);
  }
}
