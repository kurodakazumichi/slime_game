using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// スキル経験値をもつアイテム、取得するとプレイヤーにスキル経験値が貯まる
/// </summary>
public class SkillItem : MyMonoBehaviour, ISkillItem
{
  //============================================================================
  // Enum
  //============================================================================
  private enum State { 
    Idle,
    Usual,
    Move,
  }

  //============================================================================
  // Variables
  //============================================================================

  /// <summary>
  /// ステートマシン
  /// </summary>
  private StateMachine<State> stateMachine = new();

  /// <summary>
  /// コライダー、プレイヤーとの衝突判定で参照する
  /// </summary>
  new private SphereCollider collider = null;

  /// <summary>
  /// スキル用のアイコンを設定するために参照する
  /// </summary>
  private SpriteRenderer spriteRenderer = null;

  /// <summary>
  /// アイテムの下から湧き出るパーティクル
  /// </summary>
  private ParticleSystem effect = null;

  /// <summary>
  /// 丸影
  /// </summary>
  private Shadow shadow = null;

  /// <summary>
  /// 汎用タイマー
  /// </summary>
  private float timer = 0;

  /// <summary>
  /// 汎用タイム
  /// </summary>
  private float time =0;

  /// <summary>
  /// 基準の位置
  /// </summary>
  private Vector3 origin = Vector3.zero;

  /// <summary>
  /// 目標地点
  /// </summary>
  private Vector3 target = Vector3.zero;

  //============================================================================
  // Properties
  //============================================================================
  private SkillId Id { get; set; } = SkillId.Undefined;

  private int Exp { get; set; } = 0;

  //============================================================================
  // Methods
  //============================================================================

  //----------------------------------------------------------------------------
  // Public
  //----------------------------------------------------------------------------
  /// <summary>
  /// 何も設定されていない状態にリセットする
  /// </summary>
  public void Reset()
  {
    Exp = 0;
    Id = SkillId.Undefined;

    effect.Stop();
    spriteRenderer.sprite = null;

    ReleaseShadow();

    stateMachine.SetState(State.Idle);
  }

  /// <summary>
  /// スキルアイテムのセットアップ
  /// </summary>
  public void Setup(SkillId id, int exp, Vector3 position)
  {
    Position = position;
    Id       = id;
    Exp      = exp;

    SetupIcon(id);
    SetupShadow();

    stateMachine.SetState(State.Usual);
  }

  /// <summary>
  /// アイテムを指定位置まで移動させる
  /// </summary>
  public void Move(Vector3 target, float time)
  {
    this.time   = time;
    this.target = target;

    stateMachine.SetState(State.Move);
  }

  //----------------------------------------------------------------------------
  // Life Cycle
  //----------------------------------------------------------------------------
  protected override void MyAwake()
  {
    // カメラに向き合う角度に設定
    CachedTransform.rotation = Quaternion.Euler(App.CAMERA_ANGLE_X, 0, 0);

    // 各種コンポーネントを取得
    collider       = GetComponent<SphereCollider>();
    spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    effect         = GetComponentInChildren<ParticleSystem>();

    // 状態をセットアップ
    stateMachine.Add(State.Idle);
    stateMachine.Add(State.Usual, EnterUsual, UpdateUsual, ExitUsual);
    stateMachine.Add(State.Move, EnterMove, UpdateMove, null);
    stateMachine.SetState(State.Idle);
  }

  private void Update()
  {
    stateMachine.Update();
  }

  private void LateUpdate()
  {
    if (stateMachine.StateKey == State.Idle) {
      return;
    }

    if (PlayerManager.Instance is null) {
      return;
    }

    var a = PlayerManager.Instance.Position;
    var b = collider.transform.position;
    var r = PlayerManager.Instance.Collider.radius + collider.radius;

    // プレイヤーと衝突したら効果を発動する
    if (CollisionUtil.IsCollideAxB(a, b, r)) {
      Activate();
    }
  }

  //----------------------------------------------------------------------------
  // State
  //----------------------------------------------------------------------------

  //----------------------------------------------------------------------------
  // Usual State

  private void EnterUsual()
  {
    timer  = 0;
    origin = spriteRenderer.transform.position;
  }

  private void UpdateUsual()
  {
    var y = Mathf.Sin(timer * 3f) * 0.1f;
    var p = origin;
    p.y += y;
    spriteRenderer.transform.position = p;
    timer += TimeSystem.Item.DeltaTime;
  }

  private void ExitUsual()
  {
    spriteRenderer.transform.position = origin;
  }

  //----------------------------------------------------------------------------
  // Usual Move

  private void EnterMove()
  {
    timer = 0f;
    origin = Position;
  }

  private void UpdateMove()
  {
    if (time <= 0) {
      Position = target;
      stateMachine.SetState(State.Usual);
      return;
    }

    var rate = timer / time;
    Position = Vector3.Lerp(origin, target, Mathf.Pow(rate, 2f));
    timer += TimeSystem.Item.DeltaTime;
  }

  //----------------------------------------------------------------------------
  // For this
  //----------------------------------------------------------------------------
  private void SetupShadow()
  {
    if (ShadowManager.Instance is null) {
      return;
    }

    shadow = ShadowManager.Instance.Get();
    shadow.SetOwner(this, collider.radius);
  }

  private void ReleaseShadow()
  {
    if (ShadowManager.Instance is null) {
      return;
    }

    ShadowManager.Instance.Release(shadow);
    shadow = null;
  }

  private void SetupIcon(SkillId id)
  {
    if (IconManager.Instance is null) {
      return;
    }

    spriteRenderer.sprite = IconManager.Instance.Skill(id);
  }

  /// <summary>
  /// アイテムの効果を発動する
  /// </summary>
  private void Activate()
  {
    SkillManager.Instance.AddExp(Id, Exp);
    HitTextManager.Instance.Get().ShowExp(Position, Exp);
    Reset();
    ItemManager.Instance.ReleaseSkillItem(this);
  }

}
