using MyGame.Core.System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldManager : SingletonMonoBehaviour<FieldManager>
#if _DEBUG
  ,IDebugable
#endif
{
  //============================================================================
  // Inspector
  //============================================================================

  [SerializeField]
  private GameObject battleCirclePrefab;

  //============================================================================
  // Variables
  //============================================================================

  /// <summary>
  /// Field上に存在するBattleLocationを格納
  /// </summary>
  private List<BattleLocation> battleLocations = new List<BattleLocation>();

  /// <summary>
  /// 予約された戦場
  /// </summary>
  private BattleLocation reservedLocation = null;

  /// <summary>
  /// 確定された戦場
  /// </summary>
  private BattleLocation fixedLocation = null;

  /// <summary>
  /// 戦場の範囲を表す円上のオブジェクト
  /// </summary>
  private  GameObject battleCircle;

  //============================================================================
  // Properities
  //============================================================================

  /// <summary>
  /// 予約されている場所がある
  /// </summary>
  public bool HasReservedLocation => reservedLocation != null;

  /// <summary>
  /// 場所(戦場)は確定している
  /// </summary>
  public bool HasFixedLocation => fixedLocation != null;

  /// <summary>
  /// 戦場クリアに必要なキル数、予約地、もしくは確定地から取得する。
  /// </summary>
  public int RequiredKillCount {
    get {
      if (HasReservedLocation) return reservedLocation.RequiredKillCount;
      if (HasFixedLocation)    return fixedLocation.RequiredKillCount;
      return 0;
    }
  }

  /// <summary>
  /// バトルサークルが広がっている？
  /// </summary>
  public bool HasBattleCircle {
    get { return battleCircle.gameObject.activeSelf; }
  }

  /// <summary>
  /// バトルサークルの中心
  /// </summary>
  public Vector3 BattleCircleCenter {
    get {
      return battleCircle.transform.position;
    }
  }

  //============================================================================
  // Methods
  //============================================================================

  protected override void MyAwake()
  {
    battleCircle = Instantiate(battleCirclePrefab);
    battleCircle.gameObject.SetActive(false);
  }

  //----------------------------------------------------------------------------
  // Public
  //----------------------------------------------------------------------------

  //----------------------------------------------------------------------------
  // For　BattleLocation

  /// <summary>
  /// BattleLocationを登録する
  /// </summary>
  public void RegistBattleLocation(BattleLocation location)
  {
    battleLocations.Add(location);
  }

  /// <summary>
  /// 戦場を予約する
  /// </summary>
  public void ReserveLocation(BattleLocation location)
  {
    reservedLocation = location;
  }

  /// <summary>
  /// 場所を確定する
  /// </summary>
  public void FixLocation()
  {
    if (!HasReservedLocation) {
      Logger.Error("[FieldManager.FixedLocation] The reserved location does not exist.");
      return;
    }

    fixedLocation    = reservedLocation;
    reservedLocation = null;
  }

  /// <summary>
  /// Field上のバトルロケーションの有効化、無効化
  /// </summary>
  public void SetActiveBattleLocations(bool flag)
  {
    foreach (var location in battleLocations) {
      if (flag) {
        location.Run();
      }
      else {
        location.Idle();
      }
    }
  }

  /// <summary>
  /// 確定済のBattleLocationからEnemyWaveProperty一式を生成する
  /// </summary>
  public Dictionary<int, List<EnemyWaveProperty>> MakeFixedEnemyWavePropertySet()
  {
    if (HasFixedLocation) {
      return fixedLocation.MakeEnemyWavePropertySet();
    }
    else {
      return null;
    }
  }

  //----------------------------------------------------------------------------
  // For　BattleCircle

  /// <summary>
  /// BattleCircleを発動する
  /// </summary>
  public void ActivateBattleCircle()
  {
    if (!HasFixedLocation) return;

    battleCircle.transform.position = fixedLocation.Position;
    battleCircle.gameObject.SetActive(true);
  }

  /// <summary>
  /// BattleCircleを停止する
  /// </summary>
  public void InactivateBattleCircle()
  {
    battleCircle.gameObject.SetActive(false);
  }

  /// <summary>
  /// 指定した座標{position}がBattleCircleの円内にあるならばtrue
  /// </summary>
  public bool IsInBattleCircle(Vector3 position)
  {
    if (!HasBattleCircle) return false;
    return CollisionUtil.IsCollideAxB(BattleCircleCenter, position, App.BATTLE_CIRCLE_RADIUS);
  }

#if _DEBUG
  //----------------------------------------------------------------------------
  // For Debug
  //----------------------------------------------------------------------------
  private bool _isShowBattleLocations = true;

  /// <summary>
  /// デバッグ用の基底メソッド
  /// </summary>
  public override void OnDebug()
  {
    GUILayout.Label($"BattleLocationCount = {battleLocations.Count}");


    GUILayout.Label("Reserved Battle Location");
    if (HasReservedLocation) {
      reservedLocation.OnDebug();
    }

    var tmp = _isShowBattleLocations;
    _isShowBattleLocations = GUILayout.Toggle(_isShowBattleLocations, "Show Battle Locations");
    if (_isShowBattleLocations != tmp) {
      SetActiveBattleLocations(_isShowBattleLocations);
    }
  }

#endif
}
