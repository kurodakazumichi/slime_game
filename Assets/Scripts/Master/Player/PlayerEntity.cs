using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame.Master 
{ 
  public class PlayerEntity : ScriptableObject, IPlayerEntity
  {
    //=========================================================================
    // Inspector
    //=========================================================================
    [SerializeField]
    private float _speedInSearch;
    [SerializeField]
    private float _speedInBattle;
    [SerializeField]
    private int _initialHP;

    //=========================================================================
    // Property
    //=========================================================================
    public float SpeedInSearch => _speedInSearch;
    public float SpeedInBattle => _speedInBattle;
    public int InitialHP => _initialHP;
  }
}