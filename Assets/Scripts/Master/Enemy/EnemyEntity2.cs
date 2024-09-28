using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame.Master
{
  public class EnemyEntity2 : ScriptableObject, IEnemyEntityRO
  {
    //=========================================================================
    // Inspector
    //=========================================================================
    public string _id;

    public int _no;

    public string _name;

    public int _hp;

    public int _power;

    public float _speed;

    public float _mass;

    public string _attackAttributes;

    public string _weakAttributes;

    public string _resistAttributes;

    public string _nullfiedAttributes;

    public string _skillId;

    public int _exp;

    //=========================================================================
    // Property
    //=========================================================================
    public EnemyId Id => MyEnum.Parse<EnemyId>(_id);

    public int No => _no;

    public string Name => _name;

    public float HP => throw new System.NotImplementedException();

    public float Power => throw new System.NotImplementedException();

    public float Speed => throw new System.NotImplementedException();

    public float Mass => throw new System.NotImplementedException();

    public uint AttackAttr => throw new System.NotImplementedException();

    public uint WeakAttr => throw new System.NotImplementedException();

    public uint ResistAttr => throw new System.NotImplementedException();

    public uint NullfiedAttr => throw new System.NotImplementedException();

    public SkillId SkillId => throw new System.NotImplementedException();

    public int Exp => throw new System.NotImplementedException();

    public string PrefabPath => throw new System.NotImplementedException();

    public string IconPath => throw new System.NotImplementedException();


  }
}


