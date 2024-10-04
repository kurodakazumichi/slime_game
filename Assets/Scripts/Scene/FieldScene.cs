using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyGame.Core.System;
using MyGame.Master;
using MyGame.System;
using MyGame.Presenter.Manager;

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
    // Managers
    private PlayerManager mPlayer = new();
    private CameraManager mCamera = new();

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
      mPlayer.Load();
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
      mPlayer.Init(null, null);
      mPlayer.SetPlayable();

      mCamera.SetupTrackingCamera(Camera.main, mPlayer.PlayerTransform, App.CAMERA_OFFSET);
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
      mPlayer.Update();
      mCamera.Update();
    }

#if _DEBUG
    public override void OnDebug()
    {
      GUILayout.Label($"State = {state.StateKey.ToString()}");
    }
#endif
  }
}