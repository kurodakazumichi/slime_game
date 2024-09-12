using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class BattleLocation : MyMonoBehaviour
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

  [SerializeField]
  private int lv = 1;

  /// <summary>
  /// ステートマシン
  /// </summary>
  private readonly StateMachine<State> state = new();

  /// <summary>
  /// Waveデータ
  /// </summary>
  private readonly Dictionary<int, List<EnemyWaveConfig>> data = new();

  /// <summary>
  /// Playerにヒットしているかどうかのフラグ
  /// CollisionManagerの衝突判定処理から設定される
  /// </summary>
  private bool isPlayerHit { get; set; } = false;

  /// <summary>
  /// BattleLocationのコライダー
  /// </summary>
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
        if (!list.Contains(setting.props.Id)) {
          list.Add(setting.props.Id);
        }
      });

      return list;
    }
  }

  /// <summary>
  /// 戦地の名称
  /// </summary>
  public string LocationName {
    get { return GetComponentInChildren<TextMesh>().text; }
  }

  //============================================================================
  // Methods
  //============================================================================

  //----------------------------------------------------------------------------
  // Public
  //----------------------------------------------------------------------------
  public void Run()
  {
    if (state.StateKey == State.Idle) {
      state.SetState(State.Usual);
    }
  }

  //----------------------------------------------------------------------------
  // Unity Life Cycle
  //----------------------------------------------------------------------------

  protected override void MyAwake()
  {
    collider = GetComponent<SphereCollider>();
    CollectWaveData();

    state.Add(State.Idle);
    state.Add(State.Usual, EnterUsual, UpdateUsual);
    state.Add(State.Contact, EnterContact, UpdateContact, ExitContact);
    state.SetState(State.Idle);

    if (FieldManager.Instance) {
      FieldManager.Instance.RegistBattleLocation(this);
    }
  }

  private void Start()
  {
    Validate();
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

    var a = PlayerManager.Instance.Position;
    var b = collider.transform.position;
    var r = PlayerManager.Instance.Collider.radius + collider.radius;
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

    FieldManager.Instance.ReserveBattleLocation(this);
    UIManager.Instance.BattleInfo.Show(LocationName, lv, AppearingEnemyIds);
  }

  private void UpdateContact()
  {
    //Logger.Log("[BattleLocation] OnHitPlayerStay");
    if (isPlayerHit == false) {
      state.SetState(State.Usual);
      return;
    }
  }

  private void ExitContact()
  {
    Logger.Log("[BattleLocation] OnHitPlayerExit");
    // WaveManagerにWaveデータの破棄を依頼する
    FieldManager.Instance.CancelBattleLocation();

    UIManager.Instance.BattleInfo.Hide();
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
        break;
      }

      // WaveXというオブジェクトの配下にあるEnemyWaveSettingsコンポーネントを取得、保持
      var configs = wave.GetComponentsInChildren<EnemyWaveConfig>();
      this.data.Add(i, new List<EnemyWaveConfig>(configs));
    }

    // BattleLocationに設定されているLvをconfigに設定する
    ForeachWaveData((config) => { 
      config.props.EnemyLv = lv; 
    });
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

  /// <summary>
  /// BattleLocationに設定されている設定から、敵Waveプロパティ一式を生成する
  /// </summary>
  public Dictionary<int, List<EnemyWaveProperty>> MakeEnemyWavePropertySet()
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

  //----------------------------------------------------------------------------
  // For Validation
  //----------------------------------------------------------------------------

  [Conditional("_DEBUG")]
  private void Validate()
  {
    Logger.Log($"[BattleLocation.Validate] {LocationName}");

    ForeachWaveData((config) => 
    {
      if (!MyEnum.TryParse<EnemyId>(config.props.EnemyId, out var id)) {
        ErrorLog(config, $"EnemyId parse error. id = {config.props.EnemyId}");
      }
    });
  }

  [Conditional("_DEBUG")]
  private void ErrorLog(EnemyWaveConfig config, string msg)
  {
    var parentName = config.CachedTransform.parent.name;
    var name       = config.CachedTransform.name;
    Logger.Error($"[BattleLocation] {LocationName}.{parentName}.{name} {msg}");
  }

#if _DEBUG
  //----------------------------------------------------------------------------
  // For Debug
  //----------------------------------------------------------------------------

  /// <summary>
  /// デバッグ用の基底メソッド
  /// </summary>
  public override void OnDebug()
  {
    using (new GUILayout.VerticalScope(GUI.skin.box)) 
    {
      GUILayout.Label($"Location Name = {LocationName}");
      GUILayout.Label("出現する敵");
      string s = "";
      foreach (var enemyId in AppearingEnemyIds)
      {
        s += enemyId + ",";
      }
      GUILayout.Label(s);
    }
  }

#endif

}
