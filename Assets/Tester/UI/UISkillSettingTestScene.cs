using MyGame.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame.UiTest
{
  public class UISkillSettingTestScene : MonoBehaviour
  {
    public GameObject SkillIconPrefab;
    public SkillSetting UI;

    private SkillSettingController controller = new ();

    void Start()
    {
      DebugManager.Instance.Regist(ResourceManager.Instance);
      DebugManager.Instance.Regist(SkillManager.Instance);
      DebugManager.Instance.Regist(IconManager.Instance);

      // IconÇÉçÅ[ÉhÇµÇƒÇ®Ç≠
      IconManager.Instance.Load();

      MyEnum.ForEach<SkillId>(id => {
        if (id != SkillId.Undefined) {
          SkillManager.Instance.SetExp(id, 100);
        }
      });

      // SkillSettingControllerÇê∂ê¨
      controller.Init(SkillManager.Instance, IconManager.Instance, UI);
    }

    private void Update()
    {
      if (ResourceManager.Instance.IsLoading) return;

      controller.Update();

      if (Input.GetKeyDown(KeyCode.O)) {
        controller.Open();
      }

      if (Input.GetKeyDown(KeyCode.C)) {
        controller.Close();
      }
    }
  }
}

