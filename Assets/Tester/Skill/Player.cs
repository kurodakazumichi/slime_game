using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkillTester
{
  public class Player : MyMonoBehaviour, IActor
  {
    // Update is called once per frame
    void Update()
    {
      var v = Vector3.zero;

      if (Input.GetKey(KeyCode.LeftArrow)) {
        v.x = -5;
      }
      if (Input.GetKey(KeyCode.RightArrow)) {
        v.x = 5;
      }
      if (Input.GetKey(KeyCode.UpArrow)) {
        v.z = 5;
      }
      if (Input.GetKey(KeyCode.DownArrow)) { 
        v.z = -5;
      }

      CachedTransform.position += v * Time.deltaTime;
    }

    public DamageInfo TakeDamage(AttackInfo info)
    {
      return new DamageInfo();
    }
  }

}
