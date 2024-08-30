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
  // Variables
  //============================================================================
  /// <summary>
  /// 出現する敵のID
  /// </summary>
  public string EnemyId = "";

  /// <summary>
  /// Waveの形状ID
  /// </summary>
  public string ShapeId = "";

  /// <summary>
  /// Waveの起点
  /// </summary>
  public Vector3 BasePosition = Vector3.zero;

  /// <summary>
  /// Waveが敵を生成するエリア
  /// </summary>
  public Vector3 Area = Vector3.zero;

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
  /// 起点となる角度、このパラメータはShape.Circleのときのみ有効
  /// </summary>
  public float OriginAngle = 0f;

  /// <summary>
  /// Wave毎に適用される角度のオフセット
  /// </summary>
  public float WaveOffsetAngle = 0f;

  /// <summary>
  /// Wave毎に適用されるオフセット、zは角度に使う
  /// </summary>
  public Vector3 WaveOffset = Vector3.zero;

  /// <summary>
  /// 出現位置のX方向を反転する
  /// このパラメーターはSphae.Lineのときのみ有効
  /// </summary>
  public bool InverseX = false;
  /// <summary>
  /// 出現位置のY方向を反転する
  /// このパラメーターはSphae.Lineのときのみ有効
  /// </summary>
  public bool InverseY = false;

  /// <summary>
  /// 出現位置のZ方向を反転する
  /// このパラメーターはSphae.Lineのときのみ有効
  /// </summary>
  public bool InverseZ = false;

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
  public WaveShape Shape {
    get { return MyEnum.Parse<WaveShape>(ShapeId); }
  }

  /// <summary>
  /// Waveが生成する敵の合計数
  /// </summary>
  public int TotalEnemyCount {
    get { return WaveCount * EnemyAmountPerWave; }
  }

  /// <summary>
  /// Wave毎に適用されるX方向オフセット
  /// </summary>
  public float WaveOffsetX {
    get { return WaveOffset.x; }
    set { WaveOffset.x = value; }
  }

  /// <summary>
  /// Wave毎に適用されるY方向オフセット
  /// </summary>
  public float WaveOffsetY {
    get { return WaveOffset.y; }
    set { WaveOffset.y = value; }
  }

  /// <summary>
  /// Wave毎に適用されるZ方向オフセット
  /// </summary>
  public float WaveOffsetZ {
    get { return WaveOffset.z; }
    set { WaveOffset.z = value; }
  }

  //============================================================================
  // Methods
  //============================================================================

  /// <summary>
  /// インスタンスを複製する
  /// </summary>
  public EnemyWaveProperty Clone()
  {
    var param = new EnemyWaveProperty();

    param.EnemyId = this.EnemyId;
    param.ShapeId = this.ShapeId;
    param.BasePosition = this.BasePosition;
    param.Area = this.Area;
    param.WaveCount = this.WaveCount;
    param.WaveInterval = this.WaveInterval;
    param.EnemyAmountPerWave = this.EnemyAmountPerWave;
    param.WaitTime = this.WaitTime;
    param.OriginAngle = this.OriginAngle;
    param.WaveOffsetAngle = this.WaveOffsetAngle;
    param.WaveOffset = this.WaveOffset;
    param.InverseX = this.InverseX;
    param.InverseY = this.InverseY;
    param.InverseZ = this.InverseZ;

    return param;
  }
}

#if UNITY_EDITOR

[CustomPropertyDrawer(typeof(EnemyWaveProperty))]
public class EnemyWavePropertyDrawer : PropertyDrawer
{
  private bool initialized = false;
  // GUI parameters
  private int waveShapeIndex = 0;
  private string[] waveShapeOptions = {
    "None",
    "Point",
    "Circle",
    "Line",
    "Random",
  };

  private int displayedPropertyCount = 0;

  // Properties
  private SerializedProperty enemyId;
  private SerializedProperty shapeId;
  private SerializedProperty waveCount;
  private SerializedProperty waveInterval;
  private SerializedProperty enemyAmountPerWave;
  private SerializedProperty waitTime;
  private SerializedProperty originAngle;
  private SerializedProperty waveOffsetAngle;
  private SerializedProperty waveOffset;
  private SerializedProperty inverseX;
  private SerializedProperty inverseY;
  private SerializedProperty inverseZ;
  

  private void Initialize(Rect position, SerializedProperty property, GUIContent label)
  {
    enemyId            = property.FindPropertyRelative("EnemyId");
    shapeId            = property.FindPropertyRelative("ShapeId");
    waveCount          = property.FindPropertyRelative("WaveCount");
    waveInterval       = property.FindPropertyRelative("WaveInterval");
    enemyAmountPerWave = property.FindPropertyRelative("EnemyAmountPerWave");
    waitTime           = property.FindPropertyRelative("WaitTime");
    originAngle        = property.FindPropertyRelative("OriginAngle");
    waveOffsetAngle    = property.FindPropertyRelative("WaveOffsetAngle");
    waveOffset         = property.FindPropertyRelative("WaveOffset");
    inverseX           = property.FindPropertyRelative("InverseX");
    inverseY           = property.FindPropertyRelative("InverseY");
    inverseZ           = property.FindPropertyRelative("InverseZ");

    for (int i = 0; i < waveShapeOptions.Length; i++) {
      if (shapeId.stringValue == waveShapeOptions[i]) {
        waveShapeIndex = i;
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

    waveShapeIndex = EditorGUI.Popup(rect(p, 1), "Waveの形状", waveShapeIndex, waveShapeOptions);
    shapeId.stringValue = waveShapeOptions[waveShapeIndex];

    EditorGUI.PropertyField(rect(p, 2), waitTime);

    displayedPropertyCount = 3;

    if (shapeId.stringValue != "None")
    {
      EditorGUI.PropertyField(rect(p, 3), enemyId);
      EditorGUI.PropertyField(rect(p, 4), waveCount);
      EditorGUI.PropertyField(rect(p, 5), waveInterval);
      EditorGUI.PropertyField(rect(p, 6), enemyAmountPerWave);
      EditorGUI.PropertyField(rect(p, 7), waveOffset);

      displayedPropertyCount = 8;
    }

    if (shapeId.stringValue == "Circle") {
      EditorGUI.PropertyField(rect(p, 8), originAngle);
      EditorGUI.PropertyField(rect(p, 9), waveOffsetAngle);
      displayedPropertyCount = 10;
    }

    if (shapeId.stringValue == "Line") {
      EditorGUI.PropertyField(rect(p, 8), inverseX);
      EditorGUI.PropertyField(rect(p, 9), inverseY);
      EditorGUI.PropertyField(rect(p, 10), inverseZ);
      displayedPropertyCount = 11;
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