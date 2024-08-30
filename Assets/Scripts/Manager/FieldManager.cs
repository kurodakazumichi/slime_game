﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldManager : SingletonMonoBehaviour<FieldManager>
{
  //============================================================================
  // Variables
  //============================================================================

  /// <summary>
  /// Field上に存在するBattleLocationを格納
  /// </summary>
  private List<BattleLocation> battleLocations = new List<BattleLocation>();

  /// <summary>
  /// 戦闘予定のロケーションを格納
  /// </summary>
  private BattleLocation battleLocationCandidate = null;

  //============================================================================
  // Methods
  //============================================================================
  
  //----------------------------------------------------------------------------
  // Public
  //----------------------------------------------------------------------------

  /// <summary>
  /// BattleLocationを登録する
  /// </summary>
  public void RegistBattleLocation(BattleLocation location)
  {
    battleLocations.Add(location);
  }

  /// <summary>
  /// 戦地の候補を予約する
  /// </summary>
  public void ReserveBattleLocation(BattleLocation location)
  {
    battleLocationCandidate = location;
  }

  /// <summary>
  /// 戦地をキャンセルする
  /// </summary>
  public void CancelBattleLocation()
  {
    battleLocationCandidate = null;
  }
}
