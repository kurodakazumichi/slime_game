using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if _DEBUG

namespace Tester
{
  public class SkillTester : MonoBehaviour
  {
    public string SkillId;
    public GameObject TargetObject;
    private IActor target;

    // Start is called before the first frame update
    void Start()
    {
      DebugManager.Instance.Regist(SkillManager.Instance);
      DebugManager.Instance.Regist(BulletManager.Instance);
      DebugManager.Instance.Regist(ResourceManager.Instance);
      
      BulletManager.Instance.Load();

      target = TargetObject.GetComponent<IActor>();     
    }

    // Update is called once per frame
    void Update()
    {
      if (ResourceManager.Instance.IsLoading) {
        return;
      }

      if (Input.GetKeyDown(KeyCode.T)) {
        BulletManager.Instance.Terminate();
      }

      if (Input.GetKeyDown(KeyCode.C)) {
        BulletManager.Instance.Clear();
      }

      if (!Input.GetKeyDown(KeyCode.Space)) {
        return;
      }

      if (MyEnum.TryParse<global::SkillId>(SkillId, out var id)) {
        SkillManager.Instance.SetActiveSkill(0, id);
      }
      else {
        Logger.Error($"{SkillId} is not defined.");
      }

      Skill skill = SkillManager.Instance.GetActiveSkill(0) as Skill;
      skill.ManualFire(Vector3.zero, target);
    }

    private void LateUpdate()
    {
      var colliders = Physics.OverlapSphere(
        Vector3.zero, 
        10f, 
        LayerMask.GetMask(LayerName.PlayerBullet)
      );

      foreach (var collider in colliders)
      {
        collider.GetComponent<IBullet>().Intersect();
      }
    }

    private void OnGUI()
    {
      GUILayout.Label("Space: Fire Bullet");
      GUILayout.Label("T    : Bullet Terminate");
      GUILayout.Label("C    : Object Pool Clear");
    }
  }
}
#endif