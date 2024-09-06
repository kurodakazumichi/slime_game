using UnityEngine;

public interface IBullet
{
  SkillId Id { get; }
  bool IsIdle { get; }
  
  void Fire(Vector3 position, IActor target, ISkill skill, BulletOwner ownerType);
  void Terminate();
  void Attack(IActor actor);
  
  void Intersect();
  GameObject gameObject { get; }
}

/// <summary>
/// 通常弾のスクリプト
/// </summary>
public class StandardBullet : MyMonoBehaviour, IBullet
{
  //============================================================================
  // Enum
  //============================================================================

  /// <summary>
  /// 状態
  /// </summary>
  private enum State
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
  new public SphereCollider collider {  get; private set; }

  /// <summary>
  /// スプライトレンダラー
  /// </summary>
  private SpriteRenderer spriteRenderer { get; set; }

  /// <summary>
  /// ステートマシン
  /// </summary>
  private StateMachine<State> state;

  /// <summary>
  /// 速度
  /// </summary>
  private Vector3 velocity;
  private AttackStatus status = new AttackStatus();

  /// <summary>
  /// 終了フラグ
  /// </summary>
  private bool isTerminating = false;

  //============================================================================
  // Properities
  //============================================================================

  public SkillId Id { get; private set; }

  public bool IsIdle {
    get { return state.StateKey == State.Idle; }
  }

  /// <summary>
  /// 表示制御
  /// </summary>
  private bool isVisible 
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
  private bool isCollidable 
  {
    get { 
      return collider.enabled;
    }
    set {
      collider.enabled = value;
    }
  }

  /// <summary>
  /// 弾丸のOwnerタイプを設定する
  /// </summary>
  private BulletOwner owner {
    get {
      return (layer == Layer.EnemyBullet)? BulletOwner.Enemy : BulletOwner.Player;
    }
    set {
      layer = (value == BulletOwner.Enemy)? Layer.EnemyBullet : Layer.PlayerBullet;
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
  public void Fire(Vector3 position, IActor target, ISkill skill, BulletOwner owner)
  {
    Id = skill.Id;
    this.owner = owner;

    CachedTransform.position = position;

    var v = Quaternion.AngleAxis(Random.Range(0, 360f), Vector3.up) * Vector3.forward;

    if (target != null) {
      v = (target.CachedTransform.position - position).normalized;
    }

    this.velocity = v;

    status.Init(skill.Power, skill.Attributes);
    state.SetState(State.Usual);
  }

  /// <summary>
  /// 終了する
  /// </summary>
  public void Terminate()
  {
    if (IsIdle) {
      return;
    }
    isTerminating = true;
  }

  /// <summary>
  /// targetを攻撃する
  /// </summary>
  public void Attack(IActor target)
  {
    if (isTerminating) {
      return;
    }

    target.TakeDamage(status);
    isTerminating = true;
  }

  /// <summary>
  /// 衝突判定
  /// </summary>
  public void Intersect()
  {
    // Playerの弾丸なら敵との衝突判定を行う
    if (owner is BulletOwner.Player) {
      IntersectEnemies();
    } 
    
    // Enemyの弾丸の場合、Intersectが呼ばれる=Playerに接触なのでPlayerに攻撃をする
    else {
      Attack(PlayerManager.Instance.Player);
    }
  }

  //----------------------------------------------------------------------------
  // Life Cycle
  //----------------------------------------------------------------------------

  protected override void MyAwake()
  {
    collider       = GetComponent<SphereCollider>();
    spriteRenderer = GetComponentInChildren<SpriteRenderer>();

    state = new StateMachine<State>();

    state.Add(State.Idle, EnterIdle);
    state.Add(State.Usual, EnterUsual, UpdateUsual, ExitUrual);
    state.SetState(State.Idle);
  }

  // Update is called once per frame
  void Update()
  {
    state.Update();
  }

  //----------------------------------------------------------------------------
  // for Update
  //----------------------------------------------------------------------------

  //----------------------------------------------------------------------------
  // for Idle State

  private void EnterIdle()
  {
    isVisible    = false;
    isCollidable = false;
  }

  //----------------------------------------------------------------------------
  // for Usual State

  private void EnterUsual()
  {
    isTerminating = false;
    isVisible     = true;
    isCollidable  = true;
  }

  private void UpdateUsual()
  {
    var p = transform.position;
    p += velocity * TimeSystem.DeltaTime;
    transform.position = p;

    if (isTerminating) {
      state.SetState(State.Idle);
      return;
    }
  }

  private void ExitUrual()
  {
    BulletManager.Instance.Release(this);
  }

  //----------------------------------------------------------------------------
  // for Me
  //----------------------------------------------------------------------------

  /// <summary>
  /// 敵との衝突判定
  /// </summary>
  private void IntersectEnemies()
  {
    var enemies = Physics.OverlapSphere(
        CachedTransform.position,
        collider.radius,
        LayerMask.GetMask(LayerName.Enemy)
      );

    foreach (var enemy in enemies) {
      var e = enemy.GetComponent<IEnemy>();
      Attack(e);
    }
  }

}
