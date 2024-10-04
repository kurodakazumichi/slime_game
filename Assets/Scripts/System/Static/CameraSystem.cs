using UnityEngine;
using MyGame.Presenter;

namespace MyGame.System
{
  public static class CameraSystem
  {
    /// <summary>
    /// 追従カメラ
    /// </summary>
    private static TrackingCameraPresenter trackingCamera = null;

    /// <summary>
    /// 追従カメラのセットアップ
    /// </summary>
    public static void SetupTrackingCamera(Camera camera, Transform target, Vector3 offset)
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
    public static void ReleaseTrackingCamera()
    {
      trackingCamera = null;
    }

    /// <summary>
    /// 更新
    /// </summary>
    public static void Update()
    {
      if (trackingCamera != null) {
        trackingCamera.Update();
      }
    }
  }
}