using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame.UI
{
  public class SkillSetting : MyUIBehaviour
  {
    [SerializeField]
    private List<SkillIcon> slots;

    public void SelectSlot(int index)
    {
      for (int i = 0; i < slots.Count; i++) {
        slots[i].IsSelected = i == index;
      }
    }
  }

}
