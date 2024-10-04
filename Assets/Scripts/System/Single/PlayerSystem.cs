using System;
using UnityEngine;
using MyGame.Master;
using MyGame.Presenter;
using MyGame.View;

namespace MyGame.System
{
  public interface IPlayerSystem {
    Transform PlayerTransform { get; }
  }

  public class PlayerSystem : IPlayerSystem
  {
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
    public Transform PlayerTransform => presenter.TargetTransform;

    //=========================================================================
    // Methods
    //=========================================================================

    //-------------------------------------------------------------------------
    // Public
    //-------------------------------------------------------------------------

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public PlayerSystem()
    {
      presenter = new();
    }

    /// <summary>
    /// ロード
    /// </summary>
    public void Load()
    {
      presenter.Load();
    }

    /// <summary>
    /// 初期化
    /// </summary>
    public void Init(
      IFieldSystem fs,
      Action<float, float> onChangeHP
    )
    {
      presenter.Init(fs, onChangeHP);
    }

    public void Update()
    {
      presenter.Update();
    }

    public void SetPlayable()
    {
      presenter.Playable();
    }
  }
}