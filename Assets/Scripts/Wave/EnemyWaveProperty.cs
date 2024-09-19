using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

/// <summary>
/// 敵Waveパラメータ、Waveの形状や出現する敵の数や位置などの情報を保持する
/// </summary>
[System.Serializable]
public class EnemyWaveProperty
{
  //============================================================================
  // Inspector Variables
  //============================================================================
  /// <summary>
  /// 出現する敵のID
  /// </summary>
  public string EnemyId = "";

  /// <summary>
  /// Waveの役割ID
  /// </summary>
  public string RoleId = "";

  /// <summary>
  /// Wave数
  /// </summary>
  public int WaveCount = 1;

  /// <summary>
  /// 次のWaveが発動するまでの間隔
  /// </summary>
  public float WaveInterval = 0f;

  /// <summary>
  /// 1つのWaveが生成する敵の数
  /// </summary>
  public int EnemyAmountPerWave = 1;

  /// <summary>
  /// 初回Waveが発動するまでの待機時間
  /// </summary>
  public float WaitTime = 0f;

  /// <summary>
  /// 基準角度
  /// </summary>
  [Range(-360f, 360f)]
  public float OriginAngle = 0f;

  //============================================================================
  // Variables
  //============================================================================

  /// <summary>
  /// 敵のレベル
  /// </summary>
  public int EnemyLv = 1;

  //============================================================================
  // Properities
  //============================================================================

  /// <summary>
  /// 出現する敵ID
  /// </summary>
  public EnemyId Id {
    get { return MyEnum.Parse<EnemyId>(EnemyId); }
  }

  /// <summary>
  /// Waveの形状
  /// </summary>
  public EnemyWaveRole Role {
    get { return MyEnum.Parse<EnemyWaveRole>(RoleId); }
  }

  /// <summary>
  /// Waveが生成する敵の合計数
  /// </summary>
  public int TotalEnemyCount {
    get { return WaveCount * EnemyAmountPerWave; }
  }



  //============================================================================
  // Methods
  //============================================================================

  /// <summary>
  /// インスタンスを複製する
  /// </summary>
  public EnemyWaveProperty Clone()
  {
    var prop = new EnemyWaveProperty();

    prop.EnemyId            = this.EnemyId;
    prop.RoleId             = this.RoleId;
    prop.EnemyLv            = this.EnemyLv;
    prop.WaveCount          = this.WaveCount;
    prop.WaveInterval       = this.WaveInterval;
    prop.EnemyAmountPerWave = this.EnemyAmountPerWave;
    prop.WaitTime           = this.WaitTime;
    prop.OriginAngle        = this.OriginAngle;

    return prop;
  }
}

#if UNITY_EDITOR

[CustomPropertyDrawer(typeof(EnemyWaveProperty))]
public class EnemyWavePropertyDrawer : PropertyDrawer
{
  private bool initialized = false;

  // GUI parameters
  private int enemyWaveRoleIndex = 0;

  private string[] enemyWaveRoleOptions = {
    "Wait",
    "Random",
    "Circle",
    "Forward",
    "Backword",
    "Left",
    "Right",
  };

  private int displayedPropertyCount = 0;

  // Properties
  private SerializedProperty enemyId;
  private SerializedProperty roleId;
  private SerializedProperty waveCount;
  private SerializedProperty waveInterval;
  private SerializedProperty enemyAmountPerWave;
  private SerializedProperty waitTime;
  private SerializedProperty originAngle;

  private void Initialize(Rect position, SerializedProperty property, GUIContent label)
  {
    enemyId            = property.FindPropertyRelative("EnemyId");
    roleId             = property.FindPropertyRelative("RoleId");
    waveCount          = property.FindPropertyRelative("WaveCount");
    waveInterval       = property.FindPropertyRelative("WaveInterval");
    enemyAmountPerWave = property.FindPropertyRelative("EnemyAmountPerWave");
    waitTime           = property.FindPropertyRelative("WaitTime");
    originAngle        = property.FindPropertyRelative("OriginAngle");

    for (int i = 0; i < enemyWaveRoleOptions.Length; i++) {
      if (roleId.stringValue == enemyWaveRoleOptions[i]) {
        enemyWaveRoleIndex = i;
        break;
      }
    }
  }

  public override void OnGUI(Rect p, SerializedProperty property, GUIContent label)
  {
    if (!initialized) 
    {
      Initialize(p, property, label);
      initialized = true;
    }

    EditorGUI.BeginProperty(p, label, property);


    EditorGUI.LabelField(rect(p, 0), label);

    enemyWaveRoleIndex = EditorGUI.Popup(rect(p, 1), "Waveの役割", enemyWaveRoleIndex, enemyWaveRoleOptions);
    roleId.stringValue = enemyWaveRoleOptions[enemyWaveRoleIndex];

    EditorGUI.PropertyField(rect(p, 2), waitTime);

    displayedPropertyCount = 3;

    if (roleId.stringValue != "Wait")
    {
      EditorGUI.PropertyField(rect(p, 3), enemyId);
      EditorGUI.PropertyField(rect(p, 4), waveCount);
      EditorGUI.PropertyField(rect(p, 5), waveInterval);
      EditorGUI.PropertyField(rect(p, 6), enemyAmountPerWave);
      EditorGUI.PropertyField(rect(p, 7), originAngle);
      
      displayedPropertyCount = 8;
    }

    EditorGUI.EndProperty();
  }

  

  public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
  {
    float h = EditorGUIUtility.singleLineHeight;
    float s = EditorGUIUtility.standardVerticalSpacing;

    
    return (h + s) * displayedPropertyCount;
  }

  private float OffsetY(int lineNo)
  {
    return (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing) * lineNo;
  }

  private Rect rect(Rect position, int lineNo)
  {
    // 各フィールドの高さを計算
    float lineHeight = EditorGUIUtility.singleLineHeight;
    float spacing    = EditorGUIUtility.standardVerticalSpacing;

    float y = position.y + (lineHeight + spacing) * lineNo;
    return new Rect(position.x, y, position.width, lineHeight);
  }
}
#endif