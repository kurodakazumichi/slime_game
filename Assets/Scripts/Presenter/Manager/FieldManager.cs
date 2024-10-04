using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame.Presenter.Manager
{

  public interface IFieldManager
  {
    Vector3 BattleCircleCenter { get; }
    bool HasBattleCircle { get; }
    bool IsInBattleCircle(Vector3 position);
  }

}