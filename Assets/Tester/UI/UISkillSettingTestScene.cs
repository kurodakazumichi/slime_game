using UnityEngine;
using MyGame.UI;
using MyGame.Core.System;

namespace MyGame.UiTest
{
  public class UISkillSettingTestScene : MonoBehaviour
  {
    public GameObject SkillIconPrefab;
    public SkillSetting UI;

    private SkillSettingController controller = new ();

    void Start()
    {
      DebugSystem.Regist(ResourceManager.Instance);
      DebugSystem.Regist(SkillManager.Instance);
      DebugSystem.Regist(IconManager.Instance);

      // Iconをロードしておく
      IconManager.Instance.Load();

      MyEnum.ForEach<SkillId>(id => {
        if (id != SkillId.Undefined) {
          SkillManager.Instance.SetExp(id, 100);
        }
      });

      // SkillSettingControllerを生成
      controller.Init(SkillManager.Instance, IconManager.Instance, UI);
    }

    private void Update()
    {
      DebugSystem.Update();
      if (ResourceManager.Instance.IsLoading) return;

      controller.Update();

      if (Input.GetKeyDown(KeyCode.O)) {
        controller.Open();
      }

      if (Input.GetKeyDown(KeyCode.C)) {
        controller.Close();
      }
    }

    private void OnGUI()
    {
      DebugSystem.OnGUI();
    }
  }
}

