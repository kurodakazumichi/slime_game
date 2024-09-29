using UnityEngine;
using UnityEngine.AddressableAssets;

namespace MyGame.Master
{
  public static class MasterUtil
  {
    /// <summary>
    /// Masterデータの同期ロード
    /// </summary>
    public static T LoadEntity<T>(string dirname, string filename) where T : ScriptableObject
    {
      var path = $"Master/{dirname}/{filename}.asset";
      var handle = Addressables.LoadAssetAsync<T>(path);
      handle.WaitForCompletion();

      return handle.Result;
    }
  }
}
