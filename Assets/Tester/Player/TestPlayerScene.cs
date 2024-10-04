using UnityEngine;
using MyGame.System;
using MyGame.Master;
using MyGame.Presenter.Manager;

namespace MyGame.Tester
{
  
  public class TestPlayerScene : MyMonoBehaviour
  {
    public class MockFieldSystem : IFieldManager
    {
      public Vector3 BattleCircleCenter => Vector3.zero;

      public bool HasBattleCircle => true;

      public bool IsInBattleCircle(Vector3 position)
      {
        return (position.magnitude < 10f);
      }
    }

    private bool isInitialized = false;

    private PlayerManager sPlayer = new();

    // Start is called before the first frame update
    void Start()
    {
      PlayerMaster.Init();
      sPlayer.Load();
    }

    // Update is called once per frame
    void Update()
    {
      if (ResourceSystem.IsLoading) return;

      if (!isInitialized) {
        sPlayer.Init(new MockFieldSystem(), OnChangeHP);
        CameraSystem.SetupTrackingCamera(Camera.main, sPlayer.PlayerTransform, new Vector3(0, 10f, -10f));
        isInitialized = true;
      }

      sPlayer.Update();
      CameraSystem.Update();
    }

    private void OnChangeHP(float now, float rate)
    {
      Debug.Log($"{now} {rate}");
    }
  }
}