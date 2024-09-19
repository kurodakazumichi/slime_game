using UnityEngine;

public class FacingCamera : MyMonoBehaviour
{
  protected override void MyAwake()
  {
    base.MyAwake();
    CachedTransform.rotation = Quaternion.Euler(App.CAMERA_ANGLE_X, 0f, 0f);
  }
}
