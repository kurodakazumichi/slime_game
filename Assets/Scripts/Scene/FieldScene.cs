using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyGame.Core.System;
using MyGame.Master;
using MyGame.System;

namespace MyGame.Scene
{
  public class FieldScene : MyMonoBehaviour
  {
    //=========================================================================
    // Enum
    //=========================================================================

    private enum State { 
      Idle,
      SystemSetup,
      Loading,
    }

    //=========================================================================
    // Variables
    //=========================================================================
    private StateMachine<State> state = new();

    //=========================================================================
    // Methods
    //=========================================================================

    protected override void MyAwake()
    {
      base.MyAwake();

      state = new StateMachine<State>();

      state.Add(State.Idle);
      state.Add(State.SystemSetup, EnterSystemSetup, UpdateSystemSetup);
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

      DebugSystem.Regist(ResourceSystem.Debugger);
    }

    private void UpdateSystemSetup()
    {
      state.SetState(State.Idle);
    }
  }
}