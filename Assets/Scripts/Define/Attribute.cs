/// <summary>
/// 属性
/// </summary>
public enum Attribute
{
  Nil = 0,      // 設定なし
  Non = 1 << 0, // 無
  Fir = 1 << 1, // 火
  Wat = 1 << 2, // 水
  Thu = 1 << 3, // 雷
  Ice = 1 << 4, // 氷
  Lef = 1 << 5, // 草
  Win = 1 << 6, // 風
  Hol = 1 << 7, // 聖
  Dar = 1 << 8, // 闇
}

public static class AttributeUtil
{
  /// <summary>
  /// 属性文字列から属性に変換する
  /// </summary>
  public static uint GetAttributesFromString(string attributesString)
  {
    string[] words = attributesString.Split("|");

    uint flag = 0;

    foreach(string word in words) 
    {
      if (MyEnum.TryParse<Attribute>(word, out var attr)) {
        flag |= (uint)attr;
      }else {
        Logger.Error($"{word} attribute parse failed.");
      }
    }

    return flag;
  }
}