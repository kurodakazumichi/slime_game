using UnityEngine;
using MyGame.ViewLogic;
using UnityEngine.AddressableAssets;

namespace MyGame.Tester
{

  public class TestPlayerScene : MyMonoBehaviour
  {
    private PlayerLogic player = new();

    // Start is called before the first frame update
    void Start()
    {
      var handle = Addressables.LoadAssetAsync<GameObject>("Player/Player.prefab");
      handle.WaitForCompletion();

      var view = Instantiate(handle.Result);

      player.Init(view.GetComponent<MyGame.View.Player>(), OnChangeHP);
    }

    // Update is called once per frame
    void Update()
    {
      if(Input.GetKeyDown(KeyCode.Alpha0)) {
        player.SetActive(false);
      }
      if(Input.GetKeyDown(KeyCode.Alpha9)) {
        player.SetActive(true);
      }

      if (Input.GetKeyDown(KeyCode.Alpha1)) {
        player.Reset();
      }

      if (Input.GetKeyDown(KeyCode.Alpha2)) {
        player.Playable();
      }

      if (Input.GetKeyDown(KeyCode.Alpha3)) {
        player.FieldMode();
      }

      if (Input.GetKeyDown(KeyCode.Alpha4)) {
        player.BattleMode();
      }

      if (Input.GetKeyDown(KeyCode.Alpha5)) 
      {
        player.TakeDamage(new AttackInfo() { 
          Power      = 1,
          Attributes = (uint)Attribute.Non
        });
      }

      player.Update();
    }

    private void OnChangeHP(float now, float rate)
    {
      Debug.Log($"{now} {rate}");
    }
  }
}