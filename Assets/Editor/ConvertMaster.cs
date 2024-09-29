using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using MyGame;
using MyGame.Master;
using System.Text;

public class ConvertMaster
{
  //===========================================================================
  // Const
  //===========================================================================
  /// <summary>
  /// Masterデータの大元となるcsvファイルを格納しているディレクトリパス
  /// </summary>
  const string CSV_BASE_PATH = "Assets/Addressables/Master/csv";

  /// <summary>
  /// 各種Master(ScripatbleObject)を格納している親ディレクトリのパス
  /// </summary>
  const string DAT_BASE_PATH = "Assets/Addressables/Master";

  //===========================================================================
  // Convert Enemy Master
  //===========================================================================
  /// <summary>
  /// 敵のデータ(csv)をScriptableObjectに出力
  /// </summary>
  [MenuItem("Assets/ConvertMaster/Enemy/csv2obj")]
  public static void ConvertEnemyCsvToEnemyMaster()
  {
    var datas = LoadAndParseCsv($"{CSV_BASE_PATH}/Enemy.csv");

    foreach (var data in datas)
    {
      var path = $"{DAT_BASE_PATH}/Enemy/{data["Id"]}.asset";
      var so = LoadOrCreate<EnemyEntity>(path);

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
  /// 敵のデータ(ScriptableObject)の内容をcsvファイルに出力
  /// </summary>
  [MenuItem("Assets/ConvertMaster/Enemy/obj2csv")]
  public static void ConvertEnemyMasterToEnemyCsv()
  {
    var DATA_DIR = $"{DAT_BASE_PATH}/Enemy/";
    var CSV_PATH = $"{CSV_BASE_PATH}/Enemy.csv";

    // DATA_DIRにある全ScriptableObjectの内容をCsvTextにする
    var text = MakeCsvTextFromMasterDatas<EnemyEntity>(DATA_DIR, EnemyEntity.CsvHeaderString());

    // ファイル書き込み
    using (var writer = new StreamWriter(CSV_PATH)) {
      writer.Write(text);
    }
      
    // UnityにImport
    AssetDatabase.ImportAsset(CSV_PATH);
  }

  //===========================================================================
  // Convert Enemy Master
  //===========================================================================
  /// <summary>
  /// スキルデータ(csv)をScriptableObjectに出力
  /// </summary>
  [MenuItem("Assets/ConvertMaster/Skill/csv2obj")]
  public static void ConvertSkillCsvToSkillMaster()
  {
    var datas = LoadAndParseCsv($"{CSV_BASE_PATH}/Skill.csv");

    foreach (var data in datas)
    {
      var path = $"{DAT_BASE_PATH}/Skill/{data["Id"]}.asset";
      var so = LoadOrCreate<SkillEntity>(path);

      so._id              = data["Id"];
      so._no              = int.Parse(data["No"]);
      so._name            = data["Name"];
      so._maxExp          = int.Parse(data["MaxExp"]);
      so._recastF         = float.Parse(data["RecastF"]);
      so._recastL         = float.Parse(data["RecastL"]);
      so._powerF          = int.Parse(data["PowerF"]);
      so._powerL          = int.Parse(data["PowerL"]);
      so._penetrableF     = int.Parse(data["PenetrableF"]);
      so._penetrableL     = int.Parse(data["PenetrableL"]);
      so._speedGrowthRate = float.Parse(data["SpeedGrowthRate"]);
      so._attributes      = data["Attributes"];
      so._growthType      = data["GrowthType"];
      so._impact          = float.Parse(data["Impact"]);
      so._iconNo          = int.Parse(data["IconNo"]);
      so._aimingType      = data["AimingType"];

      AssetDatabase.SaveAssets();
    }
  }

  /// <summary>
  /// スキルのデータ(ScriptableObject)の内容をcsvファイルに出力
  /// </summary>
  [MenuItem("Assets/ConvertMaster/Skill/obj2csv")]
  public static void ConvertSkillMasterToSkillCsv()
  {
    var DATA_DIR = $"{DAT_BASE_PATH}/Skill/";
    var CSV_PATH = $"{CSV_BASE_PATH}/Skill.csv";

    // DATA_DIRにある全ScriptableObjectの内容をCsvTextにする
    var text = MakeCsvTextFromMasterDatas<SkillEntity>(DATA_DIR, SkillEntity.CsvHeaderString());

    // ファイル書き込み
    using (var writer = new StreamWriter(CSV_PATH)) {
      writer.Write(text);
    }
      
    // UnityにImport
    AssetDatabase.ImportAsset(CSV_PATH);
  }


  //===========================================================================
  // Common
  //===========================================================================

  /// <summary>
  /// 指定されたディレクトリにあるMasterデータを元にCsvTextを生成する
  /// </summary>
  private static string MakeCsvTextFromMasterDatas<T>(string dataDirPath, string header) where T : ScriptableObject
  {
    // データを格納しているディレクトリにあるScriptableObjectのパスを全て取得
    var paths = GetFilesWithExtension(dataDirPath, "*.asset");

    // ScriptableObjectの内容をCSV形式のデータに変換
    StringBuilder sb = new();
    sb.Append(EnemyEntity.CsvHeaderString());
    sb.Append("\r\n");

    foreach (var path in paths) 
    {
      var so = AssetDatabase.LoadAssetAtPath<T>(path) as IConvertibleCsvText;
      sb.Append(so.ToCsvText());
      sb.Append("\r\n");
    }

    return sb.ToString();
  }

  /// <summary>
  /// CSVの1行目をヘッダ行として、CSVをロード、パースした状態のデータを返す
  /// </summary>
  private static List<Dictionary<string, string>> LoadAndParseCsv(string path)
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
  private static T LoadOrCreate<T>(string path) where T : ScriptableObject
  {
    var so = AssetDatabase.LoadAssetAtPath<T>(path);

    if (so is null) {
      so = ScriptableObject.CreateInstance<T>();
      AssetDatabase.CreateAsset(so, path);
    }

    return so;
  }

  /// <summary>
  /// 特定の拡張子を持つファイル一覧を取得するメソッド 
  /// </summary>
  private static string[] GetFilesWithExtension(string folderPath, string extension)
  {
    // パスが存在するかチェック
    if (!Directory.Exists(folderPath))
    {
      Debug.LogError("フォルダが存在しません: " + folderPath);
      return new string[0];
    }

    // 拡張子に基づいてファイル一覧を取得
    return Directory.GetFiles(folderPath, extension, SearchOption.TopDirectoryOnly);
  }
}
