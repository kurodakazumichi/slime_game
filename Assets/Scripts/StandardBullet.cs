using UnityEngine;

public interface IBullet
{
  bool IsIdle { get; }
  void Terminate();
  void Fire(ISkill skill);
  void Attack(IEnemy enemy);
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

  public void Fire(ISkill skill)
  {
    var p = PlayerManager.Instance.PlayerVisualPosition;
    transform.position = p;

    var e = EnemyManager.Instance.FindNearestEnemy(p);

    var v = Quaternion.AngleAxis(Random.Range(0, 360f), Vector3.up) * Vector3.forward;

    if (e != null) {
      v = (e.transform.position - p).normalized;
    }

    this.velocity = v;

    status.Power = skill.Power;
    state.SetState(State.Usual);
  }

  public void Terminate()
  {
    if (IsIdle) {
      return;
    }
    isTerminating = true;
  }

  public void Attack(IEnemy enemy)
  {
    enemy.TakeDamage(status);
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
    state.Add(State.Usual, EnterUsual, UpdateUsual);
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

    var r = transform.rotation;
    transform.rotation = r * Quaternion.AngleAxis(720f * TimeSystem.DeltaTime, Vector3.forward);

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
