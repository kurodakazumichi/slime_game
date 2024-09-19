using UnityEngine;

public interface IMyMonoBehaviour
{
  /// <summary>
  /// GameObjectを持つ
  /// </summary>
  GameObject gameObject { get; }

  /// <summary>
  /// Transformを持つ
  /// </summary>
  Transform CachedTransform { get; }

  /// <summary>
  /// 座標
  /// </summary>
  Vector3 Position { get; set; }

  /// <summary>
  /// 親を設定する
  /// </summary>
  void SetParent(Transform parent, bool worldPositionStays);

  /// <summary>
  /// 有効化
  /// </summary>
  void SetActive(bool isActive);
}

/// <summary>
/// UnityのMonoBehaviourラッパー
/// </summary>
public class MyMonoBehaviour : MonoBehaviour, IMyMonoBehaviour
{
  //============================================================================
  // Properities
  //============================================================================
  /// <summary>
  /// Transformのキャッシュ(処理負荷軽減対策)
  /// </summary>
  public Transform CachedTransform { get; private set; }

  /// <summary>
  /// 座標
  /// </summary>
  public Vector3 Position {
    get { return CachedTransform.position; }
    set { CachedTransform.position = value; }
  }

  /// <summary>
  /// 自信がアクティブかどうか
  /// </summary>
  public bool IsActiveSelf => this.gameObject.activeSelf;

  /// <summary>
  /// Layerのショートカット
  /// </summary>
  protected int layer {
    get { return  this.gameObject.layer; }
    set { this.gameObject.layer = value; }
  }

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
