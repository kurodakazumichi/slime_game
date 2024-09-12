using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldManager : SingletonMonoBehaviour<FieldManager>
{
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
  /// 戦闘予定のロケーションを格納
  /// </summary>
  private BattleLocation battleLocationCandidate = null;

  private  GameObject battleCircle;

  //============================================================================
  // Properities
  //============================================================================

  /// <summary>
  /// 戦地が予約されているならばtrue
  /// </summary>
  public bool IsBattleReserved {
    get { return battleLocationCandidate != null; }
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
  /// BattleCircleを発動する
  /// </summary>
  public void ActivateBattleCircle()
  {
    if (battleLocationCandidate is null) {
      return;
    }

    battleCircle.transform.position = battleLocationCandidate.Position;
    battleCircle.gameObject.SetActive(true);
  }

  /// <summary>
  /// BattleCircleを停止する
  /// </summary>
  public void InactivateBattleCircle()
  {
    battleCircle.gameObject.SetActive(false);
  }

  public bool IsInBattleCircle(Vector3 position)
  {
    if (!HasBattleCircle) {
      return false;
    }

    var v = BattleCircleCenter - position;
    var r = App.BATTLE_CIRCLE_RADIUS;
    return (v.sqrMagnitude <= r * r);
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
