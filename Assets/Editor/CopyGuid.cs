using UnityEditor;
using UnityEngine;

public class CopyGuid
{
  [MenuItem("Assets/Copy GUID")]
  static void CopyGuidMenu()
  {
    var guids = string.Join( ",", Selection.assetGUIDs);
    GUIUtility.systemCopyBuffer = guids;
    Debug.Log($"copy to clipboard:{guids}");
  }
}
