using UnityEngine;

public class StandardEnemy : Enemy<StandardEnemy.State>
{
  //============================================================================
  // Const
  //============================================================================

  //============================================================================
  // Enum
  //============================================================================
  public enum State
  {
    Idle,
    Usual,
    Dead,
  }

  //============================================================================
  // Variables
  //============================================================================

  //============================================================================
  // Methods
  //============================================================================

  //----------------------------------------------------------------------------
  // Public
  //----------------------------------------------------------------------------
  public override void Run()
  {
    CastShadow();

    base.StateMachine.SetState(State.Usual);
    velocity = (PlayerManager.Instance.Position - Position).normalized * status.Speed;
  }

  //----------------------------------------------------------------------------
  // Life Cycle
  //----------------------------------------------------------------------------
  protected override void MyAwake()
  {
    base.MyAwake();

    base.StateMachine.Add(State.Idle, EnterIdle);
    base.StateMachine.Add(State.Usual, EnterUsual, UpdateUsual);
    base.StateMachine.Add(State.Dead, EnterDead, UpdateDead, ExitDead);
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
      restTimer.Start(POST_ATTACK_REST_TIME);
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
    StopTimer();
  }

  //----------------------------------------------------------------------------
  // For Usual
  private void EnterUsual()
  {
    IsCollidable = true;
    IsVisible = true;
    Alpha = 1f;
  }

  private void UpdateUsual()
  {
    if (IsTerminationRequested) {
      Kill();
    }

    if (status.IsDead) {
      base.StateMachine.SetState(State.Dead);
      return;
    }

    UpdateTimer();

    CachedTransform.position += velocity * TimeSystem.Enemy.DeltaTime;
  }

  private void EnterDead()
  {
    timer = 0.25f; 
    Alpha = 1f;
  }

  private void UpdateDead()
  {
    if (timer <= 0f) {
      StateMachine.SetState(State.Idle);
      return;
    }

    Alpha = timer;
    timer -= TimeSystem.Enemy.DeltaTime;
  }

  private void ExitDead()
  {
    Die();
  }

  //----------------------------------------------------------------------------
  // 動きに関するもの
  //----------------------------------------------------------------------------

  /// <summary>
  /// 速度を再計算
  /// </summary>
  private void ReCalcVelocity()
  {
    // ノックバックがある場合はそちらに従う
    if (knockbackTimer.IsRunning) {
      velocity = knockbackVelocity;
      return;
    }

    // 休憩中は動かない
    if (restTimer.IsRunning) {
      velocity = Vector3.zero;
      return;
    }

    // Playerに向かう速度ベクトルを生成
    var toPlayer = (PM.Position - Position).normalized * status.Speed;

    // バトルサークル外だったらとりあえずプレイヤーに向かう
    if (!FieldManager.Instance.IsInBattleCircle(Position)) {
      velocity = toPlayer;
      return;
    }
    
    // Boidsアルゴリズムによって補正をする
    velocity = EnemyManager.Instance.Boids(toPlayer, this, Collider.radius*2f) * status.Speed;
  }

}