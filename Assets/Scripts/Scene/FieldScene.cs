using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyGame.Core.System;
using MyGame.Master;
using MyGame.System;

namespace MyGame.Scene
{
  public class FieldScene : MyMonoBehaviour
#if _DEBUG
  ,IDebugable
#endif
  {
    //=========================================================================
    // Enum
    //=========================================================================

    private enum State { 
      Idle,
      SystemSetup,
      Loading,
      Initialize,
      Searching,
    }

    //=========================================================================
    // Variables
    //=========================================================================
    private StateMachine<State> state = new();

    //-------------------------------------------------------------------------
    // Systems
    private PlayerSystem sPlayer = new();

    //=========================================================================
    // Methods
    //=========================================================================

    protected override void MyAwake()
    {
      base.MyAwake();

      state = new StateMachine<State>();

      state.Add(State.Idle);
      state.Add(State.SystemSetup, EnterSystemSetup, UpdateSystemSetup);
      state.Add(State.Loading, EnterLoading, UpdateLoading);
      state.Add(State.Initialize, EnterInitialize, UpdateInitialize);
      state.Add(State.Searching, EnterSearching, UpdateSearching);
      state.SetState(State.Idle);
    }

    private void Start()
    {
      state.SetState(State.SystemSetup);
    }

    void Update()
    {
      state.Update();

#if _DEBUG
      DebugSystem.Update();
#endif
    }

    private void LateUpdate()
    {
    }

#if _DEBUG
    private void OnGUI()
    {
      DebugSystem.OnGUI();
    }
#endif

    //-------------------------------------------------------------------------
    // for Setup

    private void EnterSystemSetup()
    {
      PlayerMaster.Init();
      EnemyMaster.Init();
      SkillMaster.Init();

      DebugSystem.Regist(this);
      DebugSystem.Regist(ResourceSystem.Debugger);
    }

    private void UpdateSystemSetup()
    {
      state.SetState(State.Loading);
    }

    //-------------------------------------------------------------------------
    // for Loading
    private void EnterLoading()
    {
      sPlayer.Load();
    }

    private void UpdateLoading()
    {
      if (ResourceSystem.IsLoading) return;

      state.SetState(State.Initialize);
    }

    //-------------------------------------------------------------------------
    // for Initialize

    private void EnterInitialize()
    {
      sPlayer.Init(PlayerMaster.Config, null, null);
      sPlayer.SetPlayable();

      CameraSystem.SetupTrackingCamera(Camera.main, sPlayer.PlayerTransform, App.CAMERA_OFFSET);
    }

    private void UpdateInitialize()
    {
      state.SetState(State.Searching);
    }

    //-------------------------------------------------------------------------
    // for Searching

    private void EnterSearching()
    {

    }

    private void UpdateSearching()
    {
      sPlayer.Update();
      CameraSystem.Update();
    }

#if _DEBUG
    public override void OnDebug()
    {
      GUILayout.Label($"State = {state.StateKey.ToString()}");
    }
#endif
  }
}