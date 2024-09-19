using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if _DEBUG

namespace ItemTester
{ 
  public class ItemTestScene : MyMonoBehaviour
  {
    private void Start()
    {
      ItemManager.Instance.Load();
      IconManager.Instance.Load();
    }


    private void Update()
    {
      if (ResourceManager.Instance.IsLoading) {
        return;
      }

      if (Input.GetKeyDown(KeyCode.Space)) {
        var item = ItemManager.Instance.GetSkillItem();
        item.Setup(SkillId.NormalBullet1, 0, MyVector3.Random(10f, 0f, 10f));
      }

      if (Input.GetKeyDown(KeyCode.C)) {
        ItemManager.Instance.Clear();
      }

      if (Input.GetKeyDown(KeyCode.X)) {
        ItemManager.Instance.Collect(Vector3.zero);
      }
    }
  }
}
#endif