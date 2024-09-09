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
    MyEnum.ForEach<SkillId>((id) => 
    {
      if (id == SkillId.Undefined) {
        return;
      }

      exps.Add((int)id, -1);
    });

    // アクティブスキルを初期化
    for(int i = 0; i < App.ACTIVE_SKILL_MAX; ++i) {
      activeSkills[i] = null;
    }

    // 暫定: 通常弾に経験値をセット
    SetExp(SkillId.NormalBullet1, 0);

    // 暫定: アクティブスキル[0]に通常弾をセット
    activeSkills[0] = MakeSkill(SkillId.NormalBullet1);
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

  public void SetActiveSkill(int slotIndex, SkillId id)
  {
    SetActiveSkill(slotIndex, MakeSkill(id));
  }

  /// <summary>
  /// スロットのセットを試みる、スロットに空があれば最初に見つかったスロットにセットされる。
  /// </summary>
  /// <returns>スキルがセットされたスロットのIndex、セットできなかったら-1</returns>
  public int TrySetActiveSkill(SkillId id)
  {
    for(int i = 0; i < App.ACTIVE_SKILL_MAX; ++i) 
    {
      if (activeSkills[i] is null) {
        SetActiveSkill(i, id);
        return i;
      }
    }

    return -1;
  }

  public void RemoveActiveSkill(int slotIndex)
  {
    activeSkills[slotIndex] = null;
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

  /// <summary>
  /// デバッグ用の基底メソッド
  /// </summary>
  public override void OnDebug()
  {
    SkillManagerDebugger.OnGUI();
  }

  public static class SkillManagerDebugger
  {
    private static int tabIndex = 0;

    public static void OnGUI()
    {
      using (new GUILayout.HorizontalScope()) {
        if (GUILayout.Button("Exp")) {
          tabIndex = 0;
        }

        if (GUILayout.Button("ActiveSkills")) {
          tabIndex = 1;
        }

        if (GUILayout.Button("Simulator")) {
          tabIndex = 2;
        }
      }

      switch (tabIndex) {
        case 0: DrawExp(); break;
        case 1: DrawActiveSkill(); break;
        case 2: DrawSimulater(); break;
        default: break;
      }
    }

    private static void DrawExp()
    {
      using (new GUILayout.VerticalScope()) {
        GUILayout.Box("Fixed", GUILayout.Width(300));
        DrawExpList(Instance.exps);
      }

      using (new GUILayout.VerticalScope()) {
        GUILayout.Box("Stock");
        DrawExpStockList(Instance.tmpExps);
      }

      if (GUILayout.Button("Fix")) {
        Instance.FixExps();
      }
    }

    private static void DrawExpList(Dictionary<int, int> dic)
    {
      // 経験値リストをループ処理
      for (int i = 0, count = dic.Count; i < count; i++) {
        // 要素取得
        var item = dic.ElementAt(i);
        var name = ((SkillId)item.Key).ToString();

        using (new GUILayout.HorizontalScope(GUILayout.Width(300))) 
        {
          // Label
          GUILayout.Label(name);

          // TextField
          var value = GUILayout.TextField(item.Value.ToString(), GUILayout.Width(100));

          if (int.TryParse(value, out var result)) {
            dic[item.Key] = result;
          }

          if (GUILayout.Button("Simulate")) {
            simulationSkillId = name;
            tabIndex = 2;
          }

          if (GUILayout.Button("Activate")) {
            Instance.TrySetActiveSkill((SkillId)item.Key);
          }
        }
      }
    }

    private static void DrawExpStockList(Dictionary<int, int> dic)
    {
      // 経験値リストをループ処理
      for (int i = 0, count = dic.Count; i < count; i++) {
        // 要素取得
        var item = dic.ElementAt(i);
        var name = ((SkillId)item.Key).ToString();

        using (new GUILayout.HorizontalScope(GUILayout.Width(300))) {
          // Label
          GUILayout.Label(name);
          GUILayout.Label(item.Value.ToString());
        }
      }
    }


    private static void DrawActiveSkill()
    {
      using (new GUILayout.HorizontalScope()) {
        GUILayout.Box("Index", GUILayout.Width(60));
        GUILayout.Box("Lv", GUILayout.Width(30));
        GUILayout.Box("Name");
      }

      for (int i = 0; i < App.ACTIVE_SKILL_MAX; ++i) {
        var skill = Instance.activeSkills[i];

        if (skill == null) continue;

        using (new GUILayout.HorizontalScope()) {
          GUILayout.Box(i.ToString(), GUILayout.Width(60));
          GUILayout.Box((skill.Lv + 1).ToString(), GUILayout.Width(30));
          GUILayout.Box(skill.Id.ToString());
          if(GUILayout.Button("Remove")) {
            Instance.RemoveActiveSkill(i);
          }
        }
      }
    }

    private static string simulationSkillId = "";
    private static int simulationLv = 0;
    private static Skill simulationSkill = null;

    private static void DrawSimulater()
    {
      using (new GUILayout.HorizontalScope()) 
      {
        GUILayout.Label("SkillId");
        simulationSkillId = GUILayout.TextField(simulationSkillId);
        GUILayout.Label($"Lv {simulationLv}");
        simulationLv = (int)GUILayout.HorizontalSlider((float)simulationLv, 0f, App.ACTIVE_SKILL_MAX);

        if (GUILayout.Button("Simulate")) 
        {
          var id = MyEnum.Parse<SkillId>(simulationSkillId);
          var lv = Mathf.Clamp(simulationLv, 0, App.ACTIVE_SKILL_MAX);

          simulationSkill = new Skill();
          simulationSkill.Init(SkillMaster.FindById(id), 0);
          simulationSkill.SetLv(lv);
        }
      }

      if (simulationSkill is not null) {
        simulationSkill.OnDebug();
      }
    }

  }
#endif

}
