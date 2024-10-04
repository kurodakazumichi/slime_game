using System;
using UnityEngine;
using MyGame.Master;
using MyGame.View;
using MyGame.Presenter;

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
    /// Playerを演じる者
    /// </summary>
    private PlayerPresenter presenter;

    //=========================================================================
    // Properties
    //=========================================================================

    /// <summary>
    /// PlayerのViewオブジェクトを返す
    /// </summary>
    public View.Player View => presenter.View;

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

      presenter = new();
      presenter.Init(config, view, fs, onChangeHP);
    }

    public void Update()
    {
      presenter.Update();
    }

    public void SetPlayable()
    {
      presenter.Playable();
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