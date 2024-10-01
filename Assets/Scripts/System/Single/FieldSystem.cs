using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame.System
{

  public interface IFieldSystem
  {
    Vector3 BattleCircleCenter { get; }
    bool HasBattleCircle { get; }
    bool IsInBattleCircle(Vector3 position);
  }

}