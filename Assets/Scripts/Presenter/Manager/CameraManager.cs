using UnityEngine;

namespace MyGame.Presenter.Manager
{
  public interface ICameraManager
  {
    public void SetupTrackingCamera(Camera camera, Transform target, Vector3 offset);
  }

  public class CameraManager : ICameraManager
  {
    /// <summary>
    /// 追従カメラ
    /// </summary>
    private TrackingCameraPresenter trackingCamera = null;

    /// <summary>
    /// 追従カメラのセットアップ
    /// </summary>
    public void SetupTrackingCamera(Camera camera, Transform target, Vector3 offset)
    {
      if (trackingCamera is null) {
        trackingCamera = new();
      }
      trackingCamera.Init(camera);
      trackingCamera.SetTarget(target, offset);
    }

    /// <summary>
    /// 追従カメラを解除
    /// </summary>
    public void ReleaseTrackingCamera()
    {
      trackingCamera = null;
    }

    /// <summary>
    /// 更新
    /// </summary>
    public void Update()
    {
      if (trackingCamera != null) {
        trackingCamera.Update();
      }
    }
  }
}