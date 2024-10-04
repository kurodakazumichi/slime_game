using System;
using UnityEngine;

namespace MyGame.Old
{
  public class Player : MyMonoBehaviour, IActor
  {
    //============================================================================
    // Const
    //============================================================================

    const float SPEED = 6f;

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

    public Action<int, float> OnChangeHP { private get; set; } = null;

    //============================================================================
    // Methods
    //============================================================================

    //----------------------------------------------------------------------------
    // Public
    //----------------------------------------------------------------------------
    public DamageInfo TakeDamage(AttackInfo info)
    {
      if (state.StateKey != State.Usual) {
        return new DamageInfo(0f, DamageDetail.Undefined);
      }

      hp.Now -= info.Power;

      SyncWithHp();

      if (hp.IsEmpty) {
        state.SetState(State.Dead);
      }
      else {
        state.SetState(State.Invincible);
      }

      var result = new DamageInfo(info.Power);
      return result;
    }

    public void Respawn()
    {
      hp.Full();
      SyncWithHp();
      state.SetState(State.Idle);
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
      Collider = GetComponent<SphereCollider>();
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
      SyncWithHp();
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
      var fm = FieldManager.Instance;

      // FieldManagerがなければ移動制限が行われないだけ
      if (fm == null) {
        return;
      }

      // ロックされてなければ移動制限はしない
      if (!fm.HasBattleCircle) {
        return;
      }

      // BattleCircleの中心からPlayerに向かうベクトル
      var v = Position - fm.BattleCircleCenter;

      // BattleCircleの中心から離れすぎていたら、Circle内に戻す
      var radius = App.BATTLE_CIRCLE_RADIUS;

      if (!fm.IsInBattleCircle(Position)) {
        CachedTransform.position
          = fm.BattleCircleCenter + v.normalized * radius;
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
    // Other
    //----------------------------------------------------------------------------

    /// <summary>
    /// HPと同期をとる
    /// </summary>
    private void SyncWithHp()
    {
      // HPに合わせて色を設定
      spriteRenderer.color = Color.Lerp(Color.white, Color.red, 1f - hp.Rate);

      // HP変更時の処理を実行
      OnChangeHP?.Invoke((int)hp.Now, hp.Rate);
    }
  }
}