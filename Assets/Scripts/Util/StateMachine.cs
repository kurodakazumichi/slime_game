using System;
using System.Collections.Generic;


public class StateMachine<T>
{
  /// <summary>
  /// �X�e�[�g
  /// </summary>
  private class State
  {
    private readonly Action enterAction;
    private readonly Action updateAction;
    private readonly Action exitAction;

    /// <summary>
    /// �R���X�g���N�^
    /// </summary>
    public State(Action enter = null, Action update = null, Action exit = null)
    {
      this.enterAction = enter ?? delegate { };
      this.updateAction = update ?? delegate { };
      this.exitAction = exit ?? delegate { };
    }

    /// <summary>
    /// �J�n����
    /// </summary>
    public void Enter()
    {
      this.enterAction();
    }

    /// <summary>
    /// �X�V����
    /// </summary>
    public void Update()
    {
      this.updateAction();
    }

    /// <summary>
    /// �I������
    /// </summary>
    public void Exit()
    {
      this.exitAction();
    }

  }

  /// <summary>
  /// �X�e�[�g�e�[�u��
  /// </summary>
  private Dictionary<T, State> table = new Dictionary<T, State>();

  /// <summary>
  /// ���݂̃X�e�[�g
  /// </summary>
  private State current;

  /// <summary>
  /// ���݂̃X�e�[�g�L�[
  /// </summary>
  private T currentKey;

  /// <summary>
  /// ���Z�b�g
  /// </summary>
  public void Reset()
  {
    this.current = null;
    this.currentKey = default;
  }

  /// <summary>
  /// �X�e�[�g��ǉ�
  /// </summary>
  public void Add(T key, Action enter = null, Action update = null, Action exit = null)
  {
    this.table[key] = new State(enter, update, exit);
  }

  /// <summary>
  /// �X�e�[�g��ݒ�
  /// </summary>
  public void SetState(T key)
  {
    if (this.current != null) {
      this.current.Exit();
    }

    this.currentKey = key;
    this.current = this.table[key];
    this.current.Enter();
  }

  /// <summary>
  /// ���݂̃X�e�[�g�̃L�[��Ԃ�
  /// </summary>
  public T StateKey => (this.currentKey);

  /// <summary>
  /// �X�e�[�g���X�V
  /// </summary>
  public void Update()
  {
    if (this.current == null) {
      return;
    }

    this.current.Update();
  }

  /// <summary>
  /// �S�ẴX�e�[�g���폜
  /// </summary>
  public void Clear()
  {
    this.table.Clear();
    this.current = null;
  }
}
