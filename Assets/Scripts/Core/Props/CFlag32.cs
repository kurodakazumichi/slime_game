using System;

/// <summary>
/// �r�b�g�t���O(32bit��)
/// </summary>
public class Flag32 : IFormattable
{
  /// <summary>
  /// �t���O
  /// </summary>
  public uint Value { get; set; } = 0;

  /// <summary>
  /// �t���O���N���A
  /// </summary>
  public void Clear()
  {
    this.Value = 0;
  }

  /// <summary>
  /// �w�肳�ꂽ�t���O�ƈ�v���邩
  /// </summary>
  public bool Is(uint flags)
  {
    return Value == flags;
  }

  /// <summary>
  /// �w�肳�ꂽ�t���O�������Ă��邩
  /// </summary>
  public bool Has(uint flags)
  {
    if (Value == 0) return false;
    return (this.Value & flags) == flags;
  }

  /// <summary>
  /// �w�肳�ꂽ�t���O�̂����ꂩ�������Ă���
  /// </summary>
  public bool HasEither(uint flags)
  {
    if (Value == 0) return false;
    return (this.Value & flags) != 0;
  }

  //-------------------------------------------------------------------------
  // ���Z�q�I�[�o�[���[�h

  /// <summary>
  /// frags�𗧂Ă�̂� += �ł���悤�� operator +���I�[�o�[���[�h���Ă݂�
  /// </summary>
  public static Flag32 operator +(Flag32 self, uint flags)
  {
    self.Value |= flags;
    return self;
  }

  /// <summary>
  /// Frag�𗎂Ƃ��̂� -= �ł���悤�� operator -���I�[�o�[���[�h���Ă݂�
  /// </summary>
  public static Flag32 operator -(Flag32 self, uint flags)
  {
    self.Value &= ~flags;
    return self;
  }

  //-------------------------------------------------------------------------
  // IFormattable

  // ToString���������A������ɂ����2�i���\�L�ɂȂ�悤�ɂ��Ă���
  public string ToString(string format, IFormatProvider formatProvider)
  {
    return Convert.ToString(this.Value, 2);
  }
}

