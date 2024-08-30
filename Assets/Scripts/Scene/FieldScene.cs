using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FieldScene : MonoBehaviour
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

  private void Awake()
  {
    state = new StateMachine<State>();

    state.Add(State.Idle);
    state.Add(State.SystemSetup, EnterSystemSetup, UpdateSystemSetup);
    state.Add(State.LevelLoading, EnterLevelLoading, UpdateLevelLoading);
    state.Add(State.Serach, EnterSearch, UpdateSearch);
    state.Add(State.Battle);
    state.Add(State.Result);
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

  //----------------------------------------------------------------------------
  // for Update
  //----------------------------------------------------------------------------
  private void EnterSystemSetup()
  {
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

  }

}
