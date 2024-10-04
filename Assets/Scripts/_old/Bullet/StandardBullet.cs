using System.Threading;
using UnityEngine;

/// <summary>
/// 通常弾のスクリプト
/// </summary>
public class StandardBullet : Bullet<StandardBullet.State>
{
  //============================================================================
  // Enum
  //============================================================================

  /// <summary>
  /// 状態
  /// </summary>
  public enum State
  {
    Idle,
    Usual,
  }

  //============================================================================
  // Variables
  //============================================================================

  /// <summary>
  /// コライダー
  /// </summary>
  new private SphereCollider collider = null;

  /// <summary>
  /// スプライトレンダラー
  /// </summary>
  private SpriteRenderer spriteRenderer = null;

  //============================================================================
  // Properities
  //============================================================================

  public override bool IsIdle {
    get { return StateMachine.StateKey == State.Idle; }
  }

  /// <summary>
  /// 表示制御
  /// </summary>
  protected override bool IsVisible 
  {
    get {
      return spriteRenderer.enabled;
    }

    set {
      spriteRenderer.enabled = value;
    }
  }

  /// <summary>
  /// 衝突制御
  /// </summary>
  protected override bool IsCollidable 
  {
    get { 
      return collider.enabled;
    }
    set {
      collider.enabled = value;
    }
  }

  //============================================================================
  // Methods
  //============================================================================

  //----------------------------------------------------------------------------
  // Public
  //----------------------------------------------------------------------------

  /// <summary>
  /// 発射する
  /// </summary>
  public override void Fire(BulletFireInfo info)
  {
    base.Fire(info);

    if (ShadowManager.Instance != null) {
      shadow = ShadowManager.Instance.Get();
      shadow.SetOwner(this, collider.radius);
    }

    if (Target != null) 
    {
      direction      = (Target.Position - info.Position).normalized;
    }

    StateMachine.SetState(State.Usual);
  }

  //----------------------------------------------------------------------------
  // Life Cycle
  //----------------------------------------------------------------------------

  protected override void MyAwake()
  {
    base.MyAwake();

    collider       = GetComponent<SphereCollider>();
    spriteRenderer = GetComponentInChildren<SpriteRenderer>();

    StateMachine.Add(State.Idle, EnterIdle);
    StateMachine.Add(State.Usual, EnterUsual, UpdateUsual, ExitUrual);
    StateMachine.SetState(State.Idle);
  }

  //----------------------------------------------------------------------------
  // for Idle State

  private void EnterIdle()
  {
    IsVisible    = false;
    IsCollidable = false;
  }

  //----------------------------------------------------------------------------
  // for Usual State

  private void EnterUsual()
  {
    isTerminating = false;
    IsVisible     = true;
    IsCollidable  = true;
    timer         = 0f;
  }

  private void UpdateUsual()
  {
    // 終了 or 寿命を迎えたらIdleへ
    if (isTerminating || IsLifeOver(timer)) {
      StateMachine.SetState(State.Idle);
      return;
    }

    // 方向を更新
    direction = CalcDirection(timer);

    // スピードを更新
    velocity = direction * CalcSpeed(timer) * TimeSystem.Bullet.DeltaTime;
    CachedTransform.position  += velocity;

    if (SyncDirectionAndRotation) {
      CachedTransform.rotation = Quaternion.LookRotation(direction, Vector3.up);
    }
    
    timer += TimeSystem.Bullet.DeltaTime;
  }

  private void ExitUrual()
  {
    BulletManager.Instance.Release(this);

    if (ShadowManager.Instance != null) {
      ShadowManager.Instance.Release(shadow);
      shadow = null;
    }
  }

  //----------------------------------------------------------------------------
  // 衝突判定
  //----------------------------------------------------------------------------
  /// <summary>
  /// 衝突判定
  /// </summary>
  public override void Intersect()
  {
    // Playerの弾丸なら敵との衝突判定を行う
    if (Owner is BulletOwner.Player) {
      IntersectEnemies();
    }

    // Enemyの弾丸の場合、Intersectが呼ばれる=Playerに接触なのでPlayerに攻撃をする
    else {
      Attack(PlayerManager.Instance.Player);
    }
  }

  /// <summary>
  /// 敵との衝突判定
  /// </summary>
  private void IntersectEnemies()
  {
    var actors = Physics.OverlapSphere(
        CachedTransform.position,
        collider.radius * MyVector3.LargestCompnent(CachedTransform.localScale),
        LayerMask.GetMask(LayerName.Enemy)
      );

    foreach (var actor in actors) {
      var a = actor.GetComponent<IActor>();
      Attack(a);
    }
  }

}
