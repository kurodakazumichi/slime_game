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

    private List<global::SkillId> skillIds = new List<global::SkillId>();
    private int currentIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
      DebugManager.Instance.Regist(SkillManager.Instance);
      DebugManager.Instance.Regist(BulletManager.Instance);
      DebugManager.Instance.Regist(ResourceManager.Instance);
      
      BulletManager.Instance.Load();

      target = TargetObject.GetComponent<IActor>();

      MyEnum.ForEach<global::SkillId>((id) => {
        skillIds.Add(id);
      });

      for (int i = 0; i < skillIds.Count; i++) {
        if (MyEnum.Parse<global::SkillId>(SkillId) == skillIds[i]) {
          currentIndex = i;
          break;
        }
      }
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

      if (Input.GetKeyDown(KeyCode.LeftArrow)) {
        currentIndex = (skillIds.Count + currentIndex - 1) % skillIds.Count;
      }
      if (Input.GetKeyDown(KeyCode.RightArrow)) {
        currentIndex = (currentIndex + 1) % skillIds.Count;
      }

      if (!Input.GetKeyDown(KeyCode.Space)) {
        return;
      }

      var id = skillIds[currentIndex];

      if (id == global::SkillId.Undefined) {
        return;
      }

      SkillManager.Instance.SetActiveSkill(0, id);

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
      GUILayout.Label($"Space: Fire Bullet ({skillIds[currentIndex].ToString()})");
      GUILayout.Label("T    : Bullet Terminate");
      GUILayout.Label("C    : Object Pool Clear");
    }
  }
}
#endif