using System;
using UnityEngine;
using MyGame.Core.Props;
using MyGame.Master;
using MyGame.System;
using MyGame.View;

namespace MyGame.Presenter
{
  public class PlayerPresenter : IPresenter
  {
    //=========================================================================
    // Enum
    //=========================================================================

    private enum State
    {
      Idle,
      Usual,
    }

    //=========================================================================
    // Dependencies
    //=========================================================================
    /// <summary>
    /// Playerの設定値
    /// </summary>
    private IPlayerEntity config;

    /// <summary>
    /// 操作対象
    /// </summary>
    private Player target;

    /// <summary>
    /// FieldSystem
    /// </summary>
    private IFieldSystem fs;

    //=========================================================================
    // Variables
    //=========================================================================

    /// <summary>
    /// ステートマシン
    /// </summary>
    private StateMachine<State> state = new();

    /// <summary>
    /// 速度
    /// </summary>
    private Vector3 velocity = Vector3.zero;

    /// <summary>
    /// HP
    /// </summary>
    private RangedFloat hp = new(0);

    /// <summary>
    /// 無敵タイマー
    /// </summary>
    private Timer invincibleTimer = new();

    /// <summary>
    /// 移動速度
    /// </summary>
    private float speed = 0;

    /// <summary>
    /// HPが変化したときに呼ばれるコールバック
    /// </summary>
    private Action<float, float> onChangeHP = null;

    //=========================================================================
    // Properties
    //=========================================================================

    /// <summary>
    /// PlayerのViewオブジェクト
    /// </summary>
    public View.Player View => target;

    /// <summary>
    /// 死亡フラグ
    /// </summary>
    public bool IsDead => hp.IsEmpty;

    //=========================================================================
    // Methods
    //=========================================================================

    /// <summary>
    /// 初期化
    /// </summary>
    public void Init(
      IPlayerEntity config,
      Player target, 
      IFieldSystem fs, 
      Action<float, float> onChangeHP
    )
    {
      this.config     = config;
      this.target     = target;
      this.fs         = fs;
      this.onChangeHP = onChangeHP;
      
      hp.Init(config.InitialHP);
      OnChangeHP();

      state.Add(State.Idle);
      state.Add(State.Usual, EnterStateUsual, UpdateStateUsual);

      // コールバック
      invincibleTimer.OnStart = OnStartInvincible;
      invincibleTimer.OnStop  = OnExitInvincible;

      Reset();
    }

    /// <summary>
    /// 更新
    /// </summary>
    public void Update()
    {
      state.Update(); 
    }

    /// <summary>
    /// 有効無効の切り替え
    /// </summary>
    public void SetActive(bool flag)
    {
      target.SetActive(flag);
    }

    /// <summary>
    /// リセット(初期状態にする)
    /// </summary>
    public void Reset()
    {
      hp.Full();
      OnChangeHP();
      UpdateColor();
      SearchMode();
      state.SetState(State.Idle);
    }

    /// <summary>
    /// 操作可能にする
    /// </summary>
    public void Playable()
    {
      state.SetState(State.Usual);
    }

    /// <summary>
    /// フィールドモードにする
    /// </summary>
    public void SearchMode()
    {
      speed = config.SpeedInSearch;
    }

    /// <summary>
    /// バトルモードにする
    /// </summary>
    public void BattleMode()
    {
      speed = config.SpeedInBattle;
    }

    /// <summary>
    /// ダメージを受ける
    /// </summary>
    public DamageInfo TakeDamage(AttackInfo info)
    {
      if (state.StateKey != State.Usual) {
        return new DamageInfo(0f, DamageDetail.Undefined);
      }

      hp.Now -= info.Power;
      OnChangeHP();

      if (!hp.IsEmpty) {
        invincibleTimer.Start(0.2f);
      }

      UpdateColor();

      var result = new DamageInfo(info.Power);
      return result;
    }

    //----------------------------------------------------------------------------
    // State
    //----------------------------------------------------------------------------

    //----------------------------------------------------------------------------
    // Usual
    private void EnterStateUsual()
    {
      UpdateColor();
    }

    private void UpdateStateUsual()
    {
      var dt = TimeSystem.Player.DeltaTime;

      velocity = CalcVelocity();
      target.Position += velocity * dt;

      invincibleTimer.Update(dt);
      RestrictMovement();
    }

    //-------------------------------------------------------------------------
    // 移動
    //-------------------------------------------------------------------------

    private void RestrictMovement()
    {
      // FieldSystemがなければ何もしない
      if (fs is null) return;

      // ロックされてなければ移動制限はしない
      if (!fs.HasBattleCircle) {
        return;
      }

      // BattleCircleの中心からPlayerに向かうベクトル
      var v = target.Position - fs.BattleCircleCenter;

      // BattleCircleの中心から離れすぎていたら、Circle内に戻す
      var radius = App.BATTLE_CIRCLE_RADIUS;

      if (!fs.IsInBattleCircle(target.Position)) 
      {
        target.Position = fs.BattleCircleCenter + v.normalized * radius;
      } 
    }
    //-------------------------------------------------------------------------
    // 速度
    //-------------------------------------------------------------------------

    /// <summary>
    /// 速度を計算する
    /// </summary>
    private Vector3 CalcVelocity()
    {
      var delta = TimeSystem.Player.DeltaTime;
      var duration = 1f;
      var target = GetInputVelocity();
      var rate = duration * delta;
      return Vector3.Lerp(velocity, target, Mathf.Pow(rate, 0.8f));
    }

    /// <summary>
    /// 入力から速度を取得する
    /// </summary>
    /// <returns></returns>
    private Vector3 GetInputVelocity()
    {
      Vector3 v = Vector3.zero;

      if (Input.GetKey(KeyCode.LeftArrow)) {
        v.x = -speed;
      }

      if (Input.GetKey(KeyCode.RightArrow)) {
        v.x = speed;
      }

      if (Input.GetKey(KeyCode.UpArrow)) {
        v.z = speed;
      }

      if (Input.GetKey(KeyCode.DownArrow)) {
        v.z = -speed;
      }

      return v;
    }

    //-------------------------------------------------------------------------
    // プレイヤーの色：無敵状態のときは半透明、また体力が0に近づくほど赤くなる
    //-------------------------------------------------------------------------

    /// <summary>
    /// 状態に対応したカラーを取得
    /// </summary>
    private Color GetColor()
    {
      var c = Color.Lerp(Color.white, Color.red, 1f- hp.Rate);
      c.a = (invincibleTimer.IsRunning)? 0.3f : 1f;
      return c;
    }

    /// <summary>
    /// 色を更新
    /// </summary>
    private void UpdateColor()
    {
      target.Renderer.color = GetColor();
    }

    //-------------------------------------------------------------------------
    // 無敵の処理
    //-------------------------------------------------------------------------

    /// <summary>
    /// 無敵開始時の処理
    /// </summary>
    private void OnStartInvincible()
    {
      UpdateColor();
    }

    /// <summary>
    /// 無敵終了時の処理
    /// </summary>
    private void OnExitInvincible()
    {
      UpdateColor();
    }

    //-------------------------------------------------------------------------
    // ステータス
    //-------------------------------------------------------------------------

    /// <summary>
    /// HPに変化があったときに呼ぶ
    /// </summary>
    private void OnChangeHP()
    {
      onChangeHP?.Invoke(hp.Now, hp.Rate);
    }

  }
}