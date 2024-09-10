using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldManager : SingletonMonoBehaviour<FieldManager>
{
  public ParticleSystem Area;

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
  // Properities
  //============================================================================

  /// <summary>
  /// 戦地が予約されているならばtrue
  /// </summary>
  public bool IsBattleReserved {
    get { return battleLocationCandidate != null; }
  }

  public Vector3 BattlePosition { get; private set; } = Vector3.zero;

  public bool IsLockArea {
    get { return BattlePosition != Vector3.zero; }
  }

  //============================================================================
  // Methods
  //============================================================================

  protected override void MyAwake()
  {
    Area.gameObject.SetActive(false);
  }

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

  public void LockArea()
  {
    if (battleLocationCandidate is not null) {
      BattlePosition = battleLocationCandidate.Position;
      Area.transform.position = BattlePosition;
      Area.gameObject.SetActive(true);
    }
  }

  public void UnlockArea()
  {
    BattlePosition = Vector3.zero;
    Area.gameObject.SetActive(false);
  }

  /// <summary>
  /// 戦地をキャンセルする
  /// </summary>
  public void CancelBattleLocation()
  {
    battleLocationCandidate = null;
  }

  /// <summary>
  /// Field上のバトルロケーションのアクティブを設定
  /// </summary>
  public void SetActiveBattleLocations(bool flag)
  {
    foreach (var location in battleLocations)
    {
      location.SetActive(flag);
      if (flag) {
        location.Run();
      }
    }
  }

  /// <summary>
  /// 現在設定されているBattleLocationからEnemyWaveProperty一式を生成する
  /// </summary>
  public Dictionary<int, List<EnemyWaveProperty>> MakeCurrentEnemyWavePropertySet()
  {
    if (IsBattleReserved) {
      return battleLocationCandidate.MakeEnemyWavePropertySet();
    } else {
      return null;
    }
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
    if (IsBattleReserved) {
      battleLocationCandidate.OnDebug();
    }

    var tmp = _isShowBattleLocations;
    _isShowBattleLocations = GUILayout.Toggle(_isShowBattleLocations, "Show Battle Locations");
    if (_isShowBattleLocations != tmp) {
      SetActiveBattleLocations(_isShowBattleLocations);
    }
  }

#endif
}
