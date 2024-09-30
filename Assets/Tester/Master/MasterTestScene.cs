using UnityEngine;
using MyGame.Master;
using MyGame.Core.System;

namespace MyGame.Tester
{
  public class MasterTestScene : MonoBehaviour
#if _DEBUG
  ,IDebugable
#endif
  {
    // Start is called before the first frame update
    void Start()
    {
      EnemyMaster.Init();
      SkillMaster.Init();

      DebugSystem.Regist(this);
    }

    void Update()
    {
      DebugSystem.Update();
    }

#if _DEBUG
    void OnGUI()
    {
      DebugSystem.OnGUI();
    }

    public void OnDebug()
    {
      GUILayout.Label("MasterTestScene");
    }
#endif
  }
}