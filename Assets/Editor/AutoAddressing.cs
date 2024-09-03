using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using UnityEditor.AddressableAssets;
using System.IO;
using UnityEditor.AddressableAssets.Settings;

/// <summary>
/// Assets/Addressablesにファイルが追加されたときや、リネーム、移動、削除された時に
/// 適切にAddresable Assetsにリソースを登録する。
/// ファイルが削除されたときは特に何もせずとも勝手にAddressableAssetsから削除される
/// ようなので、ここでは特に処理をしない。
/// </summary>
public class AutoAddressing : AssetPostprocessor
{
  /// <summary>
  /// 監視対象ディレクトリ
  /// </summary>
  private const string TARGET_DIRECTORY = "Assets/Addressables";

  /// <summary>
  /// 既に処理済のパスを格納しておく
  /// </summary>
  private static List<string> processedPaths = new List<string>();

  /// <summary>
  /// Assetが追加、移動、削除されたときに動作する。
  /// </summary>
  private static void OnPostprocessAllAssets(
    string[] importedAssets, 
    string[] deletedAssets,
    string[] moveAssets,
    string[] movedFromAssetPaths)
  {
    var settings = AddressableAssetSettingsDefaultObject.Settings;

    processedPaths.Clear();

    // 新規追加やリネームをした場合は"importedAssets"にそのファイルのパスが入っている
    foreach (var asset in importedAssets) 
    {
      if (!isAddressablePath(asset)) {
        continue;
      }

      Debug.Log($"[AutoAddressing] ImportedAsset = {asset}");
      Regist(settings, asset);
    }


    // ファイルのリネームや移動した場合は"moveAssets"にそのファイルのパスが入っている
    foreach (var asset in moveAssets) 
    {
      if (!isAddressablePath(asset)) {
        continue;
      }

      Debug.Log($"[AutoAddressing] moveAsset = {asset}");
      Regist(settings, asset);
    }

  }

  /// <summary>
  /// アドレス指定可能なパスならばtrue
  /// </summary>
  private static bool isAddressablePath(string path)
  {
    // 対象ディレクトリ以外は無視
    if (!path.Contains(TARGET_DIRECTORY)) {
      return false;
    }

    // フォルダも無視
    if (File.GetAttributes(path).HasFlag(FileAttributes.Directory)) {
      return false;
    }

    // 既に処理されたファイルであれば無視
    if (processedPaths.Contains(path)) {
      return false;
    }

    return true;
  }

  /// <summary>
  /// リソースを登録する
  /// </summary>
  private static void Regist(AddressableAssetSettings settings, string asset)
  {
    var guid       = AssetDatabase.AssetPathToGUID(asset);
    var group      = settings.DefaultGroup;
    var assetEntry = settings.CreateOrMoveEntry(guid, group);

    // "Assets/Addressables/"以下の部分をアドレスとして登録する
    var address = (asset.Substring(TARGET_DIRECTORY.Length + 1));
    assetEntry.SetAddress(address);

    processedPaths.Add(asset);
  }
}
