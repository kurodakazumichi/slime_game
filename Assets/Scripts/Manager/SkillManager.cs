using System.Collections.Generic;
using UnityEngine;

public class SkillManager : SingletonMonoBehaviour<SkillManager>
{
  private Dictionary<int, int> _exp;
  private const int ACTIVE_SKILL_COUNT = 9;
  private Skill[] _activeSkills = new Skill[ACTIVE_SKILL_COUNT];

  protected override void MyAwake()
  {
    _exp = new Dictionary<int, int>();

    // �X�L���̎�ނ̐������v�f��ǉ�
    MyEnum.ForEach<SkillId>(id => _exp.Add((int)id, -1));
    SetExp(SkillId.NormalBullet, 0);

    // �A�N�e�B�u�X�L����������
    for(int i = 0; i < ACTIVE_SKILL_COUNT; ++i) {
      _activeSkills[i] = null;
    }

    // �b��Œʏ�e�X�L�����Z�b�g
    var s = new Skill();

    var entity = SkillData.FindById(SkillId.NormalBullet);
    s.Setup(entity);
    s.SetExp(GetExp(SkillId.NormalBullet));

    _activeSkills[0] = s;

  }

  public ISkill GetSkill(int slotNo)
  {
    return _activeSkills[slotNo];
  }

  /// <summary>
  /// �X�L���o���l���Z�b�g����
  /// </summary>
  public void SetExp(SkillId id, int value)
  {
    _exp[(int)id] = value;
  }

  /// <summary>
  /// �X�L���o���l��ǉ�����
  /// </summary>
  public void AddExp(SkillId id, int value)
  {
    _exp[(int)id] += value;

    // Active Skill�Ɍo���l���Z�b�g
    foreach (var skill in _activeSkills)
    {
      if (skill != null && skill.Id == id) {
        skill.SetExp(GetExp(id));
      }
    }
  }

  /// <summary>
  /// �X�L���o���l���Q��
  /// </summary>
  public int GetExp(SkillId id)
  {
    return _exp[(int)id];
  }
}
