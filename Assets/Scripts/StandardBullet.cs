using UnityEngine;

public interface IBullet
{
  SkillId Id { get; }
  bool IsIdle { get; }
  void Terminate();
  void Fire(Vector3 position, Actor target, ISkill skill);
  void Attack(Actor actor);
  GameObject gameObject { get; }
  SphereCollider collider { get; }
  Transform CachedTransform { get; }
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

  //============================================================================
  // Methods
  //============================================================================

  //----------------------------------------------------------------------------
  // Public
  //----------------------------------------------------------------------------

  public void Fire(Vector3 position, Actor target, ISkill skill)
  {
    Id = skill.Id;

    CachedTransform.position = position;

    var v = Quaternion.AngleAxis(Random.Range(0, 360f), Vector3.up) * Vector3.forward;

    if (target != null) {
      v = (target.CachedTransform.position - position).normalized;
    }

    this.velocity = v;
    status.Init(skill.Power, (uint)Attribute.Non);
    state.SetState(State.Usual);
  }

  public void Terminate()
  {
    if (IsIdle) {
      return;
    }
    isTerminating = true;
  }

  public void Attack(Actor target)
  {
    if (isTerminating) {
      return;
    }

    target.TakeDamage(status);
    isTerminating = true;
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


}
