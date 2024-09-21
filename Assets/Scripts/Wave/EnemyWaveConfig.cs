using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class EnemyWaveConfig : MyMonoBehaviour
{
  public EnemyWaveProperty props = new EnemyWaveProperty();

#if UNITY_EDITOR
  [CustomEditor(typeof(EnemyWaveConfig))]
  public class EnemyWaveConfigEditor : Editor
  {
    /// <summary>
    /// WaveÉfÅ[É^
    /// </summary>
    private readonly Dictionary<int, List<EnemyWaveConfig>> data = new();

    public override void OnInspectorGUI()
    {
      base.OnInspectorGUI();

      var config = (EnemyWaveConfig)target;
      var text  = $"FAT = {config.props.CalcFAT()}\n";
          text += $"EIPS = {config.props.CalcEIPS(1)}";
      EditorGUILayout.TextArea(text);
    }
  }
#endif
}