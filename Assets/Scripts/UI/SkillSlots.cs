using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// UI �����̃X�L���X���b�g�𓝊��Ǘ�����N���X
/// </summary>
public class SkillSlots : MyMonoBehaviour
{
  //============================================================================
  // Variables
  //============================================================================

  /// <summary>
  /// �X�L���X���b�gPrefab
  /// </summary>
  [SerializeField]
  private GameObject skillSlotPrefab;

  /// <summary>
  /// �X�L���X���b�g���X�g
  /// </summary>
  private List<SkillSlot> slots;

  protected override void MyAwake()
  {
    slots = new List<SkillSlot>();

    for(int i = 0; i < App.ACTIVE_SKILL_MAX; ++i) 
    {
      var slot = Instantiate(skillSlotPrefab).GetComponent<SkillSlot>();
      slot.CachedTransform.position = new Vector3(
        Mathf.Lerp(-800f, 800f, i / 9.0f), 0, 0
      );
      slot.CachedRectTransform.SetParent(transform, false);
      slot.SetActive(false);
      slots.Add(slot);
    }
  }

  public void SetSkill(int index, ISkill skill)
  {
    slots[index].SetSkill(skill);
  }

  public void Run()
  {
    foreach (var slot in slots) {
      slot.Charge();
    }
  }

  public void Run(int index)
  {
    slots[index].Charge();
  }

  public void Stop()
  {
    foreach (var slot in slots) {
      slot.Idle();
    }
  }
}
