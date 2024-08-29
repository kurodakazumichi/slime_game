using UnityEngine;
using UnityEditor;

public class EnemyWaveSettings : MonoBehaviour
{
  public string EnemyId = "";

  public string WaveShape = "";

  public int WaveCount = 1;

  public float WaveInterval = 0;

  public int EnemyAmountPerWave = 1;

  public float WaitTime = 0f;

  public float OriginAngle = 0f;

  public Vector3 WaveOffset = Vector3.zero;

  public float WaveOffsetAngle = 0f;

  public bool InverseX = false;

  public bool InverseY = false;

  public bool InverseZ = false;

  private void OnDrawGizmosSelected()
  {
    Gizmos.color = new Color(1, 0, 0, 0.5f);
    Gizmos.DrawCube(transform.position, transform.localScale);
  }
}

#if UNITY_EDITOR

[CustomEditor(typeof(EnemyWaveSettings))]
public class EnemyWaveSettingsEditor : Editor
{
  SerializedObject settings;

  // Properties
  SerializedProperty enemyId;
  SerializedProperty waveShape;
  SerializedProperty waveCount;
  SerializedProperty waveInterval;
  SerializedProperty enemyAmountPerWave;
  SerializedProperty waitTime;
  SerializedProperty originAngle;
  SerializedProperty waveOffsetAngle;
  SerializedProperty waveOffset;
  SerializedProperty inverseX;
  SerializedProperty inverseY;
  SerializedProperty inverseZ;


  // GUI parameters
  private int waveShapeIndex = 0;
  private string[] waveShapeOptions = {
    "None",
    "Point",
    "Circle",
    "Line",
    "Random",
  };



  private void OnEnable()
  {
    settings           = new SerializedObject(target);
    enemyId            = settings.FindProperty("EnemyId");
    waveShape          = settings.FindProperty("WaveShape");
    waveCount          = settings.FindProperty("WaveCount");
    waveInterval       = settings.FindProperty("WaveInterval");
    enemyAmountPerWave = settings.FindProperty("EnemyAmountPerWave");
    waitTime           = settings.FindProperty("WaitTime");
    originAngle        = settings.FindProperty("OriginAngle");
    waveOffsetAngle    = settings.FindProperty("WaveOffsetAngle");
    waveOffset         = settings.FindProperty("WaveOffset");
    inverseX           = settings.FindProperty("InverseX");
    inverseY           = settings.FindProperty("InverseY");
    inverseZ           = settings.FindProperty("InverseZ");


    for (int i = 0; i < waveShapeOptions.Length; i++) {
      if (waveShape.stringValue == waveShapeOptions[i]) {
        waveShapeIndex = i;
        break;
      }
    }
  }

  public override void OnInspectorGUI()
  {
    settings.Update();

    waveShapeIndex = EditorGUILayout.Popup(
      label: "Wave‚ÌŒ`ó",
      selectedIndex: waveShapeIndex,
      displayedOptions: waveShapeOptions
    );

    waveShape.stringValue = waveShapeOptions[waveShapeIndex];

    EditorGUILayout.PropertyField(waitTime);

    if (waveShape.stringValue != "None") 
    {
      EditorGUILayout.PropertyField(enemyId);
      EditorGUILayout.PropertyField(waveCount);
      EditorGUILayout.PropertyField(waveInterval);
      EditorGUILayout.PropertyField(enemyAmountPerWave);
      EditorGUILayout.PropertyField(waveOffset);
    }

    if (waveShape.stringValue == "Circle") {
      EditorGUILayout.PropertyField(originAngle);
      EditorGUILayout.PropertyField(waveOffsetAngle);
    }

    if (waveShape.stringValue == "Line") {
      EditorGUILayout.PropertyField(inverseX);
      EditorGUILayout.PropertyField(inverseY);
      EditorGUILayout.PropertyField(inverseZ);
    }

    settings.ApplyModifiedProperties();
  }
}

#endif