using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEditor;
using UnityEngine;

namespace MyGame.UI { 

  public class SkillSettingController
  {
    private SkillSetting ui;
    private SkillManager sm;
    private IconManager  im;

    private enum State
    {
      Idle,
      SelectSlot,
      SelectSkill,
    }

    private int selectedSlotIndex = 0;
    private int selectedSkillIndex = -1;

    private StateMachine<State> state = new ();

    private List<SkillId> skills {
      get 
      {
        var list = new List<SkillId> ();

        MyEnum.ForEach<SkillId>(id => 
        { 
          if (id == SkillId.Undefined) return;

          if (0 <= sm.GetExp(id)) {
            list.Add(id);
          }
        });

        return list;
      }
    }

    public void Init(SkillManager sm, IconManager im, SkillSetting ui)
    {
      this.sm = sm;
      this.im = im;
      this.ui = ui;

      state.Add(State.Idle);
      state.Add(State.SelectSlot, EnterSelectSlot, UpdateSelectSlot);
      state.Add(State.SelectSkill, EnterSelectSkill, UpdateSelectSkill);
      state.SetState(State.Idle);

      Close();
    }

    /// <summary>
    /// 選択中のアクティブスキルID
    /// </summary>
    private SkillId SelectedActiveSkillId {
      get {
        var skill = sm.GetActiveSkill(selectedSlotIndex);
        return (skill is null)? SkillId.Undefined : skill.Id;
      }
    }

    private SkillId SelectedSkillId 
    {
      get {
        var idx = selectedSkillIndex;

        if (0 <= idx && idx < skills.Count) {
          return skills[idx];
        } else {
          return SkillId.Undefined;
        }
      }
    }

    /// <summary>
    /// リセット、現在のアクティブスキルの内容を表示する。
    /// スロットは一番左を選択、スキルリストは未選択状態。
    /// </summary>
    public void Reset()
    {
      selectedSlotIndex = 0;
      selectedSkillIndex = -1;

      // スロットにアクティブスキルをセットする
      for(int i = 0; i < App.ACTIVE_SKILL_MAX; ++i) 
      {
        var skill = sm.GetActiveSkill(i);
        var sprite = GetSprite(skill);
        ui.SetSlot(i, sprite);
      }

      ui.SelectSlot(selectedSlotIndex);

      // 一番最初に登録されているアクティブスキルの詳細を表示
      SetDetail(SelectedActiveSkillId);

      // スキルリストを生成
      var skills = this.skills;
      ui.ClearSkillList();
      foreach (var id in skills) 
      {
        ui.AddSkill(im.Skill(id), sm.IsContainActiveSkill(id));
      }

      ui.SelectSkill(selectedSkillIndex);

      ui.FocusSkillSlots();
    }

    public void Open()
    {
      ui.SetActive(true);
      Reset();
      state.SetState(State.SelectSlot);
    }

    public void Close()
    {
      ui.SetActive(false);
      state.SetState(State.Idle);
    }

    private void SetDetail(SkillId id)
    {
      if (id == SkillId.Undefined){
        ui.Name = "未選択";
        ui.Lv = 0;
        ui.NextLvExp = 0;
        ui.Power = 0;
        ui.Impact = 0;
        ui.ChargeTime = 0;
        ui.PenetrableCount = 0;
        ui.AimType = SkillAimingType.None;
        ui.GrowthType = Growth.Undefined;
        return;
      }

      var config  = SkillMaster.FindById(id);
      var exp     = sm.GetExp(id);
      var lv      = SkillUtil.CalcLevelBy(config, exp);
      var crntExp = SkillUtil.GetNeedExp(config, lv);
      var nextExp = SkillUtil.GetNeedExp(config, lv+1);

      ui.Name            = config.Name;
      ui.Lv              = lv;
      ui.NextLvExp       = nextExp - exp;
      ui.ExpGaugeRate    = (float)(exp-crntExp)/(nextExp - crntExp);
      ui.Power           = SkillUtil.CalcPowerBy(config, lv);
      ui.Impact          = config.Impact;
      ui.ChargeTime      = SkillUtil.CalcRecastTimeBy(config, lv);
      ui.PenetrableCount = SkillUtil.CalcPenetrableCount(config, lv);
      ui.AimType         = config.Aiming;
      ui.GrowthType      = config.GrowthType;
      ui.Comment         = "";
    }

    public void Update()
    {
      state.Update();
    }

    //----------------------------------------------------------------------------
    // SelectSlot State

    private void EnterSelectSlot()
    {
      ui.FocusSkillSlots();
      ui.SelectSkill(-1);
      SetDetail(SelectedActiveSkillId);
    }

    private void UpdateSelectSlot()
    {
      var idx = selectedSlotIndex;
      var max = App.ACTIVE_SKILL_MAX;

      if (Input.GetKeyDown(KeyCode.A)) {
        state.SetState(State.SelectSkill);
        return;
      }
      
      if (Input.GetKeyDown(KeyCode.RightArrow)) {
        idx = (idx+1) % max;
      }

      if (Input.GetKeyDown(KeyCode.LeftArrow)) {
        idx = (max + idx - 1) % max; 
      }

      if (selectedSlotIndex != idx) {
        selectedSlotIndex = idx;
        ui.SelectSlot(selectedSlotIndex);
        SetDetail(SelectedActiveSkillId);
      }


    }

    //----------------------------------------------------------------------------
    // SelectSkill State

    private void EnterSelectSkill()
    {
      ui.FocusSkillList();
      selectedSkillIndex = 0;
      ui.SelectSkill(selectedSkillIndex);
      SetDetail(SelectedSkillId);
    }

    private void UpdateSelectSkill()
    {
      if (Input.GetKeyDown(KeyCode.Z)) {
        state.SetState(State.SelectSlot);
        return;
      }

      if (Input.GetKeyDown(KeyCode.A)) {
        sm.SetActiveSkill(selectedSlotIndex, SelectedSkillId);
        ui.SetSlot(selectedSlotIndex, im.Skill(SelectedSkillId));
        ui.MarkEqiupment(selectedSkillIndex, true);
        state.SetState(State.SelectSlot);
      }

      var idx = selectedSkillIndex;
      var max = skills.Count;

      if (Input.GetKeyDown(KeyCode.RightArrow)) {
        idx++;
        idx = (max <= idx)? 0 : idx;
      }

      if (Input.GetKeyDown(KeyCode.LeftArrow)) 
      {
        idx--;
        idx = (idx < 0)? max - 1 : idx;
      }

      if (selectedSkillIndex != idx) {
        selectedSkillIndex = idx;
        ui.SelectSkill(selectedSkillIndex);
        SetDetail(SelectedSkillId);
      }
    }

    private Sprite GetSprite(ISkill skill)
    {
      if (skill == null) return null;
      return im.Skill(skill.Id);
    }

  }

}