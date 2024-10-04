using System;
using UnityEngine;
using MyGame.Master;
using MyGame.Presenter;
using MyGame.View;

namespace MyGame.Presenter.Manager
{
  public interface IPlayerManager {
    Transform PlayerTransform { get; }
  }

  public class PlayerManager : IPlayerManager
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
    public PlayerManager()
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
      IFieldManager fm,
      Action<float, float> onChangeHP
    )
    {
      presenter.Init(fm, onChangeHP);
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