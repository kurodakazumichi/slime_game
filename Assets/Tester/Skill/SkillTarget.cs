using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Editor.Tester
{
  public class SkillTarget : MyMonoBehaviour, IActor
  {
    public DamageInfo TakeDamage(AttackInfo info)
    {
      return new DamageInfo(1f, DamageDetail.NormalDamage);
    }

    void Update()
    {
      var x = Mathf.Sin(Time.time) * 5f;
      var z = Mathf.Cos(Time.time) * 5f;

      CachedTransform.position = new Vector3(x, 0f, z);
    }
  }
}

