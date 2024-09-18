﻿using UnityEngine;
using UnityEngine.SceneManagement;

public class FieldScene : MyMonoBehaviour
{
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

  private StateMachine<State> state;

  private AsyncOperation loadScene;

  private int requiredKillCount = 0;

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
    DebugManager.Instance.Regist(this);
    DebugManager.Instance.Regist(FieldManager.Instance);
    DebugManager.Instance.Regist(BulletManager.Instance);
    DebugManager.Instance.Regist(SkillManager.Instance);
    DebugManager.Instance.Regist(ResourceManager.Instance);
    DebugManager.Instance.Regist(EnemyManager.Instance);

    SkillManager.Instance.OnGetNewSkill  = OnGetNewSkill;
    SkillManager.Instance.OnLevelUpSkill = OnLevelUpSkill;

    EnemyManager.Instance.OnDeadEnemy = (e) => 
    {
      requiredKillCount--;
      UIManager.Instance.KillCount.CountUp();

      float count = UIManager.Instance.KillCount.Count;
      float max = count + requiredKillCount;

      UIManager.Instance.HUD.ClearGauge.Set(count, count / max);

      var item = ItemManager.Instance.GetSkillItem();
      item.Position = e.Position;
      item.Init(e.SkillId, e.Exp);
    };

    PlayerManager.Instance.OnChangePlayerHP = (int hp, float rate) => {
      UIManager.Instance.HUD.HpGauge.Set(hp, rate);
    };

    UIManager.Instance.HUD.ClearGauge.SetActive(false);
    UIManager.Instance.KillCount.SetActive(false);
  }

  private void UpdateSystemSetup()
  {
    state.SetState(State.ResrouceLoading);
  }

  //----------------------------------------------------------------------------
  // for Resource Loading

  private void EnterResourceLoading()
  {
    // MultipleSpriteのロード例
    // ResourceManager.Instance.LoadSprites("Icon/Enemies.png");

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

    // MultipleSpriteの取得例
    // var sprites = ResourceManager.Instance.GetSpritesCache("Icon/Enemies.png");

    state.SetState(State.LevelLoading);
  }

  //----------------------------------------------------------------------------
  // for Level Loading

  private void EnterLevelLoading()
  {
    loadScene = SceneManager.LoadSceneAsync("Scenes/Level/level_sample", LoadSceneMode.Additive);
  }

  private void UpdateLevelLoading()
  {
    if (loadScene.isDone) {
      state.SetState(State.Serach);
      return;
    }
  }

  private void EnterSearch()
  {
    FieldManager.Instance.SetActiveBattleLocations(true);
    PlayerManager.Instance.RespawnPlayer();
    PlayerManager.Instance.Playable();
  }

  private void UpdateSearch()
  {
    if (FieldManager.Instance.IsBattleReserved && Input.GetKeyDown(KeyCode.A)) {
      state.SetState(State.Battle);
    }
  }

  private void EnterBattle()
  {
    TimeSystem.Player.Scale = 0.5f;
    UIManager.Instance.Toaster.Bake("戦闘開始!!");

    UIManager.Instance.HUD.ClearGauge.SetActive(true);
    UIManager.Instance.HUD.ClearGauge.Set(0, 0);
    UIManager.Instance.KillCount.SetActive(true);

    // このフェーズはバトルが予約されている時にしか遷移してこないのでチェックしておく
    if (!FieldManager.Instance.IsBattleReserved) 
    {
      Logger.Error("[FieldScene] Battle status must be reserved for the battle.");
      return;
    }

    UIManager.Instance.KillCount.Reset();


    // スキルを起動
    RunSkill();

    UIManager.Instance.BattleInfo.IsVisible = false;

    var fm = FieldManager.Instance;
    var wm = WaveManager.Instance;

    wm.SetEnemyWavePropertySet(fm.MakeCurrentEnemyWavePropertySet());
    requiredKillCount = fm.RequiredKillCountCandidate;
    fm.ActivateBattleCircle();
    fm.CancelBattleLocation();          // もうWaveの情報は生成したので予約は解除
    fm.SetActiveBattleLocations(false); // BattleLocationは非表示
    WaveManager.Instance.Run();
  }

  private void UpdateBattle()
  {
    // プレイヤーが死亡したか敵が全滅したらリザルトへ
    if (PlayerManager.Instance.PlayerIsDead) {
      UIManager.Instance.Toaster.Bake("敗北!!");
      state.SetState(State.BattleEnded);
      return;
    }

    // 目標撃破数に到達したらリザルトへ
    if (requiredKillCount <= 0) {
      UIManager.Instance.Toaster.Bake("勝利!!");
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
    ItemManager.Instance.Clear();

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
    UIManager.Instance.KillCount.Reset();
    TimeSystem.MenuPause = false;
  }

  //----------------------------------------------------------------------------
  // For Me
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
