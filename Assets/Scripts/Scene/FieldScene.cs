using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FieldScene : MyMonoBehaviour
{
  //============================================================================
  // Enum
  //============================================================================

  private enum State { 
    Idle,
    SystemSetup,
    ResrouceLoading,
    LevelLoading,
    Serach,
    Battle,
    BattleEnded,
    Result,
    Menu,
  }

  //============================================================================
  // Variables
  //============================================================================
  private StateMachine<State> state;

  private AsyncOperation loadLevelSceneHandler;

  private int stageRemainingKillCount = 0;
  private int stageRequiredKillCount = 0;

  //============================================================================
  // Properties
  //============================================================================

  /// <summary>
  /// クリアゲージの長さを決定するプロパティ
  /// </summary>
  private float ClearGaugeRate {
    get {
      float a = stageRequiredKillCount - stageRemainingKillCount;
      float b = Mathf.Max(1.0f, stageRequiredKillCount);
      return a / b;
    }
  }

  //============================================================================
  // Methods
  //============================================================================

  protected override void MyAwake()
  {
    base.MyAwake();

    state = new StateMachine<State>();

    state.Add(State.Idle);
    state.Add(State.SystemSetup, EnterSystemSetup, UpdateSystemSetup);
    state.Add(State.ResrouceLoading, EnterResourceLoading, UpdateResourceLoading);
    state.Add(State.LevelLoading, EnterLevelLoading, UpdateLevelLoading);
    state.Add(State.Serach, EnterSearch, UpdateSearch);
    state.Add(State.Battle, EnterBattle, UpdateBattle);
    state.Add(State.BattleEnded, EnterBattleEnded, UpdateBattleEnded);
    state.Add(State.Result, EnterResult, UpdateResult, ExitResult);
    state.Add(State.Menu);
    state.SetState(State.Idle);
  }
  
  void Start()
  {
    state.SetState(State.SystemSetup);
  }

  void Update()
  {
    state.Update();
  }

  private void LateUpdate()
  {
    // 戦闘フェーズ中は衝突判定を動かす
    if(state.StateKey == State.Battle) {
      CollisionManager.Instance.CollidePlayerBulletWithEnemy();
      CollisionManager.Instance.CollideEnemyBulletWithPlayer();
    }
  }

  //----------------------------------------------------------------------------
  // for Update
  //----------------------------------------------------------------------------

  //----------------------------------------------------------------------------
  // for Setup

  private void EnterSystemSetup()
  {
    // デバッグマネージャーに登録
    DebugManager.Instance.Regist(this);
    DebugManager.Instance.Regist(FieldManager.Instance);
    DebugManager.Instance.Regist(BulletManager.Instance);
    DebugManager.Instance.Regist(SkillManager.Instance);
    DebugManager.Instance.Regist(ResourceManager.Instance);
    DebugManager.Instance.Regist(EnemyManager.Instance);

    // コールバック設定
    SkillManager.Instance.OnGetNewSkill     = OnGetNewSkill;
    SkillManager.Instance.OnLevelUpSkill    = OnLevelUpSkill;
    EnemyManager.Instance.OnDeadEnemy       = OnDeadEnemy;
    PlayerManager.Instance.OnChangePlayerHP = OnChangePlayerHP;
  }

  private void UpdateSystemSetup()
  {
    state.SetState(State.ResrouceLoading);
  }

  //----------------------------------------------------------------------------
  // for Resource Loading

  private void EnterResourceLoading()
  {
    // 暫定
    EnemyManager.Instance.Load();
    BulletManager.Instance.Load();
    IconManager.Instance.Load();
    ShadowManager.Instance.Load();
    ItemManager.Instance.Load();
  }

  private void UpdateResourceLoading()
  {
    if (ResourceManager.Instance.IsLoading) {
      Logger.Log("[FieldScene] Loading...");
      return;
    }

    state.SetState(State.LevelLoading);
  }

  //----------------------------------------------------------------------------
  // for Level Loading

  private void EnterLevelLoading()
  {
    loadLevelSceneHandler = SceneManager.LoadSceneAsync("Scenes/Level/level_sample", LoadSceneMode.Additive);
  }

  private void UpdateLevelLoading()
  {
    if (loadLevelSceneHandler.isDone) {
      state.SetState(State.Serach);
      return;
    }
  }

  //----------------------------------------------------------------------------
  // for Search

  private void EnterSearch()
  {
    FieldManager.Instance.SetActiveBattleLocations(true);
    PlayerManager.Instance.RespawnPlayer();
    PlayerManager.Instance.Playable();

    HideHudGauge();
  }

  private void UpdateSearch()
  {
    if (FieldManager.Instance.HasReservedLocation && Input.GetKeyDown(KeyCode.A)) {
      FieldManager.Instance.FixLocation(); // 戦場を確定
      state.SetState(State.Battle);
    }
  }

  private void EnterBattle()
  {
    TimeSystem.Player.Scale = 0.5f;
    UIManager.Instance.Toaster.Bake("戦闘開始!!");

#if _DEBUG
    // このフェーズは戦場が確定しているときしかこないのでチェックしておく
    if (!FieldManager.Instance.HasFixedLocation) 
    {
      Logger.Error("[FieldScene] Battle status must be reserved for the battle.");
      return;
    }
#endif

    // スキルを起動
    RunSkill();

    UIManager.Instance.BattleLocationBoard.IsVisible = false;

    var fm = FieldManager.Instance;
    var wm = WaveManager.Instance;

    wm.SetEnemyWavePropertySet(fm.MakeFixedEnemyWavePropertySet());
    stageRequiredKillCount  = fm.RequiredKillCount;
    stageRemainingKillCount = fm.RequiredKillCount;
    fm.ActivateBattleCircle();
    fm.SetActiveBattleLocations(false); // BattleLocationは非表示
    WaveManager.Instance.Run();

    ShowHudGauge();
    UIManager.Instance.HUD.UpdateClearGauge(stageRemainingKillCount, 0f);
  }

  private void UpdateBattle()
  {
    // プレイヤーが死亡したか敵が全滅したらリザルトへ
    if (PlayerManager.Instance.PlayerIsDead) {
      UIManager.Instance.Toaster.Bake("敗北!!");
      ItemManager.Instance.Clear();
      state.SetState(State.BattleEnded);
      return;
    }

    // 目標撃破数に到達したらリザルトへ
    if (stageRemainingKillCount <= 0) {
      UIManager.Instance.Toaster.Bake("勝利!!");
      ItemManager.Instance.Collect(PlayerManager.Instance.Position);

      state.SetState(State.BattleEnded);
      return;
    }
  }

  //----------------------------------------------------------------------------
  // for BattleEnded

  private void EnterBattleEnded()
  {
    TimeSystem.Player.Pause(true);

    UIManager.Instance.HUD.SkillSlots.Stop();
    BulletManager.Instance.Terminate();
    WaveManager.Instance.Terminate();
    FieldManager.Instance.InactivateBattleCircle();

    UIManager.Instance.Toaster.Bake("戦闘終了!!");
  }

  private void UpdateBattleEnded()
  {
    if (!UIManager.Instance.Toaster.IsIdle) {
      return;
    }

    if (0 < BulletManager.Instance.ActiveBulletCount) {
      return;
    }

    if (!WaveManager.Instance.IsIdle) {
      return;
    }

    if (0 < ItemManager.Instance.SkillItemCount) {
      return;
    }

    // リザルトへ
    state.SetState(State.Result);
  }

  private void EnterResult()
  {

  }

  private void UpdateResult()
  {
    if (Input.GetKeyDown(KeyCode.A)) {
      state.SetState(State.Serach);
    }
  }

  private void ExitResult()
  {
    TimeSystem.MenuPause = false;
  }

  //----------------------------------------------------------------------------
  // Callback
  //----------------------------------------------------------------------------

  private void OnGetNewSkill(int index)
  {
    if (state.StateKey == State.Battle) 
    {
      var skill = SkillManager.Instance.GetActiveSkill(index);

      UIManager.Instance.HUD.SkillSlots.SetSkill(index, skill);
      UIManager.Instance.HUD.SkillSlots.Run(index);

      UIManager.Instance.Toaster.Bake($"New {skill.Name}を覚えた!!");
    }
  }

  private void OnLevelUpSkill(SkillId id, int preLv, int lv)
  {
    if (state.StateKey == State.Battle) {
      var master = SkillMaster.FindById(id);
      var msg = $"Lv+ {master.Name} Lv {preLv} → {lv}";
      UIManager.Instance.Toaster.Bake(msg);
    }
  }

  private void OnDeadEnemy(IEnemy enemy)
  {
    stageRemainingKillCount--;
    UIManager.Instance.HUD.UpdateClearGauge(stageRemainingKillCount, ClearGaugeRate);

    var item = ItemManager.Instance.GetSkillItem();
    item.Setup(enemy.SkillId, enemy.Exp, enemy.Position);
  }

  private void OnChangePlayerHP(int hp, float rate)
  {
    UIManager.Instance.HUD.UpdateHpGauge(rate);
  }

  //----------------------------------------------------------------------------
  // HUD
  //----------------------------------------------------------------------------

  /// <summary>
  /// ゲージ系のUIを非表示にする
  /// </summary>
  private void HideHudGauge()
  {
    var hud = UIManager.Instance.HUD;

    hud.IsVisibleClearGauge = false;
    hud.IsVisibleHpGauge    = false;
  }

  /// <summary>
  /// ゲージ系のUIを表示する
  /// </summary>
  private void ShowHudGauge()
  {
    var hud = UIManager.Instance.HUD;
    hud.IsVisibleClearGauge = true;
    hud.IsVisibleHpGauge    = true;
  }

  private void RunSkill()
  {
    // スキルセット、スキルUI稼働
    for(int i = 0; i < App.ACTIVE_SKILL_MAX; ++i) {
      UIManager.Instance.HUD.SkillSlots.SetSkill(i, SkillManager.Instance.GetActiveSkill(i));
    }
    
    UIManager.Instance.HUD.SkillSlots.Run();
  }

  private void StopSkill()
  {
    UIManager.Instance.HUD.SkillSlots.Stop();
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
    GUILayout.Label("FieldScene");

    GUILayout.Label($"State = {state.StateKey.ToString()}");

    if (GUILayout.Button("スキルを実行")) {
      RunSkill();
    }

    if (GUILayout.Button("スキルを停止")) {
      StopSkill();
    }

    TimeSystem.OnDebug();
  }

#endif

}
