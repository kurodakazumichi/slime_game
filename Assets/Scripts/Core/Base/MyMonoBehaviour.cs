﻿using UnityEngine;

/// <summary>
/// UnityのMonoBehaviourラッパー
/// </summary>
public class MyMonoBehaviour : MonoBehaviour
{
  //============================================================================
  // Variables
  //============================================================================

  /// <summary>
  /// Transformのキャッシュ(処理負荷軽減対策)
  /// </summary>
  public Transform CachedTransform {  get; private set; }

  /// <summary>
  /// RectTransformのキャッシュ(処理負荷軽減対策)
  /// </summary>
  public RectTransform CachedRectTransform { get; private set; }
  //============================================================================
  // Properities
  //============================================================================

  /// <summary>
  /// 自信がアクティブかどうか
  /// </summary>
  public bool IsActiveSelf => this.gameObject.activeSelf;

  //============================================================================
  // Methods
  //============================================================================

  //----------------------------------------------------------------------------
  // Life Cycle
  //----------------------------------------------------------------------------
  protected void Awake()
  {
    if (CachedTransform == null) {
      CachedTransform = this.transform;
    }

    if (CachedRectTransform == null) {
      CachedRectTransform = GetComponent<RectTransform>();
    }

    MyAwake();
  }

  protected virtual void MyAwake() { }

  //----------------------------------------------------------------------------
  // syntax sugar
  //----------------------------------------------------------------------------
  /// <summary>
  /// 親を設定する
  /// </summary>
  public void SetParent(Transform parent, bool worldPositionStays = true)
  {
    this.CachedTransform.SetParent(parent, worldPositionStays);
  }

  /// <summary>
  /// アクティブを設定する
  /// </summary>
  public void SetActive(bool isActive)
  {
    this.gameObject.SetActive(isActive);
  }

#if _DEBUG
  //----------------------------------------------------------------------------
  // For Debug
  //----------------------------------------------------------------------------

  /// <summary>
  /// デバッグ用の基底メソッド
  /// </summary>
  public virtual void OnDebug()
  {

  }

#endif

}
