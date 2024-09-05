using UnityEngine;

public enum Attribute
{
  Nil = 0,      // 設定なし
  Non = 1 << 0, // 無
  Fir = 1 << 1, // 火
  Wat = 1 << 2, // 水
  Thu = 1 << 3, // 雷
  Ice = 1 << 4, // 氷
  Tre = 1 << 5, // 木
  Hol = 1 << 6, // 聖
  Dar = 1 << 7, // 闇
}



public class AttackStatus
{

  private float power = 0f;
  private Flag32 attributes = new Flag32();

  public void Init(float power, uint attributes)
  {
    this.power            = power;
    this.attributes.Value = attributes;
  }

  public uint Attributes {
    get { return attributes.Value; }
  }

  public float Power 
  {
    get {
      if (attributes.Is((uint)Attribute.Non)) {
        return power;
      } else {
        return power * 1.2f;
      }
    }
  }
}
