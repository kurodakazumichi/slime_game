using System;
using UnityEngine;
using MyGame.Master;
using MyGame.View;
using MyGame.ViewLogic;

namespace MyGame.System
{
  public interface IPlayerSystem { 
    View.Player View { get; }
  }

  public class PlayerSystem : IPlayerSystem
  {
    //=========================================================================
    // Const
    //=========================================================================
    /// <summary>
    /// PlayerのPrefabのパス
    /// </summary>
    const string PLAYER_PREFAB_PATH = "Player/Player.prefab";

    //=========================================================================
    // Variables
    //=========================================================================

    /// <summary>
    /// Playerロジック
    /// </summary>
    private PlayerLogic playerLogic;

    //=========================================================================
    // Properties
    //=========================================================================

    /// <summary>
    /// PlayerのViewオブジェクトを返す
    /// </summary>
    public View.Player View => playerLogic.View;

    //=========================================================================
    // Methods
    //=========================================================================

    //-------------------------------------------------------------------------
    // Public
    //-------------------------------------------------------------------------

    /// <summary>
    /// ロード
    /// </summary>
    public void Load()
    {
      ResourceSystem.Load<GameObject>(PLAYER_PREFAB_PATH);
    }

    /// <summary>
    /// 初期化
    /// </summary>
    public void Init(
      IPlayerEntity config,
      IFieldSystem fs,
      Action<float, float> onChangeHP
    )
    {
      var view = MakePlayerView();

      playerLogic = new();
      playerLogic.Init(config, view, fs, onChangeHP);
    }

    public void Update()
    {
      playerLogic.Update();
    }

    public void SetPlayable()
    {
      playerLogic.Playable();
    }

    //-------------------------------------------------------------------------
    // Resource系
    //-------------------------------------------------------------------------
    private Player MakePlayerView()
    {
      var res = ResourceSystem.GetCache<GameObject>(PLAYER_PREFAB_PATH);
      return GameObject.Instantiate(res).GetComponent<Player>();
    }
  }
}