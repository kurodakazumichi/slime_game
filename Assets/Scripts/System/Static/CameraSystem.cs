﻿using UnityEngine;
using MyGame.ViewLogic;

namespace MyGame.System
{
  public static class CameraSystem
  {
    /// <summary>
    /// 追従カメラ
    /// </summary>
    private static TrackingCameraLogic trackingCamera = null;

    /// <summary>
    /// 追従カメラのセットアップ
    /// </summary>
    public static void SetupTrackingCamera(Camera camera, MyMonoBehaviour target, Vector3 offset)
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