using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// �ǂݎ���pLimitedFloat
/// </summary>
public interface IRangedFloat
{
  float Now { get; }
  float Max { get; }
  float Rate { get; }
  bool IsFull { get; }
  bool IsEmpty { get; }
}

/// <summary>
/// �͈͕t�����������_
/// </summary>
public class RangedFloat : IRangedFloat
{
  /// <summary>
  /// �R���X�g���N�^
  /// </summary>
  public RangedFloat(float value) {
    max = now = value;
  }

  //-------------------------------------------------------------------------
  // Member

  /// <summary>
  /// ���݂̒l
  /// </summary>
  private float now;

  /// <summary>
  /// �ő�l
  /// </summary>
  private float max;

  //-------------------------------------------------------------------------
  // Public Properity

  /// <summary>
  /// ���݂̒l
  /// </summary>
  public float Now {
    get { return this.now; }

    set {
      this.now = Mathf.Max(0, Mathf.Min(value, this.max));
    }
  }

  /// <summary>
  /// �ő�l
  /// </summary>
  public float Max {
    get { return this.max; }

    set {
      this.max = Mathf.Max(1, value);
      this.now = Mathf.Min(this.now, this.max);
    }
  }

  /// <summary>
  /// ����
  /// </summary>
  public float Diff => Max - Now;

  /// <summary>
  /// ����
  /// </summary>
  public float Rate => (this.now / this.max);

  /// <summary>
  /// ���^��
  /// </summary>
  public bool IsFull => (this.now == this.max);

  /// <summary>
  /// �����
  /// </summary>
  public bool IsEmpty => (this.now == 0);

  //-------------------------------------------------------------------------
  // Public Method

  /// <summary>
  /// �Z�b�g�A�b�v
  /// </summary>
  public void Init(float now, float max)
  {
    Max = max;
    Now = now;
  }

  /// <summary>
  /// �����ς��ɂȂ�
  /// </summary>
  public void Full()
  {
    Now = Max;
  }

  /// <summary>
  /// ��ɂȂ�
  /// </summary>
  public void Empty()
  {
    Now = 0;
  }
}
