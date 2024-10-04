using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEditor;
using UnityEngine;
using MyGame.Old;

public class BattleLocation : MyMonoBehaviour
{
  //============================================================================
  // Inspector Variables
  //============================================================================

  /// <summary>
  /// レベル
  /// </summary>
  [SerializeField]
  private int lv = 1;

  /// <summary>
  /// クリアに必要な撃破数
  /// </summary>
  [SerializeField]
  private int requiredKillCount = 0;

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
  /// 出現する敵の総数
  /// </summary>
  public int TotalEnemyCount {
    get {
      int total = 0;
      ForeachWaveData((config) => {
        total += config.props.TotalEnemyCount;
      });
      return total;
    }
  }

  /// <summary>
  /// 戦地の名称
  /// </summary>
  public string LocationName {
    get { return GetComponentInChildren<TextMesh>().text; }
  }

  /// <summary>
  /// 戦闘クリアに必要な撃破数
  /// </summary>
  public int RequiredKillCount => requiredKillCount;

  //============================================================================
  // Methods
  //============================================================================

  //----------------------------------------------------------------------------
  // Public
  //----------------------------------------------------------------------------

  /// <summary>
  /// Idle状態にする、同時に非アクティブ化
  /// </summary>
  public void Idle()
  {
    state.SetState(State.Idle);
    SetActive(false);
  }

  /// <summary>
  /// 通常状態にする、同時にアクティブ化
  /// </summary>
  public void Run()
  {
    if (state.StateKey == State.Idle) {
      SetActive(true);
      state.SetState(State.Usual);
    }
  }

  //----------------------------------------------------------------------------
  // Unity Life Cycle
  //----------------------------------------------------------------------------

  protected override void MyAwake()
  {
    collider = GetComponent<SphereCollider>();

    state.Add(State.Idle);
    state.Add(State.Usual, EnterUsual, UpdateUsual);
    state.Add(State.Contact, EnterContact, UpdateContact, ExitContact);
    Idle();

    // 子要素に設定されているWaveDataを収集
    CollectWaveData();
    Validate();

    // FieldManagerがあれば自身を登録する
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
    if (state.StateKey == State.Idle) return;

    // プレイヤーとの衝突判定
    var pm = PlayerManager.Instance;
    var a = pm.Position;
    var b = collider.transform.position;
    var r = pm.Collider.radius + collider.radius;
    isPlayerHit = CollisionUtil.IsCollideAxB(a, b, r);
  }

  //----------------------------------------------------------------------------
  // for Update
  //----------------------------------------------------------------------------

  //----------------------------------------------------------------------------
  // Usual State

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

  //----------------------------------------------------------------------------
  // Contact State

  private void EnterContact()
  {
    Logger.Log("[BattleLocation] OnHitPlayerEnter");

    // 戦場を予約
    if (FieldManager.Instance) {
      FieldManager.Instance.ReserveLocation(this);
    }

    // UIを表示
    if (UIManager.Instance)
    { 
      UIManager.Instance.BattleLocationBoard.Show(new BattleLocationInfo() {
        Name        = LocationName,
        Lv          = lv,
        EnemyIds    = AppearingEnemyIds,
        TargetCount = RequiredKillCount,
      });
    }
  }

  private void UpdateContact()
  {
    if (isPlayerHit == false) {
      state.SetState(State.Usual);
      return;
    }
  }

  private void ExitContact()
  {
    Logger.Log("[BattleLocation] OnHitPlayerExit");

    // 戦場予約を解除
    if (FieldManager.Instance) {
      FieldManager.Instance.ReserveLocation(null);
    }
    
    // UIを非表示
    if (UIManager.Instance) {
      UIManager.Instance.BattleLocationBoard.Hide();
    }
  }

  //----------------------------------------------------------------------------
  // Other
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

      // Wave{i}というオブジェクトの配下にあるEnemyWaveConfigコンポーネントを取得、保持
      var configs = wave.GetComponentsInChildren<EnemyWaveConfig>();

      if (!this.data.ContainsKey(i)) {
        this.data.Add(i, new List<EnemyWaveConfig>(configs));
      }
    }

    // BattleLocationに設定されているLvをconfig側に設定する
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
  /// BattleLocationに設定されているEnemyWaveConfigからEnemyWaveProperty一式を生成する
  /// </summary>
  public Dictionary<int, List<EnemyWaveProperty>> MakeEnemyWavePropertySet()
  {
    var data = new Dictionary<int, List<EnemyWaveProperty>>();

    foreach (var kv in this.data) 
    {
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

    if (TotalEnemyCount < RequiredKillCount) {
      ErrorLog($"出現する敵の総数がクリアに必要な撃破数未満になっています。敵総数={TotalEnemyCount} 必要撃破数={RequiredKillCount}");
    }

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

  [Conditional("_DEBUG")]
  private void ErrorLog(string msg)
  {
    Logger.Error($"[BattleLocation] {LocationName} {msg}");
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

#if UNITY_EDITOR
  [CustomEditor(typeof(BattleLocation))]
  public class BattleLocationEditor : Editor
  {
    /// <summary>
    /// Waveデータ
    /// </summary>
    public override void OnInspectorGUI()
    {
      base.OnInspectorGUI();
      
      // WaveDataを収集
      var location = (BattleLocation)target;
      location.data.Clear();
      location.CollectWaveData();

      // locationに設定されているconfigのFAT、EIPSを表示する
      for(int i = 0, count = location.data.Count; i < count; i++) {
        GUILayout.Label($"Wave[{i}]");
        ShowConfigs(location.lv, location.data[i]);
      }
    }

    private void ShowConfigs(int lv, List<EnemyWaveConfig> configs)
    {
      for (int i = 0, count = configs.Count; i < count; ++i) {
        ShowConfig(i, lv, configs[i]);
      }
    }

    private void ShowConfig(int index, int lv, EnemyWaveConfig config)
    {
      using (new GUILayout.HorizontalScope()) {
        GUILayout.Label($"{index}", GUILayout.Width(20));
        GUILayout.Label($"FAT={(int)config.props.CalcFAT()}", GUILayout.Width(100));
        GUILayout.Label($"EIPS={(int)config.props.CalcEIPS(lv)}", GUILayout.Width(100));
      }
    }
  }
#endif
}
