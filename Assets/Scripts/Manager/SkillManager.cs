using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// スキル管理者
/// スキル経験値やアクティブスキルを管理する
/// </summary>
public class SkillManager : SingletonMonoBehaviour<SkillManager>
{
  //============================================================================
  // Variables
  //============================================================================

  /// <summary>
  /// 一時的にストックしておく経験値
  /// </summary>
  private Dictionary<int, int> tmpExps = new Dictionary<int, int>();

  /// <summary>
  /// 獲得済の経験値
  /// </summary>
  private Dictionary<int, int> exps = new Dictionary<int, int>();

  /// <summary>
  /// 設定済のアクティブスキル
  /// </summary>
  private ISkill[] activeSkills = new ISkill[App.ACTIVE_SKILL_MAX];

  //============================================================================
  // Methods
  //============================================================================

  //----------------------------------------------------------------------------
  // Life Cycle
  //----------------------------------------------------------------------------

  protected override void MyAwake()
  {
    base.MyAwake();

    // スキルの種類の数だけ要素を追加
    MyEnum.ForEach<SkillId>(id => exps.Add((int)id, -1));

    // アクティブスキルを初期化
    for(int i = 0; i < App.ACTIVE_SKILL_MAX; ++i) {
      activeSkills[i] = null;
    }

    // 暫定: 通常弾に経験値をセット
    SetExp(SkillId.NormalBullet, 0);

    // 暫定: アクティブスキル[0]に通常弾をセット
    activeSkills[0] = MakeSkill(SkillId.NormalBullet);
  }

  //----------------------------------------------------------------------------
  // Public
  //----------------------------------------------------------------------------

  /// <summary>
  /// アクティブスキルを取得する
  /// </summary>
  public ISkill GetActiveSkill(int slotIndex)
  {
    return activeSkills[slotIndex];
  }

  /// <summary>
  /// アクティブスキルをセットする
  /// </summary>
  public void SetActiveSkill(int slotIndex, ISkill skill)
  {
    activeSkills[slotIndex] = skill;
  }

  /// <summary>
  /// スキル経験値をセットする
  /// </summary>
  public void SetExp(SkillId id, int value)
  {
    exps[(int)id] = value;
  }

  /// <summary>
  /// 経験値をストックする
  /// </summary>
  public void StockExp(SkillId id, int value)
  {
    if(tmpExps.TryGetValue((int)id, out int exp)) {
      tmpExps[(int)id] += value;
    } else {
      tmpExps.Add((int)id, value);
    }
  }

  /// <summary>
  /// スキル経験値を追加する
  /// </summary>
  public void AddExp(SkillId id, int value)
  {
    exps[(int)id] += value;

    // Active Skillに経験値をセット
    foreach (var skill in activeSkills)
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
    return exps[(int)id];
  }

  /// <summary>
  /// 一時的にストックしていた経験値を取得済にする
  /// </summary>
  public void FixExps()
  {
    foreach(var item in tmpExps) 
    {
      AddExp((SkillId)item.Key, item.Value);
    }

    tmpExps.Clear();
  }

  //----------------------------------------------------------------------------
  // For Me
  //----------------------------------------------------------------------------

  /// <summary>
  /// Skillインスタンスを生成する
  /// </summary>
  private Skill MakeSkill(SkillId id)
  {
    var entity = SkillMaster.FindById(id);

    var s = new Skill();
    s.Init(entity, GetExp(id));

    return s;
  }

#if _DEBUG
  //----------------------------------------------------------------------------
  // For Debug
  //----------------------------------------------------------------------------

  private int _tabIndex = 0;

  /// <summary>
  /// デバッグ用の基底メソッド
  /// </summary>
  public override void OnDebug()
  {
    GUILayout.Label("SkillManager");

    using (new GUILayout.HorizontalScope()) 
    {
      if (GUILayout.Button("Exp")) {
        _tabIndex = 0;
      }

      if (GUILayout.Button("ActiveSkills")) {
        _tabIndex = 1;
      }
    }

    switch(_tabIndex) {
      case 0: OnDebug_DrawExp(); break;
      case 1: OnDebug_DrawActiveSkill(); break;
      default: break;
    }
  }

  private void OnDebug_DrawExp()
  {
    using (new GUILayout.HorizontalScope()) {

      using (new GUILayout.VerticalScope()) {
        GUILayout.Box("Fixed");
        OnDebug_DrawExpList(exps);
      }

      using (new GUILayout.VerticalScope()) {
        GUILayout.Box("Stock");
        OnDebug_DrawExpList(tmpExps);
      }

    }

    if (GUILayout.Button("Fix")) {
      FixExps();
    }
  }

  private void OnDebug_DrawExpList(Dictionary<int, int> dic)
  {
    // 経験値リストをループ処理
    for (int i = 0, count = dic.Count; i < count; i++) {
      // 要素取得
      var item = dic.ElementAt(i);
      var name = ((SkillId)item.Key).ToString();

      using (new GUILayout.HorizontalScope()) {
        // Label
        GUILayout.Label(name);

        // TextField
        var value = GUILayout.TextField(item.Value.ToString(), GUILayout.Width(100));

        if (int.TryParse(value, out var result)) {
          dic[item.Key] = result;
        }
      }
    }
  }

  private void OnDebug_DrawActiveSkill()
  {
    using (new GUILayout.HorizontalScope()) {
      GUILayout.Box("Index", GUILayout.Width(60));
      GUILayout.Box("Lv", GUILayout.Width(30));
      GUILayout.Box("Name");
    }

    for(int i = 0; i < App.ACTIVE_SKILL_MAX; ++i) 
    {
      var skill = activeSkills[i];

      if (skill == null) continue;

      using (new GUILayout.HorizontalScope()) 
      {
        GUILayout.Box(i.ToString(), GUILayout.Width(60));
        GUILayout.Box((skill.Lv + 1).ToString(), GUILayout.Width(30));
        GUILayout.Box(skill.Id.ToString());
      }
    }
    
  }
#endif

}
