using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame.Master
{
  public interface IPlayerEntity 
  { 
    float SpeedInSearch { get; }
    float SpeedInBattle { get; }
    int InitialHP { get; }
  }

  public static class PlayerMaster
  {
    public static IPlayerEntity Config { get; private set; }

    public static void Init()
    {
      var dir  = "Player";
      var file = "Config";
      Config = MasterUtil.LoadEntity<PlayerEntity>(dir, file);
    }
  }
}
