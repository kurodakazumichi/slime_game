using System;
using System.Collections.Generic;
using UnityEngine;

public class BattleLocation : MonoBehaviour
{
  //============================================================================
  // Enum
  //============================================================================
  private enum State 
  {
    Idle,
    Usual,
    Contact,
  }

  //============================================================================
  // Variables
  //============================================================================
  
  /// <summary>
  /// ステートマシン
  /// </summary>
  private StateMachine<State> state = new StateMachine<State>();

  /// <summary>
  /// Waveデータ
  /// </summary>
  private Dictionary<int, List<EnemyWaveConfig>> data = new Dictionary<int, List<EnemyWaveConfig>>();

  /// <summary>
  /// Playerにヒットしているかどうかのフラグ
  /// CollisionManagerの衝突判定処理から設定される
  /// </summary>
  private bool isPlayerHit { get; set; } = false;

  new private SphereCollider collider;

  //============================================================================
  // Properities
  //============================================================================

  /// <summary>
  /// Waveデータを持っているか
  /// </summary>
  public bool HasWaves {
    get {
      return 0 < data.Count;
    }
  }

  /// <summary>
  /// 出現する敵のIDリスト
  /// </summary>
  public List<EnemyId> AppearingEnemyIds 
  {
    get { 
      var list = new List<EnemyId>();

      ForeachWaveData((setting) => 
      {
        var id = MyEnum.Parse<EnemyId>(setting.props.EnemyId);

        if (!list.Contains(id)) {
          list.Add(id);
        }
      });

      return list;
    }
  }

  //============================================================================
  // Methods
  //============================================================================

  //----------------------------------------------------------------------------
  // Unity Life Cycle
  //----------------------------------------------------------------------------

  void Awake()
  {
    collider = GetComponent<SphereCollider>();
    CollectWaveData();

    state.Add(State.Idle);
    state.Add(State.Usual, EnterUsual, UpdateUsual);
    state.Add(State.Contact, EnterContact, UpdateContact, ExitContact);
    state.SetState(State.Usual);

    if (FieldManager.Instance) {
      FieldManager.Instance.RegistBattleLocation(this);
    }
  }

  private void Update()
  {
    state.Update();
  }

  private void LateUpdate()
  {
    if (state.StateKey == State.Idle) {
      return;
    }

    var a = PlayerManager.Instance.PlayerOriginPosition;
    var b = collider.transform.position;
    var r = PlayerManager.Instance.PlayerCollider.radius + collider.radius;
    isPlayerHit = CollisionUtil.IsCollideAxB(a, b, r);
  }

  //----------------------------------------------------------------------------
  // for Update
  //----------------------------------------------------------------------------

  private void EnterUsual()
  {
    isPlayerHit = false;
  }

  private void UpdateUsual()
  {
    if (isPlayerHit) {
      state.SetState(State.Contact);
      return;
    }
  }

  private void EnterContact()
  {
    // WaveManagerにBattleLocationを予約する
    Logger.Log("[BattleLocation] OnHitPlayerEnter");

    WaveManager.Instance.ReserveBattleLocation(this);
  }

  private void UpdateContact()
  {
    Logger.Log("[BattleLocation] OnHitPlayerStay");
    if (isPlayerHit == false) {
      state.SetState(State.Usual);
      return;
    }
  }

  private void ExitContact()
  {
    Logger.Log("[BattleLocation] OnHitPlayerExit");
    // WaveManagerにWaveデータの破棄を依頼する
    WaveManager.Instance.CancelBattleLocation();
  }

  //----------------------------------------------------------------------------
  // for Me
  //----------------------------------------------------------------------------

  /// <summary>
  /// BattleLocationに含まれるEnemyWaveParamをかき集める
  /// </summary>
  private void CollectWaveData()
  {
    // WaveDataという名前のオブジェクトを探す
    var waveDataObject = transform.Find("WaveData");

    if (waveDataObject == null) {
      Logger.Error("[BattleLocation] This BattleLocation does not have WaveData Object.");
      return;
    }

    // WaveDataオブジェクト配下にあるWave0, Wave1, ... という名前のオブジェクトを探す
    for (int i = 0; true; i++) 
    {
      var wave = waveDataObject.Find($"Wave{i}");

      if (wave == null) {
        return;
      }

      // WaveXというオブジェクトの配下にあるEnemyWaveSettingsコンポーネントを取得、保持
      var configs = wave.GetComponentsInChildren<EnemyWaveConfig>();
      this.data.Add(i, new List<EnemyWaveConfig>(configs));
    }
  }

  /// <summary>
  /// WaveDataに含まれるEnemyWaveParamの数だけループをする
  /// </summary>
  private void ForeachWaveData(Action<EnemyWaveConfig> action)
  {
    foreach (var configs in data.Values) 
    {
      if (configs == null) continue;

      foreach (var config in configs) 
      {
        if (config != null) {
          action(config);
        }
      }
    }
  }

  public Dictionary<int, List<EnemyWaveProperty>> MakeEnemyWaveProperty()
  {
    var data = new Dictionary<int, List<EnemyWaveProperty>>();

    foreach (var kv in this.data) {
      data.Add(kv.Key, new List<EnemyWaveProperty>());

      foreach (var config in kv.Value) {
        data[kv.Key].Add(config.props.Clone());
      }
    }

    return data;
  }
}
