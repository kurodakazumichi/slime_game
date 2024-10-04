using UnityEngine;

namespace MyGame.Presenter 
{ 
  /// <summary>
  /// 追従カメラ
  /// </summary>
  public class TrackingCameraPresenter : IPresenter
  {
    /// <summary>
    /// カメラのトランスフォーム
    /// </summary>
    private Transform cameraTransform;

    /// <summary>
    /// 追従対象
    /// </summary>
    private Transform target;

    /// <summary>
    /// 対象からのオフセット
    /// </summary>
    private Vector3 offset = Vector3.zero;

    /// <summary>
    /// 初期化
    /// </summary>
    public void Init(Camera camera)
    {
      cameraTransform = camera.transform;
    }

    /// <summary>
    /// 追従対象をセット
    /// </summary>
    public void SetTarget(Transform target, Vector3 offset)
    {
      this.target = target;
      this.offset = offset;
    }

    /// <summary>
    /// 更新
    /// </summary>
    public void Update()
    {
      if (target is null) return;

      cameraTransform.position = target.position + offset;
      cameraTransform.rotation = Quaternion.LookRotation(target.position - cameraTransform.position, Vector3.up);
    }
  }

}