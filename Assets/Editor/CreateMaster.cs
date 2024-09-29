using UnityEngine;
using UnityEditor;
using System.IO;
using MyGame.Master;

public class CreateMaster
{
  [MenuItem("Assets/CreateMaster/Enemy")]
  private static void CreateEnemyMaster()
  {
    // 選択中のモノのID
    var id = Selection.activeInstanceID;
    var path = AssetDatabase.GetAssetPath(id);

    if (!PathUtil.IsDirectory(path)) {
      Debug.LogWarning("ディレクトリ上で実行してください。");
      return;
    }

    var obj = ScriptableObject.CreateInstance<EnemyEntity>();
    var filename = $"NewEnemyEneity.asset";

    AssetDatabase.CreateAsset(obj, Path.Combine(path+"\\", filename));
  }
}