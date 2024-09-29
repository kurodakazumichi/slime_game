using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyGame.Master;

namespace MyGame.Tester
{
  public class TestMasterScene : MonoBehaviour
  {
    // Start is called before the first frame update
    void Start()
    {
      EnemyMaster.Init();

      var master = EnemyMaster.FindById(EnemyId.Enemy000);

      if (master is not null) {
        Debug.Log($"Id = {master.Id}");
        Debug.Log($"No = {master.No}");
        Debug.Log($"Name = {master.Name}");
        Debug.Log($"HP = {master.HP})");
        Debug.Log($"Power = {master.Power})");
        Debug.Log($"Speed = {master.Speed})");
        Debug.Log($"Mass = {master.Mass})");
        Debug.Log($"AttackAttr = {master.AttackAttr})");
        Debug.Log($"WeakAttr = {master.WeakAttr})");
        Debug.Log($"ResistAttr = {master.ResistAttr})");
        Debug.Log($"NullfiedAttr = {master.NullfiedAttr})");
        Debug.Log($"SkillId = {master.SkillId})");
        Debug.Log($"Exp = {master.Exp})");
        Debug.Log($"PrefabPath = {master.PrefabPath})");
      }
    }
  }

}