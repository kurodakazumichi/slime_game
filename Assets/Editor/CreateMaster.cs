using UnityEngine;
using UnityEditor;
using System.IO;
using MyGame.Master;

public class CreateMaster
{
  [MenuItem("Assets/CreateMaster/Enemy")]
  private static void CreateEnemyMaster()
  {
    CreateMasterData<EnemyEntity>($"NewEnemyEneity.asset");
  }

  [MenuItem("Assets/CreateMaster/Skill")]
  private static void CreateSkillMaster()
  {
    CreateMasterData<SkillEntity>($"NewSkillEneity.asset");
  }

  private static void CreateMasterData<T>(string filename)
    where T : ScriptableObject
  {
    // 選択中のモノのID
    var id = Selection.activeInstanceID;
    var path = AssetDatabase.GetAssetPath(id);

    if (!PathUtil.IsDirectory(path)) {
      Debug.LogWarning("ディレクトリ上で実行してください。");
      return;
    }

    var obj = ScriptableObject.CreateInstance<T>();
    AssetDatabase.CreateAsset(obj, Path.Combine(path+"\\", filename));
  }
}