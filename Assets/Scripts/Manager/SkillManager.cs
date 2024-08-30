using System.Collections.Generic;
using UnityEngine;

public class SkillManager : SingletonMonoBehaviour<SkillManager>
{
  private Dictionary<int, int> _exp;
  private const int ACTIVE_SKILL_COUNT = 9;
  private Skill[] _activeSkills = new Skill[ACTIVE_SKILL_COUNT];

  protected override void MyAwake()
  {
    base.MyAwake();

    _exp = new Dictionary<int, int>();

    // スキルの種類の数だけ要素を追加
    MyEnum.ForEach<SkillId>(id => _exp.Add((int)id, -1));
    SetExp(SkillId.NormalBullet, 0);

    // アクティブスキルを初期化
    for(int i = 0; i < ACTIVE_SKILL_COUNT; ++i) {
      _activeSkills[i] = null;
    }

    // 暫定で通常弾スキルをセット
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
  /// スキル経験値をセットする
  /// </summary>
  public void SetExp(SkillId id, int value)
  {
    _exp[(int)id] = value;
  }

  /// <summary>
  /// スキル経験値を追加する
  /// </summary>
  public void AddExp(SkillId id, int value)
  {
    _exp[(int)id] += value;

    // Active Skillに経験値をセット
    foreach (var skill in _activeSkills)
    {
      if (skill != null && skill.Id == id) {
        skill.SetExp(GetExp(id));
      }
    }
  }

  /// <summary>
  /// スキル経験値を参照
  /// </summary>
  public int GetExp(SkillId id)
  {
    return _exp[(int)id];
  }
}
