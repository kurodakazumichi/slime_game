using UnityEngine;

public class StandardEnemy : Enemy<StandardEnemy.State>
{
  //============================================================================
  // Const
  //============================================================================

  /// <summary>
  /// 攻撃後の休憩時間
  /// </summary>
  private const float BREAKTIME_AFTER_ATTACK = 0.4f;

  //============================================================================
  // Enum
  //============================================================================
  public enum State
  {
    Idle,
    Usual
  }

  //============================================================================
  // Variables
  //============================================================================
  /// <summary>
  /// このタイマーに時間が設定されていたら動きを止める
  /// </summary>
  private float moveStopTimer = 0f;

  //============================================================================
  // Methods
  //============================================================================

  //----------------------------------------------------------------------------
  // Public
  //----------------------------------------------------------------------------
  public override void Run()
  {
    base.StateMachine.SetState(State.Usual);
  }

  //----------------------------------------------------------------------------
  // Life Cycle
  //----------------------------------------------------------------------------
  protected override void MyAwake()
  {
    base.MyAwake();

    base.StateMachine.Add(State.Idle, EnterIdle);
    base.StateMachine.Add(State.Usual, EnterUsual, UpdateUsual);
    base.StateMachine.SetState(State.Idle);
  }

  private void LateUpdate()
  {
    if (!IsCollidable) {
      return;
    }

    ReCalcVelocity();

    // Playerと接触した
    if (IsIntersectedWithPlayer) {
      PlayerManager.Instance.AttackPlayer(AttackInfo);
      moveStopTimer = BREAKTIME_AFTER_ATTACK;
    }
  }

  //----------------------------------------------------------------------------
  // State
  //----------------------------------------------------------------------------

  //----------------------------------------------------------------------------
  // For Idle
  private void EnterIdle()
  {
    IsCollidable = false;
    IsVisible = false;
    KnockbackVelocity = Vector3.zero;
    knockbackTimer.Stop();
  }

  //----------------------------------------------------------------------------
  // For Usual
  private void EnterUsual()
  {
    IsCollidable = true;
    IsVisible = true;
  }

  private void UpdateUsual()
  {
    if (IsTerminationRequested) {
      Kill();
    }

    if (status.IsDead) {
      base.StateMachine.SetState(State.Idle);
      Die();
      return;
    }

    if (0f < moveStopTimer) {
      moveStopTimer -= TimeSystem.Enemy.DeltaTime;
    }

    knockbackTimer.Update(TimeSystem.Enemy.DeltaTime);

    // ノックバック速度を更新
    if (knockbackTimer.IsRunning) {
      var rate = knockbackTimer.Rate;
      KnockbackVelocity = Vector3.Lerp(KnockbackVelocity, Vector3.zero, rate);
    }

    CachedTransform.position += velocity * TimeSystem.Enemy.DeltaTime;
  }

  //----------------------------------------------------------------------------
  // 動きにかんするもの
  //----------------------------------------------------------------------------

  /// <summary>
  /// 速度を再計算
  /// </summary>
  private void ReCalcVelocity()
  {
    // 動かないタイム
    if (0f < moveStopTimer) {
      velocity = Vector3.zero;
      return;
    }

    // ノックバックがある場合はそちらに従う
    if (knockbackTimer.IsRunning) {
      velocity = KnockbackVelocity;
      return;
    }

    // まずPlayerに向かう速度ベクトルをセットする
    velocity = (PM.Position - Position).normalized * status.Speed;

    // Boidsアルゴリズムによって補正をする
    velocity = EnemyManager.Instance.Boids(this, Collider.radius) * status.Speed;
  }

}