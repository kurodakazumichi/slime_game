using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MyGame.UI
{
  public class SkillSetting : MyUIBehaviour
  {
    [SerializeField]
    private List<SkillIcon> slots;

    [SerializeField]
    private RectTransform SkillListContent;

    [SerializeField]
    private Text nameText;

    [SerializeField]
    private Text lvText;

    [SerializeField]
    private Text nextLvText;

    [SerializeField]
    private SimpleGauge expGauge;

    [SerializeField]
    private Text powerText;

    [SerializeField]
    private Text impactText;

    [SerializeField]
    private Text chargeText;

    [SerializeField]
    private Text penetrableText;

    [SerializeField]
    private Text aimText;

    [SerializeField]
    private Text growthText;

    [SerializeField]
    private Text commentText;

    [SerializeField]
    private GameObject skillIconPrefab;

    [SerializeField]
    private GameObject skillSlotCover;

    [SerializeField]
    private GameObject skillListCover;


    private List<SkillIcon> skills = new List<SkillIcon>();

    public void SelectSlot(int index)
    {
      for (int i = 0; i < slots.Count; i++) {
        slots[i].IsSelected = i == index;
      }
    }

    public void SelectSkill(int index)
    {
      for(int i = 0; i < skills.Count; ++i) {
        skills[i].IsSelected = i == index;
      }
    }

    public void MarkEqiupment(int index, bool equipped)
    {
      skills[index].IsEquipped = equipped;
    }

    public void SetSlot(int index, Sprite sprite)
    {
      slots[index].SetSprite(sprite);
    }

    public string Name {
      set { nameText.text = value; }
    }

    public int Lv {
      set { lvText.text = $"Lv {value}"; }
    }

    public int NextLvExp {
      set { nextLvText.text = $"ŽŸ‚ÌLv‚Ü‚Å {value}"; }
    }

    public float ExpGaugeRate {
      set { expGauge.Rate = value; }
    }

    public int Power {
      set { powerText.text = value.ToString(); }
    }

    public float Impact {
      set { impactText.text = value.ToString("F1"); }
    }

    public float ChargeTime {
      set { chargeText.text = $"{value.ToString("F1")} •b"; }
    }

    public int PenetrableCount {
      set { penetrableText.text = $"{value.ToString()} ‰ñ"; }
    }

    public SkillAimingType AimType {
      set {
        aimText.text = SkillUtil.GetSkillAimingTypeName(value);
      }
    }

    public Growth GrowthType {
      set {
        growthText.text = SkillUtil.GetGrowthName(value);
      }
    }

    public string Comment {
      set { commentText.text = value; }
    }

    public void AddSkill(Sprite sprite, bool equipped)
    {
      var icon = Instantiate(skillIconPrefab, SkillListContent)
                 .GetComponent<SkillIcon>();
      icon.SetSprite(sprite);
      icon.IsEquipped = equipped;
      skills.Add(icon);
    }

    public void ClearSkillList()
    {
      foreach (var skill in skills)
      {
        Destroy(skill.gameObject);
      }
      skills.Clear();
    }

    public void FocusSkillSlots()
    {
      skillSlotCover.SetActive(false);
      skillListCover.SetActive(true);
    }

    public void FocusSkillList()
    {
      skillSlotCover.SetActive(true);
      skillListCover.SetActive(false);
    }
  }
}
