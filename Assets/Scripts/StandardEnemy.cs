using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandardEnemy : Enemy<StandardEnemy.State>
{
  [SerializeField]
  private float Speed = 1f;

  public enum State
  {
    Idle,
    Usual
  }

  public override void Run()
  {
    state.SetState(State.Usual);
  }

  protected override void MyAwake()
  {
    base.MyAwake();

    state.Add(State.Idle, EnterIdle);
    state.Add(State.Usual, EnterUsual, UpdateUsual);
    state.SetState(State.Idle);
  }

  //----------------------------------------------------------------------------
  // For Idle
  //----------------------------------------------------------------------------
  private void EnterIdle()
  {
    isCollidable = false;
    isVisible    = false;
  }

  //----------------------------------------------------------------------------
  // For Usual
  //----------------------------------------------------------------------------
  private void EnterUsual()
  {
    isCollidable = true;
    isVisible    = true;
  }

  private void UpdateUsual()
  {
    if (isTerminationRequested) {
      hp.Empty();
    }

    if (hp.IsEmpty) 
    {
      state.SetState(State.Idle);
      Die();
      return;
    }

    var v = PM.PlayerVisualPosition - CachedTransform.position;
    CachedTransform.position += (v.normalized * Speed) * TimeSystem.Enemy.DeltaTime; 
  }

  private void LateUpdate()
  {
    if (!isCollidable) {
      return;
    }

    // Žb’è
    var d1 = (PlayerManager.Instance.PlayerVisualPosition - transform.position).sqrMagnitude;
    var d2 = collider.radius * collider.radius;

    if (d1 < d2) {
      PlayerManager.Instance.takeDamage(1f);
    }
  }


}