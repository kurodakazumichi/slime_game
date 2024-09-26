using MyGame.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame.UiTest
{
  public class UISkillSettingTestScene : MonoBehaviour
  {
    public SkillSetting UI;

    private void Update()
    {
      if (Input.GetKeyDown(KeyCode.Alpha1)) {
        UI.SelectSlot(0);
      }
      if (Input.GetKeyDown(KeyCode.Alpha2)) {
        UI.SelectSlot(1);
      }
      if (Input.GetKeyDown(KeyCode.Alpha3)) {
        UI.SelectSlot(2);
      }
      if (Input.GetKeyDown(KeyCode.Alpha4)) {
        UI.SelectSlot(3);
      }
      if (Input.GetKeyDown(KeyCode.Alpha5)) {
        UI.SelectSlot(4);
      }
    }
  }
}

