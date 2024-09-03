using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using UnityEditor.AddressableAssets;
using System.IO;
using UnityEditor.AddressableAssets.Settings;

/// <summary>
/// Assets/Addressables�Ƀt�@�C�����ǉ����ꂽ�Ƃ���A���l�[���A�ړ��A�폜���ꂽ����
/// �K�؂�Addresable Assets�Ƀ��\�[�X��o�^����B
/// �t�@�C�����폜���ꂽ�Ƃ��͓��ɉ��������Ƃ������AddressableAssets����폜�����
/// �悤�Ȃ̂ŁA�����ł͓��ɏ��������Ȃ��B
/// </summary>
public class AutoAddressing : AssetPostprocessor
{
  /// <summary>
  /// �Ď��Ώۃf�B���N�g��
  /// </summary>
  private const string TARGET_DIRECTORY = "Assets/Addressables";

  /// <summary>
  /// ���ɏ����ς̃p�X���i�[���Ă���
  /// </summary>
  private static List<string> processedPaths = new List<string>();

  /// <summary>
  /// Asset���ǉ��A�ړ��A�폜���ꂽ�Ƃ��ɓ��삷��B
  /// </summary>
  private static void OnPostprocessAllAssets(
    string[] importedAssets, 
    string[] deletedAssets,
    string[] moveAssets,
    string[] movedFromAssetPaths)
  {
    var settings = AddressableAssetSettingsDefaultObject.Settings;

    processedPaths.Clear();

    // �V�K�ǉ��⃊�l�[���������ꍇ��"importedAssets"�ɂ��̃t�@�C���̃p�X�������Ă���
    foreach (var asset in importedAssets) 
    {
      if (!isAddressablePath(asset)) {
        continue;
      }

      Debug.Log($"[AutoAddressing] ImportedAsset = {asset}");
      Regist(settings, asset);
    }


    // �t�@�C���̃��l�[����ړ������ꍇ��"moveAssets"�ɂ��̃t�@�C���̃p�X�������Ă���
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
  /// �A�h���X�w��\�ȃp�X�Ȃ��true
  /// </summary>
  private static bool isAddressablePath(string path)
  {
    // �Ώۃf�B���N�g���ȊO�͖���
    if (!path.Contains(TARGET_DIRECTORY)) {
      return false;
    }

    // �t�H���_������
    if (File.GetAttributes(path).HasFlag(FileAttributes.Directory)) {
      return false;
    }

    // ���ɏ������ꂽ�t�@�C���ł���Ζ���
    if (processedPaths.Contains(path)) {
      return false;
    }

    return true;
  }

  /// <summary>
  /// ���\�[�X��o�^����
  /// </summary>
  private static void Regist(AddressableAssetSettings settings, string asset)
  {
    var guid       = AssetDatabase.AssetPathToGUID(asset);
    var group      = settings.DefaultGroup;
    var assetEntry = settings.CreateOrMoveEntry(guid, group);

    // "Assets/Addressables/"�ȉ��̕������A�h���X�Ƃ��ēo�^����
    var address = (asset.Substring(TARGET_DIRECTORY.Length + 1));
    assetEntry.SetAddress(address);

    processedPaths.Add(asset);
  }
}
