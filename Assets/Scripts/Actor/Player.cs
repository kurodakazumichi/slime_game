using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class Player : MyMonoBehaviour, IActor
{
  //============================================================================
  // Const
  //============================================================================

  const float SPEED = 5f;

  //============================================================================
  // Enum
  //============================================================================

  private enum State
  {
    Idle,
    Usual,
    Invincible,
    Dead,
  }

  //============================================================================
  // Variables
  //============================================================================
  private SpriteRenderer spriteRenderer;

  private StateMachine<State> state = new();
  private float timer = 0;

  private Vector3 velocity = Vector3.zero;

  private Vector3 targetVelocity = Vector3.zero;

  private RangedFloat hp = new(0);
  
  //============================================================================
  // Properities
  //============================================================================

  public SphereCollider Collider { get; private set; }

  public bool IsDead {
    get { return hp.IsEmpty; }
  }

  //============================================================================
  // Methods
  //============================================================================

  //----------------------------------------------------------------------------
  // Public
  //----------------------------------------------------------------------------
  public void TakeDamage(AttackInfo info)
  {
    if (state.StateKey != State.Usual) {
      return;
    }

    hp.Now -= info.Power;

    SyncHpToHudHpGauge();

    if (hp.IsEmpty) {
      state.SetState(State.Dead);
    }
    else {
      state.SetState(State.Invincible);
    }
  }

  public void Respawn()
  {
    hp.Full();
    state.SetState(State.Idle);
    SyncHpToHudHpGauge();
  }

  public void SetStateUsual()
  {
    state.SetState(State.Usual);
  }

  //----------------------------------------------------------------------------
  // Lief Cycle
  //----------------------------------------------------------------------------

  protected override void MyAwake()
  {
    Collider       = GetComponent<SphereCollider>();
    spriteRenderer = GetComponentInChildren<SpriteRenderer>();

    hp.Init(10f);

    state.Add(State.Idle);
    state.Add(State.Usual, EnterStateUsual, UpdateStateUsual);
    state.Add(State.Invincible, EnterStateInvisible, UpdateStateInvisible);
    state.Add(State.Dead);
    state.SetState(State.Idle);
  }

  // Start is called once before the first execution of Update after the MonoBehaviour is created
  void Start()
  {
    SyncHpToHudHpGauge();
  }

  private void Update()
  {
    state.Update();
  }

  //----------------------------------------------------------------------------
  // State
  //----------------------------------------------------------------------------

  private void EnterStateUsual()
  {
    spriteRenderer.material.color = Color.white;
  }

  private void UpdateStateUsual()
  {
    velocity = CalcVelocity();

    transform.position += velocity * TimeSystem.Player.DeltaTime;

    RestrictMovement();

    SyncCameraPosition();
  }

  private void EnterStateInvisible()
  {
    timer = 0.2f;
    var c = spriteRenderer.material.color;
    c.a = 0.3f;
    spriteRenderer.material.color = c;
  }

  private void UpdateStateInvisible()
  {
    velocity = CalcVelocity();

    transform.position += velocity * TimeSystem.Player.DeltaTime;

    if (timer < 0) {
      state.SetState(State.Usual);
      return;
    }

    timer -= TimeSystem.Player.DeltaTime;

    SyncCameraPosition();
  }

  //----------------------------------------------------------------------------
  // 移動
  //----------------------------------------------------------------------------

  private void RestrictMovement()
  {
    // ロックされてなければ移動制限はしない
    if (!FieldManager.Instance.IsLockArea) {
      return;
    }

    // BattleLocationからPlayerに向かうベクトル
    var v = Position - FieldManager.Instance.BattlePosition;

    var radius = App.BATTLE_CIRCLE_RADIUS;

    // BattleCircleの中心から離れすぎていたら、Circle内に戻す
    if (radius * radius <= v.sqrMagnitude) 
    {
      CachedTransform.position 
        = FieldManager.Instance.BattlePosition + v.normalized * radius;
    }
    
  }

  private void SyncCameraPosition()
  {
    var p = transform.position;
    p.y = 14f;
    p.z -= 14f;
    
    Camera.main.transform.position = p;
  }

  private Vector3 CalcVelocity()
  {
    var delta = TimeSystem.Player.DeltaTime;
    var duration = 1f;
    var target = GetInputVelocity();
    var rate = duration * delta;
    return Vector3.Lerp(velocity, target, Mathf.Pow(rate, 0.8f));
  }

  private Vector3 GetInputVelocity()
  {
    Vector3 v = Vector3.zero;

    if (Input.GetKey(KeyCode.LeftArrow)) {
      v.x = -SPEED;
    }

    if (Input.GetKey(KeyCode.RightArrow)) {
      v.x = SPEED;
    }

    if (Input.GetKey(KeyCode.UpArrow)) {
      v.z = SPEED;
    }

    if (Input.GetKey(KeyCode.DownArrow)) {
      v.z = -SPEED;
    }

    return v;
  }

  //----------------------------------------------------------------------------
  // UI
  //----------------------------------------------------------------------------

  private void SyncHpToHudHpGauge()
  {
    UIManager.Instance.HUD.HpGauge.Set(hp.Now, hp.Rate);
  }
}
