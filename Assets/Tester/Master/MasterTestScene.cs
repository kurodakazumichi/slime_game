using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyGame.Master;

namespace MyGame.Tester
{
  public class MasterTestScene : MonoBehaviour
  {
    // Start is called before the first frame update
    void Start()
    {
      EnemyMaster.Init();
      SkillMaster.Init();
    }
  }

}