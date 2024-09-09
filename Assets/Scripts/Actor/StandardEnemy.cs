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

  private void ReCalcVelocity()
  {
    var target = (PM.Position - Position).normalized;
    velocity = EnemyManager.Instance.Boids(this, 1f, target) * Speed;
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
    velocity = (PM.Position - Position).normalized * Speed;
    isCollidable = true;
    isVisible    = true;
  }

  private void UpdateUsual()
  {
    if (isTerminationRequested) {
      Kill();
    }

    if (status.IsDead) 
    {
      state.SetState(State.Idle);
      Die();
      return;
    }

    CachedTransform.position += velocity * TimeSystem.Enemy.DeltaTime; 
  }



  private void LateUpdate()
  {
    if (!isCollidable) {
      return;
    }

    ReCalcVelocity();

    // 暫定
    var d1 = (PlayerManager.Instance.Position - CachedTransform.position).sqrMagnitude;
    var d2 = collider.radius * collider.radius;

    if (d1 < d2) {
      PlayerManager.Instance.AttackPlayer(attackInfo);
    }
  }


}