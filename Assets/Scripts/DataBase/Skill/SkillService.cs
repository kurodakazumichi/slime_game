using System.Collections.Generic;
using UnityEngine;


public static class SkillService
{
  public static ISkillEntityRO FindById(SkillId id)
  {
    return SkillRepository.entities.Find(entity => entity.Id == id);
  }
}
