using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EnemyMaster
{
  public static IEnemyEntityRO FindById(EnemyId id)
  {
    return EnemyRepository.entities.Find(entity => entity.Id == id);
  }
}
