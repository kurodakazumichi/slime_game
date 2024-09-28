using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using MyGame.Master;

public class ConvertMaster
{
  const string CSV_BASE_PATH = "Assets/Addressables/Master/csv";
  const string DAT_BASE_PATH = "Assets/Addressables/Master";

  /// <summary>
  /// Convert Enemy.csv to Scriptable Objects
  /// </summary>
  [MenuItem("Assets/ConvertMaster/Enemy/csv2obj")]
  public static void ConvertEnemyCsvToEnemyMaster()
  {
    var datas = LoadAndParseCsv($"{CSV_BASE_PATH}/Enemy.csv");

    foreach (var data in datas)
    {
      var path = $"{DAT_BASE_PATH}/Enemy/{data["Id"]}.asset";
      var so = LoadOrCreate<EnemyEntity2>(path);

      so._id                 = data["Id"];
      so._no                 = int.Parse(data["No"]);
      so._name               = data["Name"];
      so._hp                 = int.Parse(data["HP"]);
      so._power              = int.Parse(data["Power"]);
      so._speed              = float.Parse(data["Speed"]);
      so._mass               = float.Parse(data["Mass"]);
      so._attackAttributes   = data["AttackAttr"];
      so._weakAttributes     = data["WeakAttr"];
      so._resistAttributes   = data["ResistAttr"];
      so._nullfiedAttributes = data["NullfiedAttr"];
      so._skillId            = data["SkillId"];
      so._exp                = int.Parse(data["Exp"]);

      AssetDatabase.SaveAssets();
    }
  }

  /// <summary>
  /// CSVの1行目をヘッダ行として、CSVをロード、パースした状態のデータを返す
  /// </summary>
  public static List<Dictionary<string, string>> LoadAndParseCsv(string path)
  {
    // CSVロード
    var csv    = AssetDatabase.LoadAssetAtPath<TextAsset>(path);
    var reader = new StringReader(csv.text);

    // ヘッダ行読み込み
    var header = reader.ReadLine().Split(',');

    // データ行読み込み
    List<Dictionary<string, string>> datas = new();

    while(reader.Peek() != -1) 
    {
      var line    = reader.ReadLine();
      var columns = line.Split(',');

      var data = new Dictionary<string, string>();

      for(int i = 0; i < header.Length; ++i) {
        data.Add(header[i], columns[i]);
      }

      datas.Add(data);
    }

    return datas;
  }

  /// <summary>
  /// {path}で指定したファイル(ScriptableObject)をロード、ファイルがなければ新規作成する。
  /// </summary>
  public static T LoadOrCreate<T>(string path) where T : ScriptableObject
  {
    var so = AssetDatabase.LoadAssetAtPath<T>(path);

    if (so is null) {
      so = ScriptableObject.CreateInstance<T>();
      AssetDatabase.CreateAsset(so, path);
    }

    return so;
  }
}
