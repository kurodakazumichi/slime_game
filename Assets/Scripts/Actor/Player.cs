﻿using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class Player : MyMonoBehaviour, IActor
{
  const float SPEED = 5f;

  private enum State
  {
    Idle,
    Usual,
    Invincible,
    Dead,
  }

  private float timer = 0;
  private Vector3 velocity = Vector3.zero;
  private Vector3 targetVelocity = Vector3.zero;

  private RangedFloat _hp;

  private SpriteRenderer spriteRenderer;

  new public SphereCollider collider { get; private set; }

  public bool IsDead {
    get { return _hp.IsEmpty; }
  }

  private StateMachine<State> state = new StateMachine<State>();

  public void TakeDamage(AttackInfo info)
  {
    if (state.StateKey != State.Usual) {
      return;
    }

    _hp.Now -= info.Power;

    SyncHpToHudHpGauge();

    if (_hp.IsEmpty) {
      state.SetState(State.Dead);
    }
    else {
      state.SetState(State.Invincible);
    }
  }
  public void Respawn()
  {
    _hp.Full();
    state.SetState(State.Idle);
    SyncHpToHudHpGauge();
  }

  public void SetStateUsual()
  {
    state.SetState(State.Usual);
  }

  protected override void MyAwake()
  {
    collider = GetComponent<SphereCollider>();
    spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    _hp = new RangedFloat(10f);

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

  private void EnterStateUsual()
  {
    spriteRenderer.material.color = Color.white;
  }

  private void UpdateStateUsual()
  {
    targetVelocity = GetInputVelocity();

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
    Vector3 v = GetInputVelocity();

    targetVelocity = v;

    velocity = Vector3.Lerp(velocity, targetVelocity, 0.01f);

    transform.position += velocity * TimeSystem.Player.DeltaTime;

    if (timer < 0) {
      state.SetState(State.Usual);
      return;
    }

    timer -= TimeSystem.Player.DeltaTime;

    SyncCameraPosition();
  }


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


  private void SyncHpToHudHpGauge()
  {
    UIManager.Instance.HUD.HpGauge.Set(_hp.Now, _hp.Rate);
  }

  private Vector3 CalcVelocity()
  {
    var delta = TimeSystem.Player.DeltaTime;
    var duration = 1f;
    var target = GetInputVelocity();
    var rate = duration * delta;
    return Vector3.Slerp(velocity, target, Mathf.Pow(rate, 0.8f));
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
}
