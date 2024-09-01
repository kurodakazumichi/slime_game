using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FieldScene : MyMonoBehaviour
{
  private enum State { 
    Idle,
    SystemSetup,
    LevelLoading,
    Serach,
    Battle,
    Result,
    Menu,
  }

  private StateMachine<State> state;

  AsyncOperation loadScene;

  //============================================================================
  // Methods
  //============================================================================

  protected override void MyAwake()
  {
    base.MyAwake();

    state = new StateMachine<State>();

    state.Add(State.Idle);
    state.Add(State.SystemSetup, EnterSystemSetup, UpdateSystemSetup);
    state.Add(State.LevelLoading, EnterLevelLoading, UpdateLevelLoading);
    state.Add(State.Serach, EnterSearch, UpdateSearch);
    state.Add(State.Battle, EnterBattle, UpdateBattle);
    state.Add(State.Result, EnterResult, UpdateResult);
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
    // プレイヤーの攻撃と敵の衝突
    if(state.StateKey == State.Battle) {
      CollisionManager.Instance.CollidePlayerAttackWithEnemy();
    }
  }

  //----------------------------------------------------------------------------
  // for Update
  //----------------------------------------------------------------------------
  private void EnterSystemSetup()
  {
    DebugManager.Instance.Regist(this);
    DebugManager.Instance.Regist(FieldManager.Instance);
  }

  private void UpdateSystemSetup()
  {
    state.SetState(State.LevelLoading);
  }

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
    // このフェーズはバトルが予約されている時にしか遷移してこないのでチェックしておく
    if (!FieldManager.Instance.IsBattleReserved) 
    {
      Logger.Error("[FieldScene] Battle status must be reserved for the battle.");
      return;
    }

    // スキルセット、スキルUI稼働
    UIManager.Instance.HUD.SkillSlots.SetSkill(0, SkillManager.Instance.GetActiveSkill(0));
    UIManager.Instance.HUD.SkillSlots.Run();

    var fm = FieldManager.Instance;
    var wm = WaveManager.Instance;

    wm.SetEnemyWavePropertySet(fm.MakeCurrentEnemyWavePropertySet());
    fm.CancelBattleLocation();          // もうWaveの情報は生成したので予約は解除
    fm.SetActiveBattleLocations(false); // BattleLocationは非表示
    WaveManager.Instance.Run();
  }

  private void UpdateBattle()
  {
    // プレイヤーが死亡したか敵が全滅したらリザルトへ
    if (PlayerManager.Instance.PlayerIsDead) {
      state.SetState(State.Result);
      return;
    }

    // WaveManagerがIdleになったらリザルトへ
    if (WaveManager.Instance.IsIdle) {
      state.SetState(State.Result);
      return;
    }
  }

  private void EnterResult()
  {
    UIManager.Instance.HUD.SkillSlots.Idle();
  }

  private void UpdateResult()
  {

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

    TimeSystem.OnDebug();
  }

#endif

}
