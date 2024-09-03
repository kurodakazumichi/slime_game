/// <summary>
/// ReadOnly��SkillEntity�C���^�[�t�F�[�X
/// </summary>
public interface ISkillEntityRO
{
  SkillId Id { get; }

  int MaxExp { get; }
  float FirstRecastTime { get; }
  float LastRecastTime { get; }
  int FirstPower { get; }
  int LastPower { get; }
  int Attr { get; }
  string Name { get; }
}

/// <summary>
/// Skill Entity
/// </summary>
public class SkillEntity : ISkillEntityRO
{
  public SkillEntity(SkillId id, int exp, float fr, float fl, int fp, int lp, int attr, string name)
  {
    Id              = id;
    MaxExp          = exp;
    FirstRecastTime = fr;
    LastRecastTime  = fl;
    FirstPower      = fp;
    LastPower       = lp;
    Attr            = attr;
    Name            = name;
  }

  /// <summary>
  /// Skill ID
  /// </summary>
  public SkillId Id { get; set; }

  /// <summary>
  /// LvMax�ɕK�v�Ȍo���l
  /// </summary>
  public int MaxExp { get; set; }

  /// <summary>
  /// Lv Min���̃��L���X�g�^�C��
  /// </summary>
  public float FirstRecastTime { get; set; }

  /// <summary>
  /// Lv Max���̃��L���X�g�^�C��
  /// </summary>
  public float LastRecastTime { get; set; }

  /// <summary>
  /// Lv Min���̃p���[
  /// </summary>
  public int FirstPower { get; set; }

  /// <summary>
  /// Lv Max���̃p���[
  /// </summary>
  public int LastPower { get; set; }

  /// <summary>
  /// ����
  /// </summary>
  public int Attr { get; set; }

  /// <summary>
  /// ����
  /// </summary>
  public string Name { get; set; }
}

