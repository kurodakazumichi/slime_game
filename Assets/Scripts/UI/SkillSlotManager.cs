using UnityEngine;
using System.Collections.Generic;

public class SkillSlotManager : MonoBehaviour
{
  [SerializeField]
  private GameObject skillSlotPrefab;

  private List<SkillSlot> _slots;

  private void Awake()
  {
    _slots = new List<SkillSlot>();
  }

  private void Start()
  {
    var s = Instantiate(skillSlotPrefab).GetComponent<SkillSlot>();
    s.CacheRectTransform.position = new Vector3(-320f, 35f, 0);
    s.CacheRectTransform.SetParent(transform, false);
    s.SetSkill(SkillManager.Instance.GetSkill(0));
    //s.Charge();

    _slots.Add(s);
  }
}
