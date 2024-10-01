using UnityEngine;
using MyGame.Core.Props;
using System;

namespace MyGame.ViewLogic
{
  public class PlayerLogic
  {
    //=========================================================================
    // Const
    //=========================================================================
    const float BATTLE_SPEED = 3f;
    const float FIELD_SPEED  = 6f;

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
    private MyGame.View.Player target;

    //=========================================================================
    // Variables
    //=========================================================================

    /// <summary>
    /// �X�e�[�g�}�V��
    /// </summary>
    private StateMachine<State> state = new();

    /// <summary>
    /// ���x
    /// </summary>
    private Vector3 velocity = Vector3.zero;

    /// <summary>
    /// HP
    /// </summary>
    private RangedFloat hp = new(0);

    /// <summary>
    /// ���G�^�C�}�[
    /// </summary>
    private Timer invincibleTimer = new();

    /// <summary>
    /// �ړ����x
    /// </summary>
    private float speed = 0;

    /// <summary>
    /// HP���ω������Ƃ��ɌĂ΂��R�[���o�b�N
    /// </summary>
    private Action<float, float> onChangeHP = null;

    //=========================================================================
    // Methods
    //=========================================================================

    /// <summary>
    /// ������
    /// </summary>
    public void Init(MyGame.View.Player target, Action<float, float> onChangeHP)
    {
      this.target     = target;
      this.onChangeHP = onChangeHP;

      hp.Init(10f);
      OnChangeHP();

      state.Add(State.Idle);
      state.Add(State.Usual, EnterStateUsual, UpdateStateUsual);

      // �R�[���o�b�N
      invincibleTimer.OnStart = OnStartInvincible;
      invincibleTimer.OnStop  = OnExitInvincible;

      Reset();
    }

    /// <summary>
    /// �X�V
    /// </summary>
    public void Update()
    {
      state.Update(); 
    }

    /// <summary>
    /// �L�������̐؂�ւ�
    /// </summary>
    public void SetActive(bool flag)
    {
      target.SetActive(flag);
    }

    /// <summary>
    /// ���Z�b�g(������Ԃɂ���)
    /// </summary>
    public void Reset()
    {
      hp.Full();
      OnChangeHP();
      UpdateColor();
      FieldMode();
      state.SetState(State.Idle);
    }

    /// <summary>
    /// ����\�ɂ���
    /// </summary>
    public void Playable()
    {
      state.SetState(State.Usual);
    }

    /// <summary>
    /// �t�B�[���h���[�h�ɂ���
    /// </summary>
    public void FieldMode()
    {
      speed = FIELD_SPEED;
    }

    /// <summary>
    /// �o�g�����[�h�ɂ���
    /// </summary>
    public void BattleMode()
    {
      speed = BATTLE_SPEED;
    }

    /// <summary>
    /// �_���[�W���󂯂�
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
      // RestrictMovement();
      // SyncCameraPosition();
    }

    //-------------------------------------------------------------------------
    // ���x
    //-------------------------------------------------------------------------

    /// <summary>
    /// ���x���v�Z����
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
    /// ���͂��瑬�x���擾����
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
    // �v���C���[�̐F�F���G��Ԃ̂Ƃ��͔������A�܂��̗͂�0�ɋ߂Â��قǐԂ��Ȃ�
    //-------------------------------------------------------------------------

    /// <summary>
    /// ��ԂɑΉ������J���[���擾
    /// </summary>
    private Color GetColor()
    {
      var c = Color.Lerp(Color.white, Color.red, 1f- hp.Rate);
      c.a = (invincibleTimer.IsRunning)? 0.3f : 1f;
      return c;
    }

    /// <summary>
    /// �F���X�V
    /// </summary>
    private void UpdateColor()
    {
      target.Renderer.color = GetColor();
    }

    //-------------------------------------------------------------------------
    // ���G�̏���
    //-------------------------------------------------------------------------

    /// <summary>
    /// ���G�J�n���̏���
    /// </summary>
    private void OnStartInvincible()
    {
      UpdateColor();
    }

    /// <summary>
    /// ���G�I�����̏���
    /// </summary>
    private void OnExitInvincible()
    {
      UpdateColor();
    }

    //-------------------------------------------------------------------------
    // �X�e�[�^�X
    //-------------------------------------------------------------------------

    /// <summary>
    /// HP�ɕω����������Ƃ��ɌĂ�
    /// </summary>
    private void OnChangeHP()
    {
      onChangeHP?.Invoke(hp.Now, hp.Rate);
    }

  }
}