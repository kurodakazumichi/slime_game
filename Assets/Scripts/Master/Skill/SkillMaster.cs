using System.Collections.Generic;
using UnityEngine;


public static class SkillMaster
{
  public static ISkillEntity FindById(SkillId id)
  {
    return SkillRepository.entities.Find(entity => entity.Id == id);
  }
}
