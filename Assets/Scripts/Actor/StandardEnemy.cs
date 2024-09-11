using UnityEngine;

public class StandardEnemy : Enemy<StandardEnemy.State>
{
  private const float BREAKTIME_AFTER_ATTACK = 0.4f;

  [SerializeField]
  private float Speed = 1f;

  /// <summary>
  /// このタイマーに時間が設定されていたら動きを止める
  /// </summary>
  private float moveStopTimer = 0f;

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
    // 動かないタイム
    if (0f < moveStopTimer) {
      velocity = Vector3.zero;
      return;
    }

    // まずPlayerに向かう速度ベクトルをセットする
    velocity = (PM.Position - Position).normalized * Speed;

    // Boidsアルゴリズムによって補正をする
    velocity = EnemyManager.Instance.Boids(this, collider.radius) * Speed;
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
      Kill();
    }

    if (status.IsDead) 
    {
      state.SetState(State.Idle);
      Die();
      return;
    }

    if (0f < moveStopTimer) {
      moveStopTimer -= TimeSystem.Enemy.DeltaTime;
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
      moveStopTimer = BREAKTIME_AFTER_ATTACK;
    }
  }


}